using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public class ScalarDecimal : ScalarBase<ScalarDecimal> {
        public decimal Value;

        protected override object[] Data => new object[] {
            Value
        };

        public static decimal DeserializeDecimal(ref IEnumerable<byte> data) {
            byte[] buffer = data.Take(16).ToArray();
            data = data.Skip(16);
            return new decimal(new int[] {
                BitConverter.ToInt32(buffer, 0),
                BitConverter.ToInt32(buffer, 4),
                BitConverter.ToInt32(buffer, 8),
                BitConverter.ToInt32(buffer, 12)
            });
        }

        public override NumberBase Deserialize(ref IEnumerable<byte> data) {
            return (ScalarDecimal) DeserializeDecimal(ref data);
        }

        public override NumberBase Invert() {
            return (ScalarDecimal) (1M / Value);
        }

        public override NumberBase InvertSign() {
            return (ScalarDecimal) (-Value);
        }

        public static IEnumerable<byte> SerializeDecimal(decimal val) {
            return decimal.GetBits(val).SelectMany(BitConverter.GetBytes);
        }

        public override IEnumerable<byte> Serialize() {
            return SerializeDecimal(Value);
        }

        protected override ScalarDecimal Add(ScalarDecimal o) {
            return Value + o;
        }

        protected override int CompareTo(ScalarDecimal o) {
            return Value.CompareTo(o.Value);
        }

        protected override ScalarDecimal Divide(ScalarDecimal o) {
            return Value / o;
        }

        protected override ScalarDecimal Modulus(ScalarDecimal o) {
            return Value % o;
        }

        protected override ScalarDecimal Multiply(ScalarDecimal o) {
            return Value * o;
        }

        protected override ScalarDecimal Subtract(ScalarDecimal o) {
            return Value - o;
        }

        protected override decimal ToDecimal() {
            return Value;
        }

        protected override double ToDouble() {
            return (double) Value;
        }

        protected override long ToInt64() {
            return (long) Value;
        }

        protected override ulong ToUInt64() {
            return (ulong) Value;
        }

        public override ScalarBase Load(IConvertible val) {
            return new ScalarDecimal {
                Value = val.ToDecimal(null)
            };
        }

        public static implicit operator ScalarDecimal(decimal val) {
            return new ScalarDecimal {
                Value = val
            };
        }

        public static implicit operator decimal(ScalarDecimal val) {
            return val.Value;
        }
    }
}
