using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2.Raycasting {
    [Renderer("Raycasting R2", CoordinateSystems.R2)]
    public class R2RaycastingRenderer : IRenderer, IR2Renderer {
        const int PartitioningFactor = 16;

        public IInputHandler InputHandler {
            get;
            private set;
        }
        public FrameBuffer FrameBuffer {
            get;
            private set;
        }
        ScalarBase ScreenWidth;
        ScalarBase ScreenHeight;
        List<Partition> Partitions;

        public IEnumerable<byte> CalculatePartition(Partition partition, IFractal fractal, IColorer colorer, ICoordinateTransformer transformer, int maxIterations) {
            return partition.Select(transformer.Convert).Select(point => {
                NumberBase escapePoint;
                int iterations;
                bool escaped = fractal.TestPoint(point, maxIterations, out escapePoint, out iterations);
                return colorer.DetermineColor(escaped, point, escapePoint, iterations);
            }).SelectMany(BitConverter.GetBytes);
        }

        public void InvalidateArea(int ax, int ay, int awidth, int aheight, ScalarBase type) {
            R2InputHandler inputHandler = (R2InputHandler) InputHandler;
            ScalarBase partitioningFactor = type.Load(PartitioningFactor);
            ScalarBase width = inputHandler.Right - inputHandler.Left;
            ScalarBase height = inputHandler.Bottom - inputHandler.Top;
            ScreenWidth = type.Load(FrameBuffer.Width);
            ScreenHeight = type.Load(FrameBuffer.Height);
            int maxX = ax + awidth;
            int maxY = ay + aheight;
            int idx = FrameBuffer.Width / PartitioningFactor;
            int idy = FrameBuffer.Height / PartitioningFactor;
            ScalarBase x = type.Load(ax) * width / ScreenWidth + inputHandler.Left;
            ScalarBase minY = type.Load(ay) * height / ScreenHeight + inputHandler.Top;
            ScalarBase dx = type.Load(idx) * width / ScreenWidth;
            ScalarBase dy = type.Load(idy) * height / ScreenHeight;
            ScalarBase pdx = width / ScreenWidth;
            ScalarBase pdy = height / ScreenHeight;
            for (int ix = ax; ix < maxX; ix += idx) {
                ScalarBase y = minY;
                for (int iy = ay; iy < maxY; iy += idy) {
                    Partition part = new Partition(2);
                    part.Minimums[0] = x;
                    part.Minimums[1] = y;
                    part.Maximums[0] = x + dx;
                    y += dy;
                    part.Maximums[1] = y;
                    part.Deltas[0] = pdx;
                    part.Deltas[1] = pdy;
                    Partitions.Add(part);
                }
                x += dx;
            }
        }

        public IEnumerable<Partition> CreatePartitions(int screenWidth, int screenHeight, ScalarBase type) {
            if (FrameBuffer == null) {
                FrameBuffer = new FrameBuffer(screenWidth, screenHeight);
            }
            if (InputHandler == null) {
                InputHandler = new R2InputHandler(this, type);
            }
            if (Partitions == null) {
                Partitions = new List<Partition>();
                InvalidateArea(0, 0, screenWidth, screenHeight, type);
            }
            return Partitions;
        }

        public DamagedArea EncodeFrame(int fbid) {
            return FrameBuffer.EncodeFrame(fbid);
        }

        public DamagedArea EncodeKeyFrame(int fbid) {
            return FrameBuffer.EncodeKeyFrame(fbid);
        }

        public void MergePartition(Partition partition, IEnumerable<byte> data) {
            byte[] buffer = data.ToArray();
            R2InputHandler inputHandler = (R2InputHandler) InputHandler;
            ScalarBase width = inputHandler.Right - inputHandler.Left;
            ScalarBase height = inputHandler.Bottom - inputHandler.Top;
            int off = 0;
            FrameBuffer.RawUpdate(pixel => {
                foreach (ScalarBase[] coord in partition) {
                    int x = ((coord[0] - inputHandler.Left) * ScreenWidth / width).ToInt32(null);
                    int y = ((coord[1] - inputHandler.Top) * ScreenHeight / height).ToInt32(null);
                    if (x >= 0 && x < FrameBuffer.Width && y >= 0 && y < FrameBuffer.Height) {
                        pixel(x, y, buffer, off);
                    }
                    off += 4;
                }
            });
            Partitions.Remove(partition);
        }

        public void StartEncodingVideo(int fbid) {
            FrameBuffer.StartEncodingVideo(fbid);
        }

        public void StopEncodingVideo(int fbid) {
            FrameBuffer.StopEncodingVideo(fbid);
        }
    }
}
