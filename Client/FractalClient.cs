using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;
using Com.GitHub.ZachDeibert.FractalRenderer.Math;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;
using Com.GitHub.ZachDeibert.FractalRenderer.Model;
using Com.GitHub.ZachDeibert.FractalRenderer.Server;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Client {
    [FractalProcess("renderer")]
    public class FractalClient : IProcessType, IDisposable {
        string Address;
        int Port;
        CancellationToken Token;
        WebSocket Socket;
        IRenderer Renderer;
        IFractal Fractal;
        IColorer Colorer;
        ICoordinateTransformer Transformer;
        ScalarBase PartitionScalar;
        int MaxIterations;

        void OnOpen(object sender, EventArgs e) {
            Socket.Send(new byte[] {
                (byte) PacketIds.RequestConfig
            });
        }

        void OnMessage(object sender, MessageEventArgs e) {
            try {
                if (e.IsBinary) {
                    FractalConfig config;
                    Partition partition;
                    IEnumerable<byte> data = e.RawData.Skip(1);
                    switch ((PacketIds) e.RawData[0]) {
                        case PacketIds.SerializedConfig:
                            config = new FractalConfig(e.RawData, 1);
                            Renderer = FractalConfig.Construct<IRenderer>(config.Renderer);
                            Fractal = FractalConfig.Construct<IFractal>(config.Fractal);
                            Colorer = FractalConfig.Construct<IColorer>(config.Colorer);
                            Transformer = FractalConfig.Construct<ICoordinateTransformer>(config.Transformer);
                            PartitionScalar = FractalConfig.Construct<ScalarBase>(config.PartitionScalar);
                            MaxIterations = config.MaxIterations;
                            Socket.Send(new byte[] {
                                (byte) PacketIds.RequestPartition
                            });
                            break;
                        case PacketIds.PartitionAssignment:
                            if (e.RawData.Length > 1) {
                                partition = new Partition(PartitionScalar, ref data);
                                Task.Run(() => Renderer.CalculatePartition(partition, Fractal, Colorer, Transformer, MaxIterations).ToArray(), Token).ContinueWith(t => {
                                    if (t.IsFaulted) {
                                        Console.Error.WriteLine(t.Exception);
                                    } else if (t.IsCompleted) {
                                        byte[] part = partition.Serialize().ToArray();
                                        byte[] buffer = new byte[1 + part.Length + t.Result.Length];
                                        buffer[0] = (byte) PacketIds.RenderedPartition;
                                        part.CopyTo(buffer, 1);
                                        t.Result.CopyTo(buffer, 1 + part.Length);
                                        Socket.Send(buffer);
                                        Socket.Send(new byte[] {
                                            (byte) PacketIds.RequestPartition
                                        });
                                    }
                                });
                            }
                            break;
                    }
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        void OnError(object sender, ErrorEventArgs e) {
            if (e.Exception == null) {
                if (e.Message != null) {
                    Console.Error.WriteLine(e.Message);
                }
            } else {
                Console.Error.WriteLine(e.Exception);
            }
        }

        void OnClose(object sender, CloseEventArgs e) {
            
        }

        public void Configure(ProcessConfig config) {
            Address = config.Address;
            Port = config.Port;
        }

        public void Start(CancellationToken token) {
            Token = token;
            Socket = new WebSocket(string.Format("ws://{0}:{1}", Address, Port));
            Socket.OnOpen += OnOpen;
            Socket.OnMessage += OnMessage;
            Socket.OnError += OnError;
            Socket.OnClose += OnClose;
            Token.Register(Dispose);
            Socket.Connect();
        }

        public void Dispose() {
            Socket.Close();
        }
    }
}
