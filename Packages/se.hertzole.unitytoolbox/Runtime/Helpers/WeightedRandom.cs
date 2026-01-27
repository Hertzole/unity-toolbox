#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Pool;

namespace Hertzole.UnityToolbox
{
    public static partial class WeightedRandom
    {
        private static readonly Func<object?, int, (int, object?)> generateUnityRandom = (_, maxWeight) =>
        {
            int result = UnityEngine.Random.Range(0, maxWeight);
            return (result, null);
        };

        private static readonly Func<Random, int, (int, Random)> generateSystemRandom = (random, maxWeight) =>
        {
            int result = random.Next(0, maxWeight);
            return (result, random);
        };

        public static int GetRandomIndex<T>(IReadOnlyList<T> list, Func<T, int> getWeight)
        {
            return GetRandomIndexInternal(list, getWeight, null, out _, generateUnityRandom);
        }

        public static int GetRandomIndex<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight);
            }
        }

        public static int GetRandomIndex<T>(IEnumerable<T> enumerable, Func<T, int> getWeight)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight);
            }
        }

        public static int GetRandomIndex<T>(IReadOnlyList<T> list, Func<T, int> getWeight, Random random)
        {
            return GetRandomIndexInternal(list, getWeight, random, out _, generateSystemRandom);
        }

        public static int GetRandomIndex<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight, Random random) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight, random);
            }
        }

        public static int GetRandomIndex<T>(IEnumerable<T> enumerable, Func<T, int> getWeight, Random random)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return GetRandomIndex<T>(list, getWeight, random);
            }
        }

        public static T GetRandom<T>(IReadOnlyList<T> list, Func<T, int> getWeight)
        {
            return list[GetRandomIndex(list, getWeight)];
        }

        public static T GetRandom<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight)];
            }
        }

        public static T GetRandom<T>(IEnumerable<T> enumerable, Func<T, int> getWeight)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight)];
            }
        }

        public static T GetRandom<T>(IReadOnlyList<T> list, Func<T, int> getWeight, Random random)
        {
            return list[GetRandomIndex(list, getWeight, random)];
        }

        public static T GetRandom<T, TEnumerable>(TEnumerable enumerable, Func<T, int> getWeight, Random random) where TEnumerable : IEnumerable<T>
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight, random)];
            }
        }

        public static T GetRandom<T>(IEnumerable<T> enumerable, Func<T, int> getWeight, Random random)
        {
            using (ListPool<T>.Get(out List<T> list))
            {
                foreach (T value in enumerable)
                {
                    list.Add(value);
                }

                return list[GetRandomIndex<T>(list, getWeight, random)];
            }
        }

        private static int GetRandomIndexInternal<T, TRandom>(IReadOnlyList<T> list,
            Func<T, int> getWeight,
            TRandom random,
            out TRandom newRandom,
            Func<TRandom, int, (int, TRandom)> generateRandom)
        {
            int totalWeight = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeight += getWeight(list[i]);
            }

            (int, TRandom) randomResult = generateRandom(random, totalWeight);
            int randomNumber = randomResult.Item1;
            newRandom = randomResult.Item2;

            for (int i = 0; i < list.Count; i++)
            {
                if (randomNumber < getWeight(list[i]))
                {
                    return i;
                }

                randomNumber -= getWeight(list[i]);
            }

            return list.Count - 1;
        }

        #region Obsolete
        [Obsolete("Use the overload with the getWeight parameter instead.")]
        [ExcludeFromCodeCoverage]
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
        [ExcludeFromCodeCoverage]
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
        #endregion
    }
}