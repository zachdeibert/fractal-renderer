using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public class ComplexDecimal : NumberBase<ComplexDecimal> {
        public override int Dimensions {
            get {
                return 2;
            }
        }

        public decimal Real;
        public decimal Imaginary;

        protected override ComplexDecimal Add(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real + o.Real,
                Imaginary = Imaginary + o.Imaginary
            };
        }

        protected override ComplexDecimal Subtract(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real - o.Real,
                Imaginary = Imaginary - o.Imaginary
            };
        }

        protected override ComplexDecimal Multiply(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real * o.Real - Imaginary * o.Imaginary,
                Imaginary = Real * o.Imaginary + Imaginary * o.Real
            };
        }

        protected override ComplexDecimal Divide(ComplexDecimal o) {
            // http://mathworld.wolfram.com/ComplexDivision.html
            decimal denominator = o.Real * o.Real + o.Imaginary * o.Imaginary;
            return new ComplexDecimal {
                Real = (Real * o.Real + Imaginary * o.Imaginary) / denominator,
                Imaginary = (Imaginary * o.Real - Real * o.Imaginary) / denominator
            };
        }

        public override IConvertible Magnitude() {
            return System.Math.Sqrt((double) (Real * Real + Imaginary * Imaginary));
        }

        protected override long ToInt64() {
            throw new NotSupportedException();
        }

        protected override ulong ToUInt64() {
            throw new NotSupportedException();
        }

        protected override decimal ToDecimal() {
            throw new NotSupportedException();
        }
    }
}
