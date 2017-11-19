using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Model {
    public class Rectangle {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
        public int Width {
            get {
                return Right - Left + 1;
            }
        }
        public int Height {
            get {
                return Bottom - Top + 1;
            }
        }

        public override bool Equals(object obj) {
            if (obj is Rectangle) {
                Rectangle o = (Rectangle) obj;
                return Left == o.Left && Right == o.Right && Top == o.Top && Bottom == o.Bottom;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return Tuple.Create(Left, Right, Top, Bottom).GetHashCode();
        }

        public byte[] Serialize() {
            byte[] data = new byte[16];
            BitConverter.GetBytes(Left).CopyTo(data, 0);
            BitConverter.GetBytes(Right).CopyTo(data, 4);
            BitConverter.GetBytes(Top).CopyTo(data, 8);
            BitConverter.GetBytes(Bottom).CopyTo(data, 12);
            return data;
        }

        public static Rectangle operator +(Rectangle a, Rectangle b) {
            if (a == null) {
                return b;
            } else if (b == null) {
                return a;
            }
            return new Rectangle {
                Left = System.Math.Min(a.Left, b.Left),
                Right = System.Math.Max(a.Right, b.Right),
                Top = System.Math.Min(a.Top, b.Top),
                Bottom = System.Math.Max(a.Bottom, b.Bottom)
            };
        }

        public Rectangle(byte[] data, int off = 0) {
            Left = BitConverter.ToInt32(data, off);
            Right = BitConverter.ToInt32(data, 4 + off);
            Top = BitConverter.ToInt32(data, 8 + off);
            Bottom = BitConverter.ToInt32(data, 12 + off);
        }

        public Rectangle() {
        }
    }
}
