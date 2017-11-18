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
            try {
                switch (e.Data) {
                    case "keyframe":
                        Send(Server.Fractal.ExportKeyFrame());
                        break;
                    case "frame":
                        Send(Server.Fractal.CleanRegion());
                        break;
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        protected override void OnError(ErrorEventArgs e) {
            Console.Error.WriteLine(e.Exception);
        }

        protected override void OnClose(CloseEventArgs e) {
            
        }

        public ConnectedClient(FractalServer server) {
            Server = server;
        }
    }
}
