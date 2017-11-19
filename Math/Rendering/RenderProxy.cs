using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class RenderProxy {
        readonly IRenderer Renderer;
        readonly RenderContext Context;

        public FractalColor RenderPixel(int x, int y, int maxX, int maxY) {
            return Renderer.RenderPixel(x, y, maxX, maxY, Context);
        }

        public RenderProxy(IRenderer renderer, RenderContext context) {
            Renderer = renderer;
            Context = context;
        }
    }
}
