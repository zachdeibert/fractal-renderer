using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery {
    [AttributeUsage(AttributeTargets.Class)]
    public class ColorerAttribute : Attribute {
        public readonly string Name;

        public ColorerAttribute(string name) {
            Name = name;
        }
    }
}
