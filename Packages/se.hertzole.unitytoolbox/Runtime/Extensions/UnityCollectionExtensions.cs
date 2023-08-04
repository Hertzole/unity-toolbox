using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Extension methods for collections with Unity types.
	/// </summary>
	public static class UnityCollectionExtensions
	{
		/// <summary>
		///     Gets the closest object to the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="closestDistance">The distance to the closest object. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the closest object. Will be -1 if the list is empty.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>The closest object.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static T GetClosest<T>(this IList<T> list, Vector3 position, out float closestDistance, out int targetIndex) where T : Component
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				closestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].transform.position;
			}

			targetIndex = GetClosestIndex(spanList, position, out closestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the closest game object to the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="closestDistance">The distance to the closest game object. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the closest game object. Will be -1 if the list is empty.</param>
		/// <returns>The closest game object.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static GameObject GetClosest(this IList<GameObject> list, Vector3 position, out float closestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				closestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].transform.position;
			}

			targetIndex = GetClosestIndex(spanList, position, out closestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the closest transform to the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="closestDistance">The distance to the closest transform. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the closest transform. Will be -1 if the list is empty.</param>
		/// <returns>The closest transform.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Transform GetClosest(this IList<Transform> list, Vector3 position, out float closestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			
			if (list.Count == 0)
			{
				closestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].position;
			}

			targetIndex = GetClosestIndex(spanList, position, out closestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the closest position to the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="closestDistance">The distance to the closest position. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the closest position. Will be -1 if the list is empty.</param>
		/// <returns>The closest position.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Vector3 GetClosest(this IList<Vector3> list, Vector3 position, out float closestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				closestDistance = 0;
				targetIndex = -1;
				return Vector3.zero;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i];
			}

			targetIndex = GetClosestIndex(spanList, position, out closestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the closest position to the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="closestDistance">The distance to the closest position. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the closest position. Will be -1 if the list is empty.</param>
		/// <returns>The closest position.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Vector2 GetClosest(this IList<Vector2> list, Vector2 position, out float closestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				closestDistance = 0;
				targetIndex = -1;
				return Vector2.zero;
			}
            
			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i];
			}

			targetIndex = GetClosestIndex(spanList, position, out closestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the closest index in the list.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check.</param>
		/// <param name="closestDistance">The closest distance.</param>
		/// <returns>The index of the closest position.</returns>
		private static int GetClosestIndex(ReadOnlySpan<Vector3> list, Vector3 position, out float closestDistance)
		{
			int closest = 0;
			closestDistance = Vector3.Distance(list[0], position);

			for (int i = 1; i < list.Length; i++)
			{
				float distance = Vector3.Distance(list[i], position);
				if (distance < closestDistance)
				{
					closest = i;
					closestDistance = distance;
				}
			}

			return closest;
		}

		/// <summary>
		///     Gets the furthest object from the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="furthestDistance">The distance to the furthest object. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the furthest object. Will be -1 if the list is empty.</param>
		/// <typeparam name="T">The type in the list.</typeparam>
		/// <returns>The furthest object.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static T GetFurthest<T>(this IList<T> list, Vector3 position, out float furthestDistance, out int targetIndex) where T : Component
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				furthestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].transform.position;
			}

			targetIndex = GetFurthestIndex(spanList, position, out furthestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the furthest game object from the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="furthestDistance">The distance to the furthest game object. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the furthest game object. Will be -1 if the list is empty.</param>
		/// <returns>The furthest game object.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static GameObject GetFurthest(this IList<GameObject> list, Vector3 position, out float furthestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));

			if (list.Count == 0)
			{
				furthestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].transform.position;
			}

			targetIndex = GetFurthestIndex(spanList, position, out furthestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the furthest transform from the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="furthestDistance">The distance to the furthest transform. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the furthest transform. Will be -1 if the list is empty.</param>
		/// <returns>The furthest transform.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Transform GetFurthest(this IList<Transform> list, Vector3 position, out float furthestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			
			if (list.Count == 0)
			{
				furthestDistance = 0;
				targetIndex = -1;
				return null;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i].position;
			}

			targetIndex = GetFurthestIndex(spanList, position, out furthestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the furthest position from the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="furthestDistance">The distance to the furthest position. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the furthest position. Will be -1 if the list is empty.</param>
		/// <returns>The furthest position.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Vector3 GetFurthest(this IList<Vector3> list, Vector3 position, out float furthestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
			
			if (list.Count == 0)
			{
				furthestDistance = 0;
				targetIndex = -1;
				return Vector2.zero;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i];
			}

			targetIndex = GetFurthestIndex(spanList, position, out furthestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the furthest position from the given position.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check towards.</param>
		/// <param name="furthestDistance">The distance to the furthest position. Will be 0 if the list is empty.</param>
		/// <param name="targetIndex">The index of the furthest position. Will be -1 if the list is empty.</param>
		/// <returns>The furthest position.</returns>
		/// <exception cref="ArgumentNullException">If the list is null.</exception>
		public static Vector2 GetFurthest(this IList<Vector2> list, Vector2 position, out float furthestDistance, out int targetIndex)
		{
			ThrowHelper.ThrowIfNull(list, nameof(list));
            
			if (list.Count == 0)
			{
				furthestDistance = 0;
				targetIndex = -1;
				return Vector2.zero;
			}

			Span<Vector3> spanList = stackalloc Vector3[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				spanList[i] = list[i];
			}

			targetIndex = GetFurthestIndex(spanList, position, out furthestDistance);

			return list[targetIndex];
		}

		/// <summary>
		///     Gets the furthest index in the list.
		/// </summary>
		/// <param name="list">The list to search.</param>
		/// <param name="position">The position to check.</param>
		/// <param name="furthestDistance">The furthest distance.</param>
		/// <returns>The index of the furthest position.</returns>
		private static int GetFurthestIndex(ReadOnlySpan<Vector3> list, Vector3 position, out float furthestDistance)
		{
			int furthest = 0;
			furthestDistance = Vector3.Distance(list[0], position);

			for (int i = 1; i < list.Length; i++)
			{
				float distance = Vector3.Distance(list[i], position);
				if (distance > furthestDistance)
				{
					furthest = i;
					furthestDistance = distance;
				}
			}

			return furthest;
		}
	}
}