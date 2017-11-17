using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    [FractalProcess("server")]
    public class FractalServer : IProcessType {
        const int MaxFPS = 30;
        IPAddress Address;
        int Port;
        CancellationToken Token;
        public readonly RenderedFractal Fractal;
        public readonly HashSet<ConnectedClient> Displays;
        int TestColor;

        void DisplayUpdateLoop(Task t) {
            if (!Token.IsCancellationRequested) {
                if (Fractal.DirtyRegion == null) {
                    Task.Delay(500 / MaxFPS, Token).ContinueWith(DisplayUpdateLoop, Token);
                } else {
                    if (Displays.Count > 0) {
                        byte[] data = Fractal.CleanRegion();
                        foreach (ConnectedClient client in Displays.ToArray()) {
                            client.Send(data);
                        }
                    } else {
                        Fractal.DirtyRegion = null;
                    }
                    Task.Delay(1000 / MaxFPS, Token).ContinueWith(DisplayUpdateLoop, Token);
                }
            }
        }

        void DrawTest(Task t) {
            if (!Token.IsCancellationRequested) {
                TestColor += 16;
                TestColor %= 256;
                Fractal.Clear(new FractalColor { R = TestColor, G = 0, B = 255 });
                Task.Delay(100, Token).ContinueWith(DrawTest, Token);
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
            Task.Delay(1, Token).ContinueWith(DisplayUpdateLoop, Token);
            Task.Delay(100, Token).ContinueWith(DrawTest, Token);
        }

        public FractalServer() {
            Fractal = new RenderedFractal();
            Fractal.Clear(new FractalColor { R = 0, G = 0, B = 0 });
            Displays = new HashSet<ConnectedClient>();
        }
    }
}
