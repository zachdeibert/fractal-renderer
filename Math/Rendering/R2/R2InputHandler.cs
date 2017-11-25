using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2 {
    public class R2InputHandler : IInputHandler {
        public ScalarBase Left;
        public ScalarBase Top;
        public ScalarBase Right;
        public ScalarBase Bottom;

        public void Swipe(int x, int y, int dx, int dy) {
            throw new NotImplementedException();
        }

        public R2InputHandler(FrameBuffer fb, ScalarBase type) {
            Top = Left = type.Load(-2);
            Right = Bottom = type.Load(2);
        }
    }
}
