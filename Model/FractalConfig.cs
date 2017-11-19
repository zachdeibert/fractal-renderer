using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Model {
    public class FractalConfig {
        public Type Renderer     = typeof(Default2DRenderer);
        public int Dimensions    = 2;
        public Type Transformer  = typeof(DecimalImaginaryPlaneTransformer);
        public Type Fractal      = typeof(MandelbrotFractal);
        public Type Colorer      = typeof(BluePurpleColorer);
        public int MaxIterations = 100;

        private static T Construct<T>(Type t) {
            ConstructorInfo ctor = t.GetConstructor(new Type[0]);
            return (T) ctor.Invoke(new object[0]);
        }

        public RenderProxy CreateRenderer() {
            return new RenderProxy(Construct<IRenderer>(Renderer), new RenderContext(Dimensions, Construct<IDomainTransformer>(Transformer), Construct<IFractal>(Fractal), Construct<IColorer>(Colorer), MaxIterations));
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
            return SerializeType(Renderer)
                  .Concat(SerializeInt(Dimensions))
                  .Concat(SerializeType(Transformer))
                  .Concat(SerializeType(Fractal))
                  .Concat(SerializeType(Colorer))
                  .Concat(SerializeInt(MaxIterations))
                  .ToArray();
        }

        public FractalConfig(byte[] data, int off = 0) {
            IEnumerable<byte> stream = data.Skip(off);
            Renderer = DeserializeType(ref stream);
            Dimensions = DeserializeInt(ref stream);
            Transformer = DeserializeType(ref stream);
            Fractal = DeserializeType(ref stream);
            Colorer = DeserializeType(ref stream);
            MaxIterations = DeserializeInt(ref stream);
        }

        public FractalConfig() {
        }
    }
}
