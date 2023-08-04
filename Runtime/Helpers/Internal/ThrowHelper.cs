#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Object = UnityEngine.Object;
#if NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

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
		public static void ThrowIfNull<T>(
#if NETSTANDARD2_1
			[NotNull]
#endif
			T? obj,
			string paramName) where T : class
		{
#if DEBUG
			// Do some special checking for Unity objects as they override the null check.
			if ((obj is Object unityObj && unityObj == null) || obj == null)
			{
				throw new ArgumentNullException(paramName, $"{paramName} cannot be null.");
			}
#endif // DEBUG
		}

		/// <summary>
		///     Throws an exception if the collection is empty.
		/// </summary>
		/// <param name="list">The collection to check.</param>
		/// <param name="paramName">The name of the collection.</param>
		/// <typeparam name="T">The type in the collection.</typeparam>
		/// <exception cref="ArgumentException">If the collection is empty.</exception>
		[Conditional("DEBUG")]
		public static void ThrowIfEmpty<T>(ICollection<T> list, string paramName)
		{
#if DEBUG
			if (list.Count == 0)
			{
				throw new ArgumentException($"{paramName} cannot be empty.", paramName);
			}
#endif // DEBUG
		}
	}
}