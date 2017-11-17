using System;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class RenderedFractal {
        public const int Width = 1920;
        public const int Height = 1080;
        readonly byte[] Data;

        public byte[] Export(int x, int y, int width, int height) {
            return BitConverter.GetBytes(x)
                    .Concat(BitConverter.GetBytes(y))
                    .Concat(BitConverter.GetBytes(width))
                    .Concat(BitConverter.GetBytes(height))
                    .Concat(Data).ToArray();
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
        }

        public void SetPixel(int x, int y, FractalColor color) {
            SetPixelLinear((x + Width * y) * 4, color);
        }

        public RenderedFractal() {
            Data = new byte[Width * Height * 4];
        }
    }
}
