using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using Newtonsoft.Json;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    class Program {
        static CancellationTokenSource[] Tokens;

        static void Unload(AssemblyLoadContext ctx) {
            AssemblyLoadContext.Default.Unloading -= Unload;
            foreach (CancellationTokenSource token in Tokens) {
                token.Cancel();
            }
        }

        static void Main(string[] args) {
            if (args.Length != 1) {
                Console.Error.WriteLine("Usage: FractalRenderer.dll <config file>");
                Console.Error.WriteLine("See the 'conf' folder in the source code for configuration examples.");
                Environment.Exit(1);
            }
            if (!File.Exists(args[0])) {
                Console.Error.WriteLine("Config file does not exist.");
                Environment.Exit(1);
            }
            ProcessConfig[] configs = JsonConvert.DeserializeObject<ProcessConfig[]>(File.ReadAllText(args[0]));
            ProcessFactory.RegisterAll(Assembly.GetEntryAssembly());
            bool successful = true;
            Tokens = configs.Select(config => ProcessFactory.Start(config, ref successful)).ToArray();
            AssemblyLoadContext.Default.Unloading += Unload;
            if (!successful) {
                Console.Error.WriteLine("Unable to start fractal renderer.");
                Unload(AssemblyLoadContext.Default);
                Environment.Exit(1);
            }
            Console.WriteLine("Fractal renderer started with {0} process{1}.", configs.Length, configs.Length == 1 ? "" : "es");
            if (Console.IsInputRedirected || Console.IsOutputRedirected || Console.IsErrorRedirected) {
                while (true) {
                    Thread.Sleep(int.MaxValue);
                }
            } else {
                Console.WriteLine("Press any key to quit.");
                Console.ReadKey(true);
            }
        }
    }
}
