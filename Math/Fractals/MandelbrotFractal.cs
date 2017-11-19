using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals {
    public class MandelbrotFractal : IFractal {
        public bool TryEscape(NumberBase c, int maxIterations, out int iterations, out IConvertible magnitude) {
            int i = 0;
            NumberBase z = c - c;
            while (z.Magnitude().ToDouble(null) < 2.0 && i < maxIterations) {
                z = z * z + c;
                ++i;
            }
            iterations = i;
            magnitude = z.Magnitude();
            return magnitude.ToDouble(null) >= 2.0;
        }
    }
}
