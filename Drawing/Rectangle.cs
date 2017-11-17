using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Drawing {
    public class Rectangle {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public int Width {
            get {
                return Right - Left;
            }
        }
        public int Height {
            get {
                return Bottom - Top;
            }
        }
    }
}
