using System;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using Com.GitHub.ZachDeibert.FractalRenderer.Main;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Colors;
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
        RenderProxy Renderer;

        byte[] RenderPartition(Rectangle partition) {
            byte[] data = new byte[partition.Width * partition.Height * 4];
            for (int dx = 0; dx < partition.Width; ++dx) {
                for (int dy = 0; dy < partition.Height; ++dy) {
                    int x = dx + partition.Left;
                    int y = dy + partition.Top;
                    FractalColor color = Renderer.RenderPixel(x, y, RenderedFractal.Width, RenderedFractal.Height);
                    int i = (dx + dy * partition.Width) * 4;
                    data[i] = (byte) color.R;
                    data[i + 1] = (byte) color.G;
                    data[i + 2] = (byte) color.B;
                    data[i + 3] = 255;
                }
            }
            return data;
        }

        void OnOpen(object sender, EventArgs e) {
            Socket.Send("config");
        }

        void OnMessage(object sender, MessageEventArgs e) {
            try {
                if (e.IsBinary) {
                    Rectangle partition;
                    switch ((PacketIds) e.RawData[0]) {
                        case PacketIds.SerializedConfig:
                            Renderer = new FractalConfig(e.RawData, 1).CreateRenderer();
                            Socket.Send("req");
                            break;
                        case PacketIds.PartitionAssignment:
                            partition = new Rectangle(e.RawData, 1);
                            Task.Run(() => RenderPartition(partition), Token).ContinueWith(t => {
                                if (t.IsFaulted) {
                                    Console.Error.WriteLine(t.Exception);
                                } else if (t.IsCompleted) {
                                    byte[] buffer = new byte[17 + t.Result.Length];
                                    buffer[0] = (byte) PacketIds.RenderedPartition;
                                    partition.Serialize().CopyTo(buffer, 1);
                                    t.Result.CopyTo(buffer, 17);
                                    Socket.Send(buffer);
                                    Socket.Send("req");
                                }
                            });
                            break;
                    }
                }
            } catch (Exception ex) {
                Console.Error.WriteLine(ex);
                throw;
            }
        }

        void OnError(object sender, ErrorEventArgs e) {
            Console.Error.WriteLine(e.Exception);
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
