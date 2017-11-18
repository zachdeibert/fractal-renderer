using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math {
    public abstract class NumberBase : IConvertible {
        public abstract int Dimensions {
            get;
        }

        protected abstract NumberBase Add(NumberBase o);

        protected abstract NumberBase Subtract(NumberBase o);

        protected abstract NumberBase Multiply(NumberBase o);

        protected abstract NumberBase Divide(NumberBase o);

        public abstract IConvertible Magnitude();

        protected abstract long ToInt64();

        protected abstract ulong ToUInt64();

        protected abstract decimal ToDecimal();

        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }

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
            throw new NotSupportedException();
        }

        public decimal ToDecimal(IFormatProvider provider) {
            return ToDecimal();
        }

        public double ToDouble(IFormatProvider provider) {
            return (double) ToDecimal();
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
            return (float) ToDecimal();
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
                throw new NotSupportedException();
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
    }

    public abstract class NumberBase<T> : NumberBase where T : NumberBase<T> {
        protected abstract T Add(T o);

        protected abstract T Subtract(T o);

        protected abstract T Multiply(T o);

        protected abstract T Divide(T o);

        protected override NumberBase Add(NumberBase o) {
            if (o.GetType() != GetType()) {
                throw new ArgumentException("Invalid number type");
            }
            return Add((T) o);
        }

        protected override NumberBase Subtract(NumberBase o) {
            if (o.GetType() != GetType()) {
                throw new ArgumentException("Invalid number type");
            }
            return Subtract((T) o);
        }

        protected override NumberBase Multiply(NumberBase o) {
            if (o.GetType() != GetType()) {
                throw new ArgumentException("Invalid number type");
            }
            return Multiply((T) o);
        }

        protected override NumberBase Divide(NumberBase o) {
            if (o.GetType() != GetType()) {
                throw new ArgumentException("Invalid number type");
            }
            return Divide((T) o);
        }
    }
}
