using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    [FractalProcess("server")]
    public class FractalServer : IProcessType {
        IPAddress Address;
        int Port;
        CancellationToken Token;
        public readonly HashSet<int> VideoIds;
        public readonly FractalImpls Impl;
        public event Action PartitionsAdded;

        public void OnPartitionsAdded() {
            PartitionsAdded?.Invoke();
        }

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
            VideoIds = new HashSet<int>();
            Impl = new FractalImpls();
        }
    }
}
