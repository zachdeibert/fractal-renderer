using System;
using System.Collections.Generic;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public abstract class NumberBase : IEquatable<NumberBase> {
        public abstract int Dimensions {
            get;
        }

        protected abstract object[] Data {
            get;
        }

        protected abstract NumberBase Add(NumberBase o);

        protected abstract NumberBase Subtract(NumberBase o);

        protected abstract NumberBase Multiply(NumberBase o);

        protected abstract NumberBase Divide(NumberBase o);

        protected abstract NumberBase Modulus(NumberBase o);

        public abstract NumberBase Divide(NumberBase o, out NumberBase mod);

        public abstract NumberBase Invert();

        public abstract NumberBase InvertSign();

        public abstract ScalarBase Magnitude();

        public abstract IEnumerable<byte> Serialize();

        public abstract NumberBase Deserialize(ref IEnumerable<byte> data);

        public abstract bool Equals(NumberBase o);

        public override bool Equals(object obj) {
            if (obj is NumberBase) {
                return Equals((NumberBase) obj);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return Data.GetHashCode();
        }

        public static NumberBase operator +(NumberBase a, NumberBase b) {
            return a.Add(b);
        }

        public static NumberBase operator -(NumberBase a, NumberBase b) {
            return a.Subtract(b);
        }

        public static NumberBase operator *(NumberBase a, NumberBase b) {
            return a.Multiply(b);
        }

        public static NumberBase operator /(NumberBase a, NumberBase b) {
            return a.Divide(b);
        }

        public static NumberBase operator %(NumberBase a, NumberBase b) {
            return a.Modulus(b);
        }

        public static bool operator ==(NumberBase a, NumberBase b) {
            return a.Equals(b);
        }

        public static bool operator !=(NumberBase a, NumberBase b) {
            return !a.Equals(b);
        }
    }

    public abstract class NumberBase<T> : NumberBase where T : NumberBase<T> {
        protected abstract T Add(T o);

        protected abstract T Subtract(T o);

        protected abstract T Multiply(T o);

        protected abstract T Divide(T o);

        protected abstract T Modulus(T o);

        protected virtual T Divide(T o, out T mod) {
            mod = Modulus(o);
            return Divide(o);
        }

        protected override NumberBase Add(NumberBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return Add((T) o);
        }

        protected override NumberBase Subtract(NumberBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return Subtract((T) o);
        }

        protected override NumberBase Multiply(NumberBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return Multiply((T) o);
        }

        protected override NumberBase Divide(NumberBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return Divide((T) o);
        }

        protected override NumberBase Modulus(NumberBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return Modulus((T) o);
        }

        public override NumberBase Divide(NumberBase o, out NumberBase mod) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            T tmp;
            T res = Divide((T) o, out tmp);
            mod = tmp;
            return res;
        }

        public abstract bool Equals(T o);

        public override bool Equals(NumberBase o) {
            if (GetType() != o.GetType()) {
                return false;
            } else {
                return Equals((T) o);
            }
        }
    }
}
