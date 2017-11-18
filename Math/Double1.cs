using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public class Double1 : NumberBase<Double1> {
        public override int Dimensions {
            get {
                return 1;
            }
        }
        public double Value;

        protected override Double1 Add(Double1 o) {
            return Value + o.Value;
        }

        protected override Double1 Subtract(Double1 o) {
            return Value - o.Value;
        }

        protected override Double1 Multiply(Double1 o) {
            return Value * o.Value;
        }

        protected override Double1 Divide(Double1 o) {
            return Value / o.Value;
        }

        public override IConvertible Magnitude() {
            return System.Math.Abs(Value);
        }

        protected override long ToInt64() {
            return (long) Value;
        }

        protected override ulong ToUInt64() {
            return (ulong) Value;
        }

        protected override decimal ToDecimal() {
            return (decimal) Value;
        }

        public static implicit operator Double1(double val) {
            return new Double1 {
                Value = val
            };
        }

        public static implicit operator double(Double1 val) {
            return val.Value;
        }
    }
}
