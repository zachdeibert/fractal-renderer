using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;
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
                if (e.IsBinary) {
                    DamagedArea area;
                    Partition part;
                    IEnumerable<byte> data;
                    switch ((PacketIds) e.RawData[0]) {
                        case PacketIds.RenderedPartition:
                            data = e.RawData.Skip(1);
                            part = new Partition(Server.Impl.PartitionScalar, ref data);
                            Server.Impl.Renderer.MergePartition(part, data);
                            Server.Impl.PartitionManager.PartitionRendered(part);
                            break;
                        case PacketIds.RenderKeyFrame:
                            if (FrameBufferId < 0) {
                                lock (Server.VideoIds) {
                                    Random rand = new Random();
                                    do {
                                        FrameBufferId = rand.Next();
                                    } while (Server.VideoIds.Contains(FrameBufferId));
                                    Server.VideoIds.Add(FrameBufferId);
                                    Server.Impl.Renderer.StartEncodingVideo(FrameBufferId);
                                }
                            }
                            area = Server.Impl.Renderer.EncodeKeyFrame(FrameBufferId);
                            Send(new byte[] {
                                (byte) PacketIds.RenderKeyFrame
                            }.Concat(area.Serialize()).ToArray());
                            break;
                        case PacketIds.RenderFrame:
                            area = Server.Impl.Renderer.EncodeFrame(FrameBufferId);
                            Send(new byte[] {
                                (byte) PacketIds.RenderFrame
                            }.Concat(area.Serialize()).ToArray());
                            break;
                        case PacketIds.RequestPartition:
                            part = Server.Impl.PartitionManager.Request();
                            if (part == null) {
                                Send(new byte[] {
                                    (byte) PacketIds.PartitionAssignment
                                });
                            } else {
                                Send(new byte[] {
                                    (byte) PacketIds.PartitionAssignment
                                }.Concat(part.Serialize()).ToArray());
                            }
                            break;
                        case PacketIds.RequestConfig:
                            Send(new byte[] {
                                (byte) PacketIds.SerializedConfig
                            }.Concat(Server.Impl.Config.Serialize()).ToArray());
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
            lock (Server.VideoIds) {
                Server.Impl.Renderer.StopEncodingVideo(FrameBufferId);
                Server.VideoIds.Remove(FrameBufferId);
                FrameBufferId = -1;
            }
        }

        public ConnectedClient(FractalServer server) {
            Server = server;
        }
    }
}
