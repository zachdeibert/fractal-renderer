using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public class ComplexDecimal : NumberBase<ComplexDecimal> {
        public decimal Real;
        public decimal Imaginary;

        public override int Dimensions => 2;

        protected override object[] Data => new object[] {
            Real,
            Imaginary
        };

        public override NumberBase Deserialize(ref IEnumerable<byte> data) {
            return new ComplexDecimal {
                Real = ScalarDecimal.DeserializeDecimal(ref data),
                Imaginary = ScalarDecimal.DeserializeDecimal(ref data)
            };
        }

        public override bool Equals(ComplexDecimal o) {
            return Real == o.Real && Imaginary == o.Imaginary;
        }

        public override NumberBase Invert() {
            decimal denominator = Real * Real + Imaginary * Imaginary;
            return new ComplexDecimal {
                Real = Real / denominator,
                Imaginary = -Imaginary / denominator
            };
        }

        public override NumberBase InvertSign() {
            return new ComplexDecimal {
                Real = -Real,
                Imaginary = -Imaginary
            };
        }

        public override ScalarBase Magnitude() {
            return (ScalarDecimal) (decimal) System.Math.Sqrt((double) (Real * Real + Imaginary * Imaginary));
        }

        public override IEnumerable<byte> Serialize() {
            return ScalarDecimal.SerializeDecimal(Real).Concat(ScalarDecimal.SerializeDecimal(Imaginary));
        }

        protected override ComplexDecimal Add(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real + o.Real,
                Imaginary = Imaginary + o.Imaginary
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

        protected override ComplexDecimal Modulus(ComplexDecimal o) {
            throw new NotSupportedException();
        }

        protected override ComplexDecimal Multiply(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real * o.Real - Imaginary * o.Imaginary,
                Imaginary = Real * o.Imaginary + Imaginary * o.Real
            };
        }

        protected override ComplexDecimal Subtract(ComplexDecimal o) {
            return new ComplexDecimal {
                Real = Real - o.Real,
                Imaginary = Imaginary - o.Imaginary
            };
        }
    }
}
