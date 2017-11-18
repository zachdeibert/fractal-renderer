using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates {
    public class DecimalImaginaryPlaneTransformer : IDomainTransformer {
        public bool SupportsDisplayDimensions(int dimensions) {
            return dimensions == 2;
        }

        public NumberBase Transform(DisplayCoordinate coordinate) {
            return new ComplexDecimal {
                Real = coordinate.X.ToDecimal(null),
                Imaginary = coordinate.Y.ToDecimal(null)
            };
        }
    }
}
