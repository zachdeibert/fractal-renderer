using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery {
    [AttributeUsage(AttributeTargets.Class)]
    public class FractalAttribute : Attribute {
        public readonly string Name;

        public FractalAttribute(string name) {
            Name = name;
        }
    }
}
