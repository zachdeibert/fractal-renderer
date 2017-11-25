using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    class PartitionEnumerator : IEnumerator<ScalarBase[]> {
        readonly Partition Partition;
        ScalarBase[] Point;

        public ScalarBase[] Current {
            get {
                ScalarBase[] clone = new ScalarBase[Point.Length];
                Point.CopyTo(clone, 0);
                return clone;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose() {
        }

        public bool MoveNext() {
            for (int i = Point.Length - 1; i >= 0; --i) {
                Point[i] += Partition.Deltas[i];
                if (Point[i] > Partition.Maximums[i]) {
                    for (int j = i; j < Point.Length; ++j) {
                        Point[j] = Partition.Minimums[j];
                    }
                } else {
                    return true;
                }
            }
            return false;
        }

        public void Reset() {
            Point = new ScalarBase[Partition.Minimums.Length];
            Partition.Minimums.CopyTo(Point, 0);
        }

        public PartitionEnumerator(Partition partition) {
            Partition = partition;
            Reset();
        }
    }
}
