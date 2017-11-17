using System;
using System.Threading;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Main {
    public interface IProcessType {
        void Configure(ProcessConfig config);

        void Start(CancellationToken token);
    }
}
