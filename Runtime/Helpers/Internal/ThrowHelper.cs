using System;
using System.Collections.Generic;
using System.Diagnostics;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	internal static class ThrowHelper
	{
		/// <summary>
		///     Throws an exception if the given value is null.
		/// </summary>
		/// <param name="obj">The value to check.</param>
		/// <param name="paramName">The name of the parameter.</param>
		/// <typeparam name="T">The type of the object.</typeparam>
		/// <exception cref="ArgumentNullException">If the object is null.</exception>
		[Conditional("DEBUG")]
		public static void ThrowIfNull<T>(T obj, string paramName) where T : class
		{
#if DEBUG
			// Do some special checking for Unity objects as they override the null check.
			if (obj is Object unityObj && unityObj == null)
			{
				throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
			}

			if (obj == null)
			{
				throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
			}
#endif // DEBUG
		}

		/// <summary>
		///     Throws an exception if the list is empty.
		/// </summary>
		/// <param name="list">The list to check.</param>
		/// <param name="paramName">The name of the list.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <exception cref="ArgumentException">If the list is empty.</exception>
		[Conditional("DEBUG")]
		public static void ThrowIfEmpty<T>(IList<T> list, string paramName)
		{
#if DEBUG
			if (list.Count == 0)
			{
				throw new ArgumentException($"{paramName} cannot be empty.", paramName);
			}
#endif
		}

		/// <summary>
		///     Throws an exception if the dictionary is empty.
		/// </summary>
		/// <param name="dictionary">The dictionary to check.</param>
		/// <param name="paramName">The name of the dictionary.</param>
		/// <typeparam name="TKey">The key type.</typeparam>
		/// <typeparam name="TValue">The value type.</typeparam>
		/// <exception cref="ArgumentException">If the dictionary is empty.</exception>
		[Conditional("DEBUG")]
		public static void ThrowIfEmpty<TKey, TValue>(IDictionary<TKey, TValue> dictionary, string paramName)
		{
#if DEBUG
			if (dictionary.Count == 0)
			{
				throw new ArgumentException($"{paramName} cannot be empty.", paramName);
			}
#endif
		}
	}
}