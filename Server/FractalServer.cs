using System;
using System.Threading;
using System.Threading.Tasks;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    [FractalProcess("server")]
    public class FractalServer : IProcessType {
        public void Configure(ProcessConfig config) {
            
        }

        public void Start(CancellationToken token) {
            Task.Delay(10000, token).ContinueWith(t => {
                Console.WriteLine("Hello, world!");
            }, token);
        }
    }
}
