using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors {
    public interface IColorer {
        FractalColor Color(bool escaped, int iterations, IConvertible magnitude, ColorHistogram histogram);
    }
}
