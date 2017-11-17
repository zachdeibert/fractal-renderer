using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class ConnectedClient : WebSocketBehavior {
        readonly FractalServer Server;

        public new void Send(byte[] data) {
            base.Send(data);
        }

        protected override void OnOpen() {

        }

        protected override void OnMessage(MessageEventArgs e) {
            switch (e.Data) {
                case "display":
                    Server.Displays.Add(this);
                    Send(Server.Fractal.ExportKeyFrame());
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
