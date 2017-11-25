using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class FrameBuffer {
        public readonly int Width;
        public readonly int Height;
        readonly byte[] Buffer;
        readonly Dictionary<int, DamagedArea> Videos;
        readonly object VideoLock;

        private static void PartitionedCopy(byte[] src, int srcX, int srcY, int srcWidth, byte[] dst, int dstX, int dstY, int dstWidth, int width, int height) {
            for (int dx = 0; dx < width; ++dx) {
                for (int dy = 0; dy < height; ++dy) {
                    int srcOff = (srcX + dx + srcWidth * (srcY + dy)) * 4;
                    int dstOff = (dstX + dx + dstWidth * (dstY + dy)) * 4;
                    for (int i = 0; i < 4; ++i) {
                        dst[dstOff + i] = src[srcOff + i];
                    }
                }
            }
        }

        public void StartEncodingVideo(int fbid) {
            lock (VideoLock) {
                Videos[fbid] = null;
            }
        }

        public void StopEncodingVideo(int fbid) {
            lock (VideoLock) {
                Videos.Remove(fbid);
            }
        }

        public DamagedArea EncodeKeyFrame(int fbid) {
            lock (VideoLock) {
                Videos[fbid] = null;
                DamagedArea area = new DamagedArea {
                    X = 0,
                    Y = 0,
                    Width = Width,
                    Height = Height,
                    Bitmap = new byte[Buffer.Length]
                };
                Buffer.CopyTo(area.Bitmap, 0);
                return area;
            }
        }

        public DamagedArea EncodeFrame(int fbid) {
            lock (VideoLock) {
                DamagedArea area = Videos[fbid];
                if (area == null) {
                    return new DamagedArea {
                        X = 0,
                        Y = 0,
                        Width = 0,
                        Height = 0,
                        Bitmap = new byte[0]
                    };
                } else {
                    Videos[fbid] = null;
                    area.Bitmap = new byte[area.Width * area.Height * 4];
                    PartitionedCopy(Buffer, area.X, area.Y, Width, area.Bitmap, 0, 0, area.Width, area.Width, area.Height);
                    return area;
                }
            }
        }

        public void RawUpdate(Action<Action<int, int, byte[], int>> func) {
            lock (VideoLock) {
                int minX = Width;
                int maxX = -1;
                int minY = Height;
                int maxY = -1;
                func((x, y, color, colorOff) => {
                    int off = (x + y * Width) * 4;
                    Buffer[off] = color[colorOff];
                    Buffer[off + 1] = color[colorOff + 1];
                    Buffer[off + 2] = color[colorOff + 2];
                    Buffer[off + 3] = color[colorOff + 3];
                    if (x < minX) {
                        minX = x;
                    }
                    if (x > maxX) {
                        maxX = x;
                    }
                    if (y < minY) {
                        minY = y;
                    }
                    if (y > maxY) {
                        maxY = y;
                    }
                });
                foreach (KeyValuePair<int, DamagedArea> video in Videos.ToArray()) {
                    if (video.Value == null) {
                        Videos[video.Key] = new DamagedArea {
                            X = minX,
                            Y = minY,
                            Width = maxX - minX + 1,
                            Height = maxY - minY + 1
                        };
                    } else {
                        if (video.Value.X > minX) {
                            video.Value.X = minX;
                        }
                        if (video.Value.X + video.Value.Width < maxX) {
                            video.Value.Width = maxX - minX + 1;
                        }
                        if (video.Value.Y > minY) {
                            video.Value.Y = minY;
                        }
                        if (video.Value.Y + video.Value.Height < maxY) {
                            video.Value.Height = maxY - minY + 1;
                        }
                    }
                }
            }
        }

        public byte[] GetPixel(int x, int y) {
            byte[] b = new byte[4];
            Array.Copy(Buffer, (x + Width * y) * 4, b, 0, 4);
            return b;
        }

        public void CopyFrom(FrameBuffer other) {
            lock (Videos) {
                other.Buffer.CopyTo(Buffer, 0);
                foreach (int fbid in Videos.Keys.ToArray()) {
                    Videos[fbid] = new DamagedArea {
                        X = 0,
                        Y = 0,
                        Width = Width,
                        Height = Height
                    };
                }
            }
        }

        public FrameBuffer(int width, int height) {
            Width = width;
            Height = height;
            Buffer = new byte[4 * Width * Height];
            for (int i = 0; i < Buffer.Length; ++i) {
                Buffer[i] = 0;
                Buffer[++i] = 0;
                Buffer[++i] = 0;
                Buffer[++i] = 255;
            }
            Videos = new Dictionary<int, DamagedArea>();
            VideoLock = new object();
        }
    }
}
