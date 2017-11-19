using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Browser {
    [FractalProcess("display")]
    public class DisplayProcess : IProcessType {
        string Address;
        int Port;

        public void Configure(ProcessConfig config) {
            Address = config.Address;
            Port = config.Port;
        }

        public void Start(CancellationToken token) {
            string url = string.Format("http://{0}:{1}/display.html", Address, Port);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Process.Start(new ProcessStartInfo("cmd", string.Concat("/c start ", url)));
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Process.Start("xdg-open", url);
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                Process.Start("open", url);
            }
        }
    }
}
