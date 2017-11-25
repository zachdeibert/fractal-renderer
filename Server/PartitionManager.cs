using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class PartitionManager {
        readonly FractalImpls Impl;
        List<ServerPartition> Partitions;

        public void PartitionRendered(Partition partition) {
            Partitions.RemoveAll(p => p.Partition == partition);
        }

        public Partition Request() {
            ServerPartition part = Partitions.GroupBy(p => p.AssignedClients).OrderBy(g => g.Key).Select(g => g.First()).FirstOrDefault();
            if (part == null) {
                return null;
            } else {
                ++part.AssignedClients;
                return part.Partition;
            }
        }

        public void Repartition() {
            Partitions = Impl.Renderer.CreatePartitions(Impl.ScreenWidth, Impl.ScreenHeight, Impl.PartitionScalar).Select(part => new ServerPartition {
                Partition = part,
                AssignedClients = 0
            }).ToList();
        }

        public PartitionManager(FractalImpls impl) {
            Impl = impl;
            Repartition();
        }
    }
}
