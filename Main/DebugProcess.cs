using System;
using System.Threading;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    [FractalProcess("debug-brk")]
    public class DebugProcess : IProcessType {
        public void Configure(ProcessConfig config) {
            Console.WriteLine("Waiting for debugger to attach.");
            Console.WriteLine("Please set a breakpoint at DebugProcess.cs:12 and set go = true");
            bool go = false;
            while (!go) {
                Thread.Sleep(100);
            }
        }

        public void Start(CancellationToken token) {
        }
    }
}
