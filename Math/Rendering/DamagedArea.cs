using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class DamagedArea {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public byte[] Bitmap;

        public IEnumerable<byte> Serialize() {
            return BitConverter.GetBytes(X)
                  .Concat(BitConverter.GetBytes(Y))
                  .Concat(BitConverter.GetBytes(Width))
                  .Concat(BitConverter.GetBytes(Height))
                  .Concat(Bitmap);
        }
    }
}
