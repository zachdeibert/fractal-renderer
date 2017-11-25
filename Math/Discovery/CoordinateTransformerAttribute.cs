using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery {
    [AttributeUsage(AttributeTargets.Class)]
    public class CoordinateTransformerAttribute : Attribute {
        public readonly string Name;

        public CoordinateTransformerAttribute(string name) {
            Name = name;
        }
    }
}
