using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery {
    [AttributeUsage(AttributeTargets.Class)]
    public class SupportedCoordinateSystemsAttribute : Attribute {
        public readonly CoordinateSystems[] Systems;

        public SupportedCoordinateSystemsAttribute(params CoordinateSystems[] systems) {
            Systems = systems;
        }
    }
}
