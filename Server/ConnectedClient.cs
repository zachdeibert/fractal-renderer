using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    class ConnectedClient : WebSocketBehavior {
        readonly FractalServer Server;

        protected override void OnOpen() {

        }

        protected override void OnMessage(MessageEventArgs e) {
            
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
