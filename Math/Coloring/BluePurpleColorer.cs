using System;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring {
    [Colorer("Blue/Purple")]
    public class BluePurpleColorer : IColorer {
        const int Scale = 32;

        public uint DetermineColor(bool escaped, NumberBase point, NumberBase escapePoint, int iterations) {
            if (escaped) {
                uint factor = (uint) System.Math.Max(System.Math.Min(System.Math.Abs(256 - (iterations * Scale) % 512), 255), 0);
                //return (factor << 24) | 0x0000FFFF;
                return factor | 0xFFFF0000;
            } else {
                //return 0x0F002FFF;
                return 0xFF2F000F;
            }
        }
    }
}
