using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Com.GitHub.ZachDeibert.FractalRenderer.Math.Rendering {
    public class Partition : IEnumerable<ScalarBase[]>, IEquatable<Partition> {
        public readonly ScalarBase[] Minimums;
        public readonly ScalarBase[] Maximums;
        public readonly ScalarBase[] Deltas;
        public ScalarBase Delta {
            set {
                for (int i = 0; i < Deltas.Length; ++i) {
                    Deltas[i] = value;
                }
            }
        }

        public IEnumerator<ScalarBase[]> GetEnumerator() {
            return new PartitionEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool Equals(Partition other) {
            return Minimums.SequenceEqual(other.Minimums) && Maximums.SequenceEqual(other.Maximums) && Deltas.SequenceEqual(other.Deltas);
        }

        public override bool Equals(object obj) {
            if (obj is Partition) {
                return Equals((Partition) obj);
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return new int[] {
                Minimums.GetHashCode(),
                Maximums.GetHashCode(),
                Deltas.GetHashCode()
            }.GetHashCode();
        }

        public IEnumerable<byte> Serialize() {
            return new ScalarBase[] {
                (ScalarInt) Minimums.Length
            }.Concat(Minimums).Concat(Maximums).Concat(Deltas).SelectMany(o => o.Serialize());
        }

        public Partition(int dimensions) {
            Minimums = new ScalarBase[dimensions];
            Maximums = new ScalarBase[dimensions];
            Deltas = new ScalarBase[dimensions];
        }

        public Partition(ScalarBase template, ref IEnumerable<byte> data) {
            int dimensions = (ScalarInt) new ScalarInt().Deserialize(ref data);
            Minimums = new ScalarBase[dimensions];
            Maximums = new ScalarBase[dimensions];
            Deltas = new ScalarBase[dimensions];
            for (int i = 0; i < dimensions; ++i) {
                Minimums[i] = (ScalarBase) template.Deserialize(ref data);
            }
            for (int i = 0; i < dimensions; ++i) {
                Maximums[i] = (ScalarBase) template.Deserialize(ref data);
            }
            for (int i = 0; i < dimensions; ++i) {
                Deltas[i] = (ScalarBase) template.Deserialize(ref data);
            }
        }
    }
}
