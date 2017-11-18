using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates {
    public class DisplayCoordinate {
        public readonly int Dimensions;
        public readonly NumberBase[] Coordinates;

        public NumberBase X {
            get {
                return Coordinates[0];
            }
        }

        public NumberBase Y {
            get {
                return Coordinates[1];
            }
        }

        public NumberBase Z {
            get {
                return Coordinates[2];
            }
        }

        public NumberBase W {
            get {
                return Coordinates[3];
            }
        }

        public DisplayCoordinate(params NumberBase[] coordinates) {
            Dimensions = coordinates.Length;
            Coordinates = coordinates;
        }
    }
}
