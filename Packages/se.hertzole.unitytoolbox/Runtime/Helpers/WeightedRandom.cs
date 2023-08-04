using System;
using System.Collections.Generic;

namespace Hertzole.UnityToolbox
{
	public static class WeightedRandom
	{
		public static T GetRandom<T>(IReadOnlyList<T> list) where T : IWeighted
		{
			int totalWeight = 0;
			for (int i = 0; i < list.Count; i++)
			{
				totalWeight += list[i].RandomWeight;
			}

			int random = UnityEngine.Random.Range(0, totalWeight);
			for (int i = 0; i < list.Count; i++)
			{
				if (random < list[i].RandomWeight)
				{
					return list[i];
				}

				random -= list[i].RandomWeight;
			}

			return list[list.Count - 1];
		}

		public static T GetRandom<T>(IReadOnlyList<T> list, Random random) where T : IWeighted
		{
			int totalWeight = 0;
			for (int i = 0; i < list.Count; i++)
			{
				totalWeight += list[i].RandomWeight;
			}

			int randomNumber = random.Next(0, totalWeight);
			for (int i = 0; i < list.Count; i++)
			{
				if (randomNumber < list[i].RandomWeight)
				{
					return list[i];
				}

				randomNumber -= list[i].RandomWeight;
			}

			return list[list.Count - 1];
		}

		public static T GetRandom<T>(IReadOnlyList<T> list, ref Unity.Mathematics.Random random) where T : IWeighted
		{
			int totalWeight = 0;
			for (int i = 0; i < list.Count; i++)
			{
				totalWeight += list[i].RandomWeight;
			}

			int randomNumber = random.NextInt(0, totalWeight);
			for (int i = 0; i < list.Count; i++)
			{
				if (randomNumber < list[i].RandomWeight)
				{
					return list[i];
				}

				randomNumber -= list[i].RandomWeight;
			}

			return list[list.Count - 1];
		}
	}
}