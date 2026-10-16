#if TOOLBOX_MATHEMATICS
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Pool;
using Random = Unity.Mathematics.Random;

namespace Hertzole.UnityToolbox
{
    partial class WeightedRandom
    {
        private static readonly Func<Random, int, (int, Random)> generateMathRandom = (random, maxWeight) =>
        {
            int result = random.NextInt(0, maxWeight);
            return (result, random);
        };

        public static int GetRandomIndex<T>(IReadOnlyList<T> list, Func<T, int> getWeight, ref Random random)
        {
            int result = GetRandomIndexInternal(list, getWeight, random, out Random newRandom, generateMathRandom);
            random = newRandom;
            return result;
        }

        public static int GetRandomIndex<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight, ref Random random) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight, ref random);
            }
        }

        public static int GetRandomIndex<T>(IEnumerable<T> enumerable, Func<T, int> getWeight, ref Random random)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight, ref random);
            }
        }

        public static T GetRandom<T>(IReadOnlyList<T> list, Func<T, int> getWeight, ref Random random)
        {
            return list[GetRandomIndex(list, getWeight, ref random)];
        }

        public static T GetRandom<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight, ref Random random) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight, ref random)];
            }
        }

        public static T GetRandom<T>(IEnumerable<T> enumerable, Func<T, int> getWeight, ref Random random)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight, ref random)];
            }
        }

        #region Obsolete
        [Obsolete("Use the overload with the getWeight parameter instead.")]
        [ExcludeFromCodeCoverage]
        public static T GetRandom<T>(IReadOnlyList<T> list, ref Random random) where T : IWeighted
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
        #endregion
    }
}
#endif