using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hertzole.UnityToolbox
{
    [Serializable]
    [Obsolete("Use MinMaxInt with MinMaxExtensions.GetRandomValue() instead.")]
    public struct RandomInt : IMinMaxInt
    {
        [SerializeField]
        private int min;
        [SerializeField]
        private int max;

        public int Value
        {
            get { return GetRandom(); }
        }

        public int Min
        {
            get { return min; }
            set { min = value; }
        }
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <inheritdoc />
        public void Deconstruct(out int minValue, out int maxValue)
        {
	        throw new NotImplementedException();
        }

        public RandomInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        private int GetRandom()
        {
            return Random.Range(min, max);
        }

        public static implicit operator int(RandomInt random)
        {
            return random.GetRandom();
        }
    }
}