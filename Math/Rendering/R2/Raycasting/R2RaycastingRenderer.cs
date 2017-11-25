using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Discovery;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering.R2.Raycasting {
    [Renderer("Raycasting R2", CoordinateSystems.R2)]
    public class R2RaycastingRenderer : IRenderer {
        const int PartitioningFactor = 16;

        public IInputHandler InputHandler {
            get;
            private set;
        }
        FrameBuffer FrameBuffer;
        ScalarBase ScreenWidth;
        ScalarBase ScreenHeight;

        public IEnumerable<byte> CalculatePartition(Partition partition, IFractal fractal, IColorer colorer, ICoordinateTransformer transformer, int maxIterations) {
            return partition.Select(transformer.Convert).Select(point => {
                NumberBase escapePoint;
                int iterations;
                bool escaped = fractal.TestPoint(point, maxIterations, out escapePoint, out iterations);
                return colorer.DetermineColor(escaped, point, escapePoint, iterations);
            }).SelectMany(BitConverter.GetBytes);
        }

        public IEnumerable<Partition> CreatePartitions(int screenWidth, int screenHeight, ScalarBase type) {
            if (FrameBuffer == null) {
                FrameBuffer = new FrameBuffer(screenWidth, screenHeight);
            }
            if (InputHandler == null) {
                InputHandler = new R2InputHandler(FrameBuffer, type);
            }
            R2InputHandler inputHandler = (R2InputHandler) InputHandler;
            Partition[] partitions = new Partition[PartitioningFactor * PartitioningFactor];
            ScalarBase partitioningFactor = type.Load(PartitioningFactor);
            ScalarBase one = type.Load(1);
            ScalarBase width = inputHandler.Right - inputHandler.Left;
            ScalarBase height = inputHandler.Bottom - inputHandler.Top;
            ScalarBase pdx = width / partitioningFactor;
            ScalarBase pdy = height / partitioningFactor;
            ScreenWidth = type.Load(screenWidth);
            ScreenHeight = type.Load(screenHeight);
            ScalarBase dx = width / ScreenWidth;
            ScalarBase dy = height / ScreenHeight;
            ScalarBase x = type;
            for (int ix = 0; ix < PartitioningFactor; ++ix) {
                ScalarBase y = type;
                for (int iy = 0; iy < PartitioningFactor; ++iy) {
                    Partition partition = partitions[ix + iy * PartitioningFactor] = new Partition(2);
                    partition.Minimums[0] = inputHandler.Left + pdx * x;
                    partition.Minimums[1] = inputHandler.Top + pdy * y;
                    partition.Maximums[0] = inputHandler.Left + pdx * (x + one);
                    y += one;
                    partition.Maximums[1] = inputHandler.Top + pdy * y;
                    partition.Deltas[0] = dx;
                    partition.Deltas[1] = dy;
                }
                x += one;
            }
            return partitions;
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
                    pixel(x, y, buffer, off);
                    off += 4;
                }
            });
        }

        public void StartEncodingVideo(int fbid) {
            FrameBuffer.StartEncodingVideo(fbid);
        }

        public void StopEncodingVideo(int fbid) {
            FrameBuffer.StopEncodingVideo(fbid);
        }
    }
}
