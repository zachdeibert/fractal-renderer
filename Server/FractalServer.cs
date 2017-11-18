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

        void DrawTest(Task t) {
            try {
                if (!Token.IsCancellationRequested) {
                    Fractal.Clear(new FractalColor { R = 0, G = 0, B = 0 });
                    IRenderer renderer = new Default2DRenderer();
                    RenderContext ctx = new RenderContext(2, new DecimalImaginaryPlaneTransformer(), new MandelbrotFractal(), new BluePurpleColorer(), 100);
                    for (int x = 0; x < RenderedFractal.Width; ++x) {
                        for (int y = 0; y < RenderedFractal.Height; ++y) {
                            Fractal.SetPixel(x, y, renderer.RenderPixel(x, y, RenderedFractal.Width, RenderedFractal.Height, ctx));
                        }
                    }
                    Task.Delay(10000, Token).ContinueWith(DrawTest, Token);
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

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
            Task.Delay(1000, Token).ContinueWith(DrawTest, Token);
        }

        public FractalServer() {
            Fractal = new RenderedFractal();
            Fractal.Clear(new FractalColor { R = 0, G = 0, B = 0 });
        }
    }
}
