using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    class ConnectedClient : WebSocketBehavior {
        readonly FractalServer Server;

        protected override void OnOpen() {

        }

        protected override void OnMessage(MessageEventArgs e) {
            switch (e.Data) {
                case "display":
                    Send(Server.Fractal.Export(0, 0, RenderedFractal.Width, RenderedFractal.Height));
                    break;
            }
        }

        protected override void OnError(ErrorEventArgs e) {

        }

        protected override void OnClose(CloseEventArgs e) {

        }

        public ConnectedClient(FractalServer server) {
            Server = server;
        }
    }
}
