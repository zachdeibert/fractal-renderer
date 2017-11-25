using System;
using System.Collections.Generic;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class PartitionManager {
        readonly FractalImpls Impl;
        List<ServerPartition> Partitions;
        object PartitionLock;
        readonly HashSet<Partition> FinishedPartitions;

        public void PartitionRendered(Partition partition) {
            lock (PartitionLock) {
                Partitions.RemoveAll(p => p.Partition.Equals(partition));
            }
            FinishedPartitions.Add(partition);
        }

        public Partition Request() {
            lock (PartitionLock) {
                ServerPartition part = Partitions.GroupBy(p => p.AssignedClients).OrderBy(g => g.Key).Select(g => g.First()).FirstOrDefault();
                if (part == null) {
                    return null;
                } else {
                    ++part.AssignedClients;
                    return part.Partition;
                }
            }
        }

        public void Repartition() {
            lock (PartitionLock) {
                Partition[] parts = Impl.Renderer.CreatePartitions(Impl.ScreenWidth, Impl.ScreenHeight, Impl.PartitionScalar).ToArray();
                Partitions = parts.Except(FinishedPartitions)
                                .Select(part => Partitions.FirstOrDefault(p => p.Partition == part) ?? new ServerPartition {
                                    Partition = part,
                                    AssignedClients = 0
                                }).ToList();
                HashSet<Partition> partSet = new HashSet<Partition>(parts);
                FinishedPartitions.RemoveWhere(part => !partSet.Contains(part));
            }
        }

        public PartitionManager(FractalImpls impl) {
            Impl = impl;
            Partitions = new List<ServerPartition>();
            PartitionLock = new object();
            FinishedPartitions = new HashSet<Partition>();
            Repartition();
        }
    }
}
