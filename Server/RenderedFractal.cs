using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Model;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class RenderedFractal {
        public const int Width = 1920;
        public const int Height = 1080;
        readonly byte[] Data;
        readonly Dictionary<int, Rectangle> DirtyRegion;
        readonly object RegionLock;
        readonly Random FrameBufferIdRandom;
        public readonly FractalConfig Config;

        byte[] Export(int x, int y, int width, int height) {
            byte[] buffer = new byte[16 + width * height * 4];
            BitConverter.GetBytes(x).CopyTo(buffer, 0);
            BitConverter.GetBytes(y).CopyTo(buffer, 4);
            BitConverter.GetBytes(width).CopyTo(buffer, 8);
            BitConverter.GetBytes(height).CopyTo(buffer, 12);
            for (int dx = 0; dx < width; ++dx) {
                for (int dy = 0; dy < height; ++dy) {
                    int oldOffset = (dx + x + Width * (dy + y)) * 4;
                    int newOffset = (dx + width * dy) * 4 + 16;
                    for (int i = 0; i < 4; ++i) {
                        buffer[newOffset + i] = Data[oldOffset + i];
                    }
                }
            }
            return buffer;
        }

        public byte[] ExportKeyFrame(ref int fbid) {
            lock (RegionLock) {
                if (!DirtyRegion.ContainsKey(fbid)) {
                    fbid = FrameBufferIdRandom.Next();
                    DirtyRegion[fbid] = null;
                }
            }
            return Export(0, 0, Width, Height);
        }

        public byte[] CleanRegion(ref int fbid) {
            lock (RegionLock) {
                if (!DirtyRegion.ContainsKey(fbid)) {
                    fbid = FrameBufferIdRandom.Next();
                    DirtyRegion[fbid] = null;
                }
                Rectangle region = DirtyRegion[fbid];
                if (region == null) {
                    return new byte[16];
                } else {
                    byte[] data = Export(region.Left, region.Top, region.Width, region.Height);
                    DirtyRegion[fbid] = null;
                    return data;
                }
            }
        }

        public void DisconnectFrameBuffer(ref int fbid) {

        }

        void SetPixelLinear(int i, FractalColor color) {
            Data[i] = (byte) color.R;
            Data[i + 1] = (byte) color.G;
            Data[i + 2] = (byte) color.B;
            Data[i + 3] = 255;
        }

        public void Clear(FractalColor color) {
            for (int i = 0; i < Data.Length; i += 4) {
                SetPixelLinear(i, color);
            }
            lock (RegionLock) {
                foreach (int fbid in DirtyRegion.Keys.ToArray()) {
                    DirtyRegion[fbid] = new Rectangle {
                        Left = 0,
                        Top = 0,
                        Right = Width - 1,
                        Bottom = Height - 1
                    };
                }
            }
        }

        public void SetPixel(int x, int y, FractalColor color) {
            lock (RegionLock) {
                SetPixelLinear((x + Width * y) * 4, color);
                foreach (int fbid in DirtyRegion.Keys.ToArray()) {
                    DirtyRegion[fbid] += new Rectangle {
                        Left = x,
                        Top = y,
                        Right = x,
                        Bottom = y
                    };
                }
            }
        }

        public void SetRegion(int x, int y, int width, int height, byte[] data, int off) {
            lock (RegionLock) {
                for (int dx = 0; dx < width; ++dx) {
                    for (int dy = 0; dy < height; ++dy) {
                        int oldOffset = (dx + width * dy) * 4 + off;
                        int newOffset = (dx + x + Width * (dy + y)) * 4;
                        for (int i = 0; i < 4; ++i) {
                            Data[newOffset + i] = data[oldOffset + i];
                        }
                    }
                }
                foreach (int fbid in DirtyRegion.Keys.ToArray()) {
                    DirtyRegion[fbid] += new Rectangle {
                        Left = x,
                        Top = y,
                        Right = x + width - 1,
                        Bottom = y + height - 1
                    };
                }
            }
        }

        public RenderedFractal() {
            Data = new byte[Width * Height * 4];
            DirtyRegion = new Dictionary<int, Rectangle>();
            RegionLock = new object();
            FrameBufferIdRandom = new Random();
            Config = new FractalConfig();
        }
    }
}
