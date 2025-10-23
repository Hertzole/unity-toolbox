using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    [Serializable]
    public struct MinMaxInt : IMinMaxInt, IEquatable<MinMaxInt>
    {
        [SerializeField]
        private int min;
        [SerializeField]
        private int max;

        /// <inheritdoc />
        public int Min
        {
            get { return min; }
            set { min = value; }
        }
        /// <inheritdoc />
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        /// <inheritdoc />
        public bool Equals(MinMaxInt other)
        {
            return min == other.min && max == other.max;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is MinMaxInt other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (min * 397) ^ max;
            }
        }

        public static bool operator ==(MinMaxInt left, MinMaxInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MinMaxInt left, MinMaxInt right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"MinMaxInt (Min: {min}, Max: {max})";
        }
    }
}