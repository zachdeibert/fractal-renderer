using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public interface IInputHandler {
        void Swipe(int x, int y, int dx, int dy);
    }
}
