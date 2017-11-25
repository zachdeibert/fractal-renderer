using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public class ScalarInt : ScalarBase<ScalarInt> {
        public int Value;

        protected override object[] Data => new object[] {
            Value
        };

        public override NumberBase Deserialize(ref IEnumerable<byte> data) {
            byte[] buffer = data.Take(4).ToArray();
            data = data.Skip(4);
            return (ScalarInt) BitConverter.ToInt32(buffer, 0);
        }

        public override NumberBase Invert() {
            if (Value == 1 || Value == -1) {
                return (ScalarInt) Value;
            } else if (Value == 0) {
                return (ScalarInt) int.MaxValue;
            } else {
                return (ScalarInt) 0;
            }
        }

        public override NumberBase InvertSign() {
            return (ScalarInt) (-Value);
        }

        public override IEnumerable<byte> Serialize() {
            return BitConverter.GetBytes(Value);
        }

        protected override ScalarInt Add(ScalarInt o) {
            return Value + o;
        }

        protected override int CompareTo(ScalarInt o) {
            return Value.CompareTo(o.Value);
        }

        protected override ScalarInt Divide(ScalarInt o) {
            if (o.Value == 0) {
                return int.MaxValue;
            }
            return Value / o;
        }

        protected override ScalarInt Modulus(ScalarInt o) {
            if (o.Value == 0) {
                return 0;
            }
            return Value % o;
        }

        protected override ScalarInt Multiply(ScalarInt o) {
            return Value * o;
        }

        protected override ScalarInt Subtract(ScalarInt o) {
            return Value - o;
        }

        protected override decimal ToDecimal() {
            return Value;
        }

        protected override double ToDouble() {
            return Value;
        }

        protected override long ToInt64() {
            return Value;
        }

        protected override ulong ToUInt64() {
            return (ulong) Value;
        }

        public override ScalarBase Load(IConvertible val) {
            return new ScalarInt {
                Value = val.ToInt32(null)
            };
        }

        public static implicit operator ScalarInt(int val) {
            return new ScalarInt {
                Value = val
            };
        }

        public static implicit operator int(ScalarInt val) {
            return val.Value;
        }
    }
}
