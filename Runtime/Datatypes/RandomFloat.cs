using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	public struct RandomFloat
	{
		[SerializeField] 
		private float min;
		[SerializeField] 
		private float max;

		public float Min { get { return min; } set { min = value; } }
		public float Max { get { return max; } set { max = value; } }
		
		public float Value { get { return GetRandom(); } }
		
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
	}
}