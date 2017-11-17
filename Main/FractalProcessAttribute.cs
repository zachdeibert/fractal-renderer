using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    public class FractalProcessAttribute : Attribute {
        public readonly string Name;

        public FractalProcessAttribute(string name) {
            Name = name;
        }
    }
}
