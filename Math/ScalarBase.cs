using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public abstract class ScalarBase : NumberBase, IConvertible, IComparable, IComparable<ScalarBase> {
        public override int Dimensions => 1;

        public override ScalarBase Magnitude() {
            if (ToDouble() < 0) {
                return (ScalarBase) InvertSign();
            } else {
                return this;
            }
        }

        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }

        protected abstract long ToInt64();

        protected abstract ulong ToUInt64();

        protected abstract double ToDouble();

        protected abstract decimal ToDecimal();

        public bool ToBoolean(IFormatProvider provider) {
            return ToInt64() != 0;
        }

        public byte ToByte(IFormatProvider provider) {
            return (byte) ToUInt64();
        }

        public char ToChar(IFormatProvider provider) {
            return (char) ToUInt64();
        }

        public DateTime ToDateTime(IFormatProvider provider) {
            return new DateTime(ToInt64());
        }

        public decimal ToDecimal(IFormatProvider provider) {
            return ToDecimal();
        }

        public double ToDouble(IFormatProvider provider) {
            return ToDouble();
        }

        public short ToInt16(IFormatProvider provider) {
            return (short) ToInt64();
        }

        public int ToInt32(IFormatProvider provider) {
            return (int) ToInt64();
        }

        public long ToInt64(IFormatProvider provider) {
            return ToInt64();
        }

        public sbyte ToSByte(IFormatProvider provider) {
            return (sbyte) ToInt64();
        }

        public float ToSingle(IFormatProvider provider) {
            return (float) ToDouble();
        }

        public string ToString(IFormatProvider provider) {
            return ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider) {
             if (conversionType == typeof(bool)) { 
                return ToBoolean(provider); 
            } else if (conversionType == typeof(byte)) { 
                return ToByte(provider); 
            } else if (conversionType == typeof(char)) { 
                return ToChar(provider); 
            } else if (conversionType == typeof(DateTime)) { 
                return ToDateTime(provider); 
            } else if (conversionType == typeof(decimal)) { 
                return ToDecimal(provider); 
            } else if (conversionType == typeof(double)) { 
                return ToDouble(provider); 
            } else if (conversionType == typeof(short)) { 
                return ToInt16(provider); 
            } else if (conversionType == typeof(int)) { 
                return ToInt32(provider); 
            } else if (conversionType == typeof(long)) { 
                return ToInt64(provider); 
            } else if (conversionType == typeof(sbyte)) { 
                return ToSByte(provider); 
            } else if (conversionType == typeof(float)) { 
                return ToSingle(provider); 
            } else if (conversionType == typeof(string)) { 
                return ToString(provider); 
            } else if (conversionType == typeof(ushort)) { 
                return ToUInt16(provider); 
            } else if (conversionType == typeof(uint)) { 
                return ToUInt32(provider); 
            } else if (conversionType == typeof(ulong)) { 
                return ToUInt64(provider); 
            } else {
                throw new ArgumentException("Invalid conversion type");
            }
        }

        public ushort ToUInt16(IFormatProvider provider) {
            return (ushort) ToUInt64();
        }

        public uint ToUInt32(IFormatProvider provider) {
            return (uint) ToUInt64();
        }

        public ulong ToUInt64(IFormatProvider provider) {
            return ToUInt64();
        }

        public abstract int CompareTo(ScalarBase o);

        public int CompareTo(object o) {
            if (o is ScalarBase) {
                return CompareTo((ScalarBase) o);
            } else {
                throw new ArgumentException("Wrong number type");
            }
        }

        public override bool Equals(NumberBase o) {
            return CompareTo(o) == 0;
        }

        public abstract ScalarBase Load(IConvertible val);

        public static ScalarBase operator +(ScalarBase a, ScalarBase b) {
            return (ScalarBase) a.Add(b);
        }

        public static ScalarBase operator -(ScalarBase a, ScalarBase b) {
            return (ScalarBase) a.Subtract(b);
        }

        public static ScalarBase operator *(ScalarBase a, ScalarBase b) {
            return (ScalarBase) a.Multiply(b);
        }

        public static ScalarBase operator /(ScalarBase a, ScalarBase b) {
            return (ScalarBase) a.Divide(b);
        }

        public static ScalarBase operator %(ScalarBase a, ScalarBase b) {
            return (ScalarBase) a.Modulus(b);
        }

        public static bool operator <(ScalarBase a, ScalarBase b) {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(ScalarBase a, ScalarBase b) {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <=(ScalarBase a, ScalarBase b) {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(ScalarBase a, ScalarBase b) {
            return a.CompareTo(b) >= 0;
        }
    }

    public abstract class ScalarBase<T> : ScalarBase where T : ScalarBase<T> {
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

        protected abstract int CompareTo(T o);

        public override int CompareTo(ScalarBase o) {
            if (GetType() != o.GetType()) {
                throw new ArgumentException("Wrong number type");
            }
            return CompareTo((T) o);
        }
    }
}
