using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    public static class ProcessFactory {
        static Dictionary<string, TypeInfo> RegisteredTypes = new Dictionary<string, TypeInfo>();

        static void Register(TypeInfo type, string name) {
            RegisteredTypes.Add(name, type);
        }

        public static void Register<T>(string name) where T : IProcessType {
            Register(typeof(T).GetTypeInfo(), name);
        }

        public static void RegisterAll(Assembly assembly) {
            foreach (TypeInfo type in assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.GetCustomAttribute<FractalProcessAttribute>() != null)) {
                Register(type, type.GetCustomAttribute<FractalProcessAttribute>().Name);
            }
        }

        public static CancellationTokenSource Start(ProcessConfig config, ref bool successful) {
            CancellationTokenSource source = new CancellationTokenSource();
            if (!RegisteredTypes.ContainsKey(config.Type)) {
                Console.Error.WriteLine("Type '{0}' is not registered.", config.Type);
                successful = false;
            } else {
                ConstructorInfo ctor = RegisteredTypes[config.Type].GetConstructor(new Type[0]);
                if (ctor == null) {
                    throw new InvalidOperationException("Cannot construct process");
                }
                IProcessType proc = (IProcessType) ctor.Invoke(new object[0]);
                proc.Configure(config);
                proc.Start(source.Token);
            }
            return source;
        }
    }
}
