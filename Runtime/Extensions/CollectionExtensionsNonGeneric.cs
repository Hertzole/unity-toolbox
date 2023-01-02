#if false
using System;
using System.Collections;
using Hertzole.UnityToolbox.Helpers;
using Random = Unity.Mathematics.Random;

namespace Hertzole.UnityToolbox
{
	public static partial class Extensions
	{
		/// <summary>
		///     Randomly shuffles a list.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle(this IList list)
		{
			Shuffle(list, GetCurrentRandom());
		}

		/// <summary>
		///     Randomly shuffles a list with a given seed.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle(this IList list, int seed)
		{
			Shuffle(list, new Random((uint) seed));
		}

		/// <summary>
		///     Randomly shuffles a list with a given seed.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle(this IList list, uint seed)
		{
			Shuffle(list, new Random(seed));
		}

		/// <summary>
		///     Randomly shuffles a list with a given random struct.
		/// </summary>
		/// <param name="list">The list to shuffle.</param>
		/// <param name="random">The random struct to use for randomizing.</param>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static void Shuffle(this IList list, Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				return;
			}

			for (int i = 0; i < list.Count; i++)
			{
				object temp = list[i];
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
		public static void Shuffle(this IList list, System.Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfNull(random, nameof(random));

			if (list.Count == 0)
			{
				return;
			}

			for (int i = 0; i < list.Count; i++)
			{
				object temp = list[i];
				int randomIndex = random.Next(list.Count);
				list[i] = list[randomIndex];
				list[randomIndex] = temp;
			}
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static object GetRandom(this IList list)
		{
			return GetRandom(list, GetCurrentRandom());
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static object GetRandom(this IList list, int seed)
		{
			return GetRandom(list, new Random((uint) seed));
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="seed">The seed to use when randomizing.</param>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static object GetRandom(this IList list, uint seed)
		{
			return GetRandom(list, new Random(seed));
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="random">The random struct to use for randomizing.</param>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static object GetRandom(this IList list, Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(list), "The list is empty.");
			}

			return list[random.NextInt(list.Count)];
		}

		/// <summary>
		///     Gets a random element from a list.
		/// </summary>
		/// <param name="list">The list to get the element from.</param>
		/// <param name="random">The random object to use for randomizing.</param>
		/// <returns>A random element from the list.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		/// <exception cref="ArgumentNullException">If the random object is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">If the list is empty.</exception>
		public static object GetRandom(this IList list, System.Random random)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			ThrowHelper.ThrowIfNull(random, nameof(random));

			if (list.Count == 0)
			{
				throw new ArgumentOutOfRangeException(nameof(list), "The list is empty.");
			}

			return list[random.Next(list.Count)];
		}
	}
}
#endif