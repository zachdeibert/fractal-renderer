using System;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Model {
    public enum PacketIds : byte {
        SerializedConfig = 1,
        PartitionAssignment,
        RenderedPartition,
        RenderKeyFrame,
        RenderFrame,
        RequestPartition,
        RequestConfig
    }
}
