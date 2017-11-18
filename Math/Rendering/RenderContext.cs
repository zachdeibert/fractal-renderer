using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class RenderContext {
        readonly int Dimensions;
        readonly IDomainTransformer Transformer;
        readonly IFractal Fractal;
        readonly IColorer Colorer;
        readonly int MaxIterations;
        readonly ColorHistogram Histogram;

        public FractalColor DetermineColor(DisplayCoordinate coordinate) {
            if (coordinate.Dimensions != Dimensions) {
                throw new ArgumentException("Coordinate has wrong number of dimensions");
            }
            NumberBase c = Transformer.Transform(coordinate);
            int iterations;
            bool escaped = Fractal.TryEscape(c, MaxIterations, out iterations);
            return Colorer.Color(escaped, iterations, Histogram);
        }

        public RenderContext(int dimensions, IDomainTransformer transformer, IFractal fractal, IColorer colorer, int maxIterations) {
            Dimensions = dimensions;
            Transformer = transformer;
            Fractal = fractal;
            Colorer = colorer;
            MaxIterations = maxIterations;
            Histogram = new ColorHistogram();
            if (!Transformer.SupportsDisplayDimensions(Dimensions)) {
                throw new ArgumentException("Transformer does not support display dimensions");
            }
        }
    }
}
