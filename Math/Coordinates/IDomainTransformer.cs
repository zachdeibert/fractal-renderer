using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates {
    public interface IDomainTransformer {
        bool SupportsDisplayDimensions(int dimensions);

        NumberBase Transform(DisplayCoordinate coordinate);
    }
}
