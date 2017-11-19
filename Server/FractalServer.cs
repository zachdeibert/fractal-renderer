using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;
using Com.GitHub.ZachDeibert.FractalRenderer.Math;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    [FractalProcess("server")]
    public class FractalServer : IProcessType {
        IPAddress Address;
        int Port;
        CancellationToken Token;
        public readonly RenderedFractal Fractal;
        public readonly DisplayPartitioner Partitioner;

        public void Configure(ProcessConfig config) {
            Address = IPAddress.Parse(config.Address);
            Port = config.Port;
        }

        public void Start(CancellationToken token) {
            Token = token;
            WebSocketServer server = new WebSocketServer(Address, Port);
            server.AddWebSocketService<ConnectedClient>("/", () => new ConnectedClient(this));
            Token.Register(server.Stop);
            server.Start();
        }

        public FractalServer() {
            Fractal = new RenderedFractal();
            Fractal.Clear(new FractalColor { R = 0, G = 0, B = 0 });
            Partitioner = new DisplayPartitioner();
        }
    }
}
