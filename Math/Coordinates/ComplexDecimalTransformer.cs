using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates {
    [CoordinateTransformer("R2 -> Complex (128-bit float)")]
    [SupportedCoordinateSystems(CoordinateSystems.R2)]
    public class ComplexDecimalTransformer : ICoordinateTransformer {
        public NumberBase Convert(ScalarBase[] coordinate) {
            return new ComplexDecimal {
                Real = coordinate[0].ToDecimal(null),
                Imaginary = coordinate[1].ToDecimal(null)
            };
        }
    }
}
