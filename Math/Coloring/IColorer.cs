using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring {
    public interface IColorer {
        uint DetermineColor(bool escaped, NumberBase point, NumberBase escapePoint, int iterations);
    }
}
