using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals {
    [Fractal("Mandelbrot")]
    public class Mandelbrot : IFractal {
        public bool TestPoint(NumberBase c, int maxIterations, out NumberBase fz, out int iterations) {
            NumberBase z = c - c;
            int i;
            for (i = 0; z.Magnitude().ToDouble(null) < 2 && i < maxIterations; ++i) {
                z = z * z + c;
            }
            fz = z;
            iterations = i;
            return z.Magnitude().ToDouble(null) >= 2;
        }
    }
}
