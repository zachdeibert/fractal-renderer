using System;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Drawing;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class RenderedFractal {
        public const int Width = 1920;
        public const int Height = 1080;
        readonly byte[] Data;
        public Rectangle DirtyRegion;

        byte[] Export(int x, int y, int width, int height) {
            return BitConverter.GetBytes(x)
                    .Concat(BitConverter.GetBytes(y))
                    .Concat(BitConverter.GetBytes(width))
                    .Concat(BitConverter.GetBytes(height))
                    .Concat(Data).ToArray();
        }

        public byte[] ExportKeyFrame() {
            return Export(0, 0, Width, Height);
        }

        public byte[] CleanRegion() {
            byte[] data = Export(DirtyRegion.Left, DirtyRegion.Top, DirtyRegion.Width, DirtyRegion.Height);
            DirtyRegion = null;
            return data;
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
            DirtyRegion = new Rectangle {
                Left = 0,
                Top = 0,
                Right = Width,
                Bottom = Height
            };
        }

        public void SetPixel(int x, int y, FractalColor color) {
            SetPixelLinear((x + Width * y) * 4, color);
            if (DirtyRegion == null) {
                DirtyRegion = new Rectangle {
                    Left = x,
                    Top = y,
                    Right = x,
                    Bottom = y
                };
            } else {
                if (x < DirtyRegion.Left) {
                    DirtyRegion.Left = x;
                } else if (x > DirtyRegion.Right) {
                    DirtyRegion.Right = x;
                }
                if (y < DirtyRegion.Top) {
                    DirtyRegion.Top = y;
                } else if (y > DirtyRegion.Bottom) {
                    DirtyRegion.Bottom = y;
                }
            }
        }

        public RenderedFractal() {
            Data = new byte[Width * Height * 4];
        }
    }
}
