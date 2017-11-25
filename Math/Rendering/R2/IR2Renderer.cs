using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2 {
    public interface IR2Renderer {
        FrameBuffer FrameBuffer {
            get;
        }

        void InvalidateArea(int x, int y, int width, int height, ScalarBase type);
    }
}
