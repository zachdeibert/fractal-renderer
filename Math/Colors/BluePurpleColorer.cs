using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors {
    public class BluePurpleColorer : IColorer {
        const int Scale = 32;

        public FractalColor Color(bool escaped, int iterations, IConvertible magnitude, ColorHistogram histogram) {
            if (escaped) {
                return new FractalColor {
                    R = System.Math.Min(System.Math.Abs(256 - (iterations * Scale) % 512), 255),
                    G = 0,
                    B = 255
                };
            } else {
                return new FractalColor {
                    R = 15,
                    G = 0,
                    B = 47
                };
            }
        }
    }
}
