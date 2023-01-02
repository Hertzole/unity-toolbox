using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Random = Unity.Mathematics.Random;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Extension methods for collections.
	/// </summary>
	public static class CollectionExtensions
	{
		/// <summary>
		///     Randomly shuffles a list.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle<T>(this IList<T> list)
		{
			Shuffle(list, GetCurrentRandom());
		}

		/// <summary>
		///     Randomly shuffles a list with a given seed.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle<T>(this IList<T> list, int seed)
		{
			Shuffle(list, new Random((uint) seed));
		}

		/// <summary>
		///     Randomly shuffles a list with a given seed.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle<T>(this IList<T> list, uint seed)
		{
			Shuffle(list, new Random(seed));
		}

		/// <summary>
		///     Randomly shuffles a list with a given random struct.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="random">The random struct to use for randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle<T>(this IList<T> list, Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				return;
			}

			for (int i = 0; i < list.Count; i++)
			{
				T temp = list[i];
				int randomIndex = random.NextInt(list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		/// <summary>
		///     Randomly shuffles a list with a provided random object.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="random">The random object to use for randomizing.</param>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentNullException">If the random object is null.</exception>
		public static void Shuffle<T>(this IList<T> list, System.Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfNull(random, nameof(random));

			if (list.Count == 0)
			{
				return;
			}

			for (int i = 0; i < list.Count; i++)
			{
				T temp = list[i];
				int randomIndex = random.Next(list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static T GetRandom<T>(this IList<T> list)
		{
			return GetRandom(list, GetCurrentRandom());
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static T GetRandom<T>(this IList<T> list, int seed)
		{
			return GetRandom(list, new Random((uint) seed));
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static T GetRandom<T>(this IList<T> list, uint seed)
		{
			return GetRandom(list, new Random(seed));
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="random">The random struct to use for randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentException">If the list is empty.</exception>
		public static T GetRandom<T>(this IList<T> list, Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfEmpty(list, nameof(list));

			return list[random.NextInt(list.Count)];
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="random">The random object to use for randomizing.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentNullException">If the random object is null.</exception>
		/// <exception cref="ArgumentException">If the list is empty.</exception>
		public static T GetRandom<T>(this IList<T> list, System.Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfNull(random, nameof(random));
			ThrowHelper.ThrowIfEmpty(list, nameof(list));

			return list[random.Next(list.Count)];
		}

		/// <summary>
		///     Finds the smallest element in a list.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="index">The index the smallest element was found at.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>The smallest value.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentException">If the list is empty.</exception>
		public static T GetSmallest<T>(this IList<T> list, out int index) where T : IComparable<T>
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfEmpty(list, nameof(list));

			T smallest = list[0];
			index = 0;

			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].CompareTo(smallest) < 0)
				{
					smallest = list[i];
					index = i;
				}
			}

			return smallest;
		}

		/// <summary>
		///     Finds the largest element in a list.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="index">The index the largest element was found at.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>The largest value.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentException">If the list is empty.</exception>
		public static T GetLargest<T>(this IList<T> list, out int index) where T : IComparable<T>
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfEmpty(list, nameof(list));

			T biggest = list[0];
			index = 0;

			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].CompareTo(biggest) > 0)
				{
					biggest = list[i];
					index = i;
				}
			}

			return biggest;
		}

		/// <summary>
		///     Returns all the values in a comma separated sequence.
		/// </summary>
		/// <param name="list">The list to get the values from.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>A string with all the values. If the list is empty, an empty string will be returned instead.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static string ToCommaSeparatedString<T>(this IList<T> list)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				return string.Empty;
			}

			return string.Join(", ", list);
		}

		/// <summary>
		///     Returns all the keys and values in a comma separated sequence.
		/// </summary>
		/// <param name="dictionary">The dictionary to get the values from.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <returns>A string with all the keys and values. If the dictionary is empty, an empty string will be returned instead.</returns>
		public static string ToCommaSeparatedString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			ThrowHelper.ThrowIfNull(dictionary, nameof(dictionary));

			if (dictionary.Count == 0)
			{
				return string.Empty;
			}

			return string.Join(", ", dictionary.Select(x => $"{{ {x.Key}: {x.Value} }}"));
		}

		/// <summary>
		///     Checks if a list is null or empty.
		/// </summary>
		/// <param name="list">The list to check.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>True if the list is null or empty, otherwise false.</returns>
		public static bool IsNullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		/// <summary>
		///     Checks if a dictionary is null or empty.
		/// </summary>
		/// <param name="dictionary">The dictionary to check.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <returns>True if the list is null or empty, otherwise false.</returns>
		public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
		{
			return dictionary == null || dictionary.Count == 0;
		}

		/// <summary>
		///     Returns a new random struct object with the current date as the seed.
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Random GetCurrentRandom()
		{
			return new Random((uint) DateTime.UtcNow.GetHashCode());
		}
	}
}