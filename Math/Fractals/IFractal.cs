using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals {
    public interface IFractal {
        bool TryEscape(NumberBase coordinate, int maxIterations, out int iterations, out IConvertible magnitude);
    }
}
