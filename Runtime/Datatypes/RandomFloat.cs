using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hertzole.UnityToolbox
{
	[Serializable]
    [Obsolete("Use MinMaxFloat with MinMaxExtensions.GetRandomValue() instead.")]
	public struct RandomFloat : IMinMaxFloat
	{
		[SerializeField]
		private float min;
		[SerializeField]
		private float max;

		public float Value
		{
			get { return GetRandom(); }
		}

		public float Min
		{
			get { return min; }
			set { min = value; }
		}
		public float Max
		{
			get { return max; }
			set { max = value; }
		}

		/// <inheritdoc />
		public void Deconstruct(out float minValue, out float maxValue)
		{
			throw new NotImplementedException();
		}

		public RandomFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		private float GetRandom()
		{
			return Random.Range(min, max);
		}

		public static implicit operator float(RandomFloat random)
		{
			return random.GetRandom();
		}

		public override string ToString()
		{
			return $"{nameof(min)}: {min}, {nameof(max)}: {max}";
		}
	}
}