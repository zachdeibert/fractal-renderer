using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Model;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class ConnectedClient : WebSocketBehavior {
        readonly FractalServer Server;
        int FrameBufferId;

        public new void Send(byte[] data) {
            base.Send(data);
        }

        protected override void OnOpen() {
            FrameBufferId = -1;
        }

        protected override void OnMessage(MessageEventArgs e) {
            try {
                byte[] buffer, data;
                Rectangle partition;
                if (e.IsText) {
                    switch (e.Data) {
                        case "keyframe":
                            Send(Server.Fractal.ExportKeyFrame(ref FrameBufferId));
                            break;
                        case "frame":
                            Send(Server.Fractal.CleanRegion(ref FrameBufferId));
                            break;
                        case "req":
                            partition = Server.Partitioner.RequestPartition();
                            if (partition != null) {
                                data = partition.Serialize();
                                buffer = new byte[data.Length + 1];
                                buffer[0] = (byte) PacketIds.PartitionAssignment;
                                data.CopyTo(buffer, 1);
                                Send(buffer);
                            }
                            break;
                        case "config":
                            data = Server.Fractal.Config.Serialize();
                            buffer = new byte[data.Length + 1];
                            buffer[0] = (byte) PacketIds.SerializedConfig;
                            data.CopyTo(buffer, 1);
                            Send(buffer);
                            break;
                    }
                } else if (e.IsBinary) {
                    switch ((PacketIds) e.RawData[0]) {
                        case PacketIds.RenderedPartition:
                            partition = new Rectangle(e.RawData, 1);
                            Server.Fractal.SetRegion(partition.Left, partition.Top, partition.Width, partition.Height, e.RawData, 17);
                            Server.Partitioner.FinishPartition(partition);
                            break;
                    }
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        protected override void OnError(ErrorEventArgs e) {
            if (e.Exception == null) {
                if (e.Message != null) {
                    Console.Error.WriteLine(e.Message);
                }
            } else {
                Console.Error.WriteLine(e.Exception);
            }
        }

        protected override void OnClose(CloseEventArgs e) {
            Server.Fractal.DisconnectFrameBuffer(ref FrameBufferId);
        }

        public ConnectedClient(FractalServer server) {
            Server = server;
        }
    }
}
