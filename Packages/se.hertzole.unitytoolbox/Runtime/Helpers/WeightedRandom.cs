using System;
using System.Collections.Generic;

namespace Hertzole.UnityToolbox
{
    public static partial class WeightedRandom
    {
        public static T GetRandom<T>(IReadOnlyList<T> list, Func<T, int> getWeight)
        {
            int totalWeight = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += getWeight(list[i]);
            }

            int random = UnityEngine.Random.Range(0, totalWeight);
            for (int i = 0; i < list.Count; i++)
            {
                if (random < getWeight(list[i]))
                {
                    return list[i];
                }

                random -= getWeight(list[i]);
            }

            return list[list.Count - 1];
        }

        public static T GetRandom<T>(IReadOnlyList<T> list, Func<T, int> getWeight, Random random)
        {
            int totalWeight = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += getWeight(list[i]);
            }

            int randomNumber = random.Next(0, totalWeight);
            for (int i = 0; i < list.Count; i++)
            {
                if (randomNumber < getWeight(list[i]))
                {
                    return list[i];
                }

                randomNumber -= getWeight(list[i]);
            }

            return list[list.Count - 1];
        }

        [Obsolete("Use the overload with the getWeight parameter instead.")]
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

        [Obsolete("Use the overload with the getWeight parameter instead.")]
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
    }
}