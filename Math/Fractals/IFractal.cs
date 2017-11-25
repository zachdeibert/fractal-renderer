using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals {
    public interface IFractal {
        bool TestPoint(NumberBase point, int maxIterations, out NumberBase escapePoint, out int iterations);
    }
}
