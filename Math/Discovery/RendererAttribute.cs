using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery {
    [AttributeUsage(AttributeTargets.Class)]
    public class RendererAttribute : Attribute {
        public readonly string Name;
        public readonly CoordinateSystems CoordinateSystem;

        public RendererAttribute(string name, CoordinateSystems coordinateSystem) {
            Name = name;
            CoordinateSystem = coordinateSystem;
        }
    }
}
