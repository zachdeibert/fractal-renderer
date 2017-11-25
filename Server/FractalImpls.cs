using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2.Raycasting;
using Com.GitHub.ZachDeibert.FractalRenderer.Model;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class FractalImpls {
        public FractalConfig Config {
            get {
                return new FractalConfig {
                    Colorer = Colorer.GetType(),
                    Transformer = Transformer.GetType(),
                    Fractal = Fractal.GetType(),
                    Renderer = Renderer.GetType(),
                    PartitionScalar = PartitionScalar.GetType(),
                    MaxIterations = MaxIterations,
                    ScreenWidth = ScreenWidth,
                    ScreenHeight = ScreenHeight
                };
            }
        }

        public IColorer Colorer = new BluePurpleColorer();
        public ICoordinateTransformer Transformer = new ComplexDecimalTransformer();
        public IFractal Fractal = new Mandelbrot();
        public IRenderer Renderer = new R2RaycastingRenderer();
        public ScalarBase PartitionScalar = new ScalarDecimal();
        public int MaxIterations = 100;
        public int ScreenWidth = 1920;
        public int ScreenHeight = 1080;

        public readonly PartitionManager PartitionManager;

        public FractalImpls() {
            PartitionManager = new PartitionManager(this);
        }
    }
}
