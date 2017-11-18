using System;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors {
    public class FractalColor {
        int r;
        int g;
        int b;

        public int R {
            get {
                return r;
            }
            set {
                if (value < 0 || value > 255) {
                    throw new ArgumentOutOfRangeException(nameof(R));
                }
                r = value;
            }
        }

        public int G {
            get {
                return g;
            }
            set {
                if (value < 0 || value > 255) {
                    throw new ArgumentOutOfRangeException(nameof(G));
                }
                g = value;
            }
        }

        public int B {
            get {
                return b;
            }
            set {
                if (value < 0 || value > 255) {
                    throw new ArgumentOutOfRangeException(nameof(B));
                }
                b = value;
            }
        }
    }
}
