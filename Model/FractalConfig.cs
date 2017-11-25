using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Model {
    public class FractalConfig {
        public Type Colorer;
        public Type Transformer;
        public Type Fractal;
        public Type Renderer;
        public Type PartitionScalar;
        public int MaxIterations;
        public int ScreenWidth;
        public int ScreenHeight;

        public static T Construct<T>(Type t) {
            return (T) t.GetTypeInfo().GetConstructor(new Type[0]).Invoke(new object[0]);
        }

        private static IEnumerable<byte> SerializeType(Type type) {
            return Encoding.UTF8.GetBytes(type.FullName).Concat(new byte[] { 0 });
        }

        private static IEnumerable<byte> SerializeInt(int data) {
            return BitConverter.GetBytes(data);
        }

        private static Type DeserializeType(ref IEnumerable<byte> stream) {
            byte[] tmp = stream.TakeWhile(b => b != 0).ToArray();
            stream = stream.Skip(tmp.Length + 1);
            return Type.GetType(Encoding.UTF8.GetString(tmp));
        }

        private static int DeserializeInt(ref IEnumerable<byte> stream) {
            byte[] tmp = stream.Take(4).ToArray();
            stream = stream.Skip(4);
            return BitConverter.ToInt32(tmp, 0);
        }

        public byte[] Serialize() {
            return SerializeType(Colorer)
                  .Concat(SerializeType(Transformer))
                  .Concat(SerializeType(Fractal))
                  .Concat(SerializeType(Renderer))
                  .Concat(SerializeType(PartitionScalar))
                  .Concat(SerializeInt(MaxIterations))
                  .Concat(SerializeInt(ScreenWidth))
                  .Concat(SerializeInt(ScreenHeight))
                  .ToArray();
        }

        public FractalConfig(byte[] data, int off = 0) {
            IEnumerable<byte> stream = data.Skip(off);
            Colorer = DeserializeType(ref stream);
            Transformer = DeserializeType(ref stream);
            Fractal = DeserializeType(ref stream);
            Renderer = DeserializeType(ref stream);
            PartitionScalar = DeserializeType(ref stream);
            MaxIterations = DeserializeInt(ref stream);
            ScreenWidth = DeserializeInt(ref stream);
            ScreenHeight = DeserializeInt(ref stream);
        }

        public FractalConfig() {
        }
    }
}
