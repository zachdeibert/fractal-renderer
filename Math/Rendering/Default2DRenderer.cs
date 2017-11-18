using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class Default2DRenderer : IRenderer {
        public FractalColor RenderPixel(int x, int y, int maxX, int maxY, RenderContext ctx) {
            return ctx.DetermineColor(new DisplayCoordinate(
                    (Double1) (((double) x) * 4.0 / ((double) maxX) - 2.0),
                    (Double1) (((double) y) * 4.0 / ((double) maxY) - 2.0)
            ));
        }
    }
}
