using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
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

        void OnGet(object sender, HttpRequestEventArgs e) {
            try {
                if (e.Request.Url.PathAndQuery.EndsWith(".html")) {
                    e.Response.ContentType = "text/html";
                } else if (e.Request.Url.PathAndQuery.EndsWith(".js")) {
                    e.Response.ContentType = "text/javascript";
                } else if (e.Request.Url.PathAndQuery.EndsWith(".css")) {
                    e.Response.ContentType = "text/css";
                } else {
                    return;
                }
                using (Stream stream = typeof(FractalServer).GetTypeInfo().Assembly.GetManifestResourceStream(string.Concat("FractalRenderer.", e.Request.Url.PathAndQuery.Substring(1)))) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        e.Response.WriteContent(Encoding.UTF8.GetBytes(reader.ReadToEnd()));
                    }
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
            }
        }

        public void Configure(ProcessConfig config) {
            Address = IPAddress.Parse(config.Address);
            Port = config.Port;
        }

        public void Start(CancellationToken token) {
            Token = token;
            HttpServer server = new HttpServer(Address, Port);
            server.OnGet += OnGet;
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
