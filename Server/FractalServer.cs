using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    [FractalProcess("server")]
    public class FractalServer : IProcessType {
        IPAddress Address;
        int Port;
        public readonly RenderedFractal Fractal;

        public void Configure(ProcessConfig config) {
            Address = IPAddress.Parse(config.Address);
            Port = config.Port;
        }

        public void Start(CancellationToken token) {
            WebSocketServer server = new WebSocketServer(Address, Port);
            server.AddWebSocketService<ConnectedClient>("/", () => new ConnectedClient(this));
            token.Register(server.Stop);
            server.Start();
        }

        public FractalServer() {
            Fractal = new RenderedFractal();
            Fractal.Clear(new FractalColor { R = 0, G = 0, B = 0 });
        }
    }
}
