using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public interface IRenderer {
        FractalColor RenderPixel(int x, int y, int maxX, int maxY, RenderContext ctx);
    }
}
