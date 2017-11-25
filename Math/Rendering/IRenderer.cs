using System;
using System.Collections.Generic;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coloring;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Coordinates;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Fractals;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public interface IRenderer {
        IInputHandler InputHandler {
            get;
        }

        IEnumerable<Partition> CreatePartitions(int screenWidth, int screenHeight, ScalarBase type);

        IEnumerable<byte> CalculatePartition(Partition partition, IFractal fractal, IColorer colorer, ICoordinateTransformer transformer, int maxIterations);

        void MergePartition(Partition partition, IEnumerable<byte> data);

        void StartEncodingVideo(int fbid);

        void StopEncodingVideo(int fbid);

        DamagedArea EncodeKeyFrame(int fbid);

        DamagedArea EncodeFrame(int fbid);
    }
}
