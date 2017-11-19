using System;
using System.Linq;
using Com.GitHub.ZachDeibert.FractalRenderer.Model;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Server {
    public class DisplayPartitioner {
        const int PartitioningFactor = 20;
        readonly PartitionInfo[] Partitions;

        public Rectangle RequestPartition() {
            PartitionInfo partition = Partitions.Where(p => !p.Completed).OrderBy(p => p.AssignedClients).FirstOrDefault();
            if (partition == null) {
                return null;
            } else {
                ++partition.AssignedClients;
                return partition.Area;
            }
        }

        public void FinishPartition(Rectangle partition) {
            Partitions.First(p => p.Area.Equals(partition)).Completed = true;
        }

        public DisplayPartitioner() {
            Partitions = new PartitionInfo[PartitioningFactor * PartitioningFactor];
            for (int x = 0; x < PartitioningFactor; ++x) {
                for (int y = 0; y < PartitioningFactor; ++y) {
                    Partitions[x + y * PartitioningFactor] = new PartitionInfo {
                        Area = new Rectangle {
                            Left = x * RenderedFractal.Width / PartitioningFactor,
                            Top = y * RenderedFractal.Height / PartitioningFactor,
                            Right = (x + 1) * RenderedFractal.Width / PartitioningFactor - 1,
                            Bottom = (y + 1) * RenderedFractal.Height / PartitioningFactor - 1
                        },
                        Completed = false,
                        AssignedClients = 0
                    };
                }
            }
        }
    }
}
