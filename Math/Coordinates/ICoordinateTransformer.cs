using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates {
    public interface ICoordinateTransformer {
        NumberBase Convert(ScalarBase[] coordinate);
    }
}
