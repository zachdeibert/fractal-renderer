using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2 {
    public class R2InputHandler : IInputHandler {
        public ScalarBase Left;
        public ScalarBase Top;
        public ScalarBase Right;
        public ScalarBase Bottom;
        readonly IR2Renderer Renderer;

        public void Swipe(int dx, int dy) {
            FrameBuffer newBuffer = new FrameBuffer(Renderer.FrameBuffer.Width, Renderer.FrameBuffer.Height);
            newBuffer.RawUpdate(pixel => {
                for (int x = 0; x < Renderer.FrameBuffer.Width; ++x) {
                    int nx = x + dx;
                    if (nx >= 0 && nx < Renderer.FrameBuffer.Width) {
                        for (int y = 0; y < Renderer.FrameBuffer.Height; ++y) {
                            int ny = y + dy;
                            if (ny >= 0 && ny < Renderer.FrameBuffer.Height) {
                                pixel(nx, ny, Renderer.FrameBuffer.GetPixel(x, y), 0);
                            }
                        }
                    }
                }
            });
            Renderer.FrameBuffer.CopyFrom(newBuffer);
            ScalarBase type = Left - Left;
            ScalarBase sdx = type.Load(dx) * (Left - Right) / type.Load(Renderer.FrameBuffer.Width);
            ScalarBase sdy = type.Load(dy) * (Left - Right) / type.Load(Renderer.FrameBuffer.Height);
            Left += sdx;
            Top += sdy;
            Right += sdx;
            Bottom += sdy;
            int dxp;
            int dxm;
            if (dx < 0) {
                Renderer.InvalidateArea(Renderer.FrameBuffer.Width + dx, 0, -dx, Renderer.FrameBuffer.Height, type);
                dxp = 0;
                dxm = -dx;
            } else if (dx > 0) {
                Renderer.InvalidateArea(0, 0, dx, Renderer.FrameBuffer.Height, type);
                dxm = dxp = dx;
            } else {
                dxm = dxp = 0;
            }
            if (dy < 0) {
                Renderer.InvalidateArea(dxp, Renderer.FrameBuffer.Height + dy, Renderer.FrameBuffer.Width - dxm, -dy, type);
            } else {
                Renderer.InvalidateArea(dxp, 0, Renderer.FrameBuffer.Width - dxm, dy, type);
            }
        }

        public R2InputHandler(IR2Renderer renderer, ScalarBase type) {
            Top = Left = type.Load(-2);
            Right = Bottom = type.Load(2);
            Renderer = renderer;
        }
    }
}
