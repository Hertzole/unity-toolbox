using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Extension methods related to Unity vectors.
	/// </summary>
	public static class VectorExtensions
	{
		/// <summary>
		///     Converts a Vector2 to a Vector2Int.
		/// </summary>
		/// <param name="vector2">The Vector2 to convert.</param>
		/// <returns>The input Vector2 rounded to Vector2Int.</returns>
		/// <example>
		///     <code>
		///     Vector2 worldPosition = new Vector2(1, 2);
		///     Vector2Int gridPosition = worldPosition.ToInt();
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2Int ToInt(this Vector2 vector2)
		{
			return new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
		}

        /// <summary>
        ///     Floors each component of a <see cref="Vector2"/> to an integer and converts it to a <see cref="Vector2Int"/>.
        /// </summary>
        /// <param name="vector2">The <see cref="Vector2"/> to convert.</param>
        /// <returns>The input <see cref="Vector2"/> floored to a <see cref="Vector2Int"/>.</returns>
        /// <example>
        ///     <code>
        ///     Vector2 worldPosition = new Vector2(1.5f, 2.7f);
        ///     Vector2Int gridPosition = worldPosition.FloorToInt();
        ///     </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int FloorToInt(this Vector2 vector2)
        {
            return new Vector2Int(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y));
        }

        /// <summary>
        ///     Ceils each component of a <see cref="Vector2"/> to an integer and converts it to a <see cref="Vector2Int"/>.
        /// </summary>
        /// <param name="vector2">The <see cref="Vector2"/> to convert.</param>
        /// <returns>The input <see cref="Vector2"/> ceiled to a <see cref="Vector2Int"/>.</returns>
        /// <example>
        ///     <code>
        ///     Vector2 worldPosition = new Vector2(1.2f, 2.3f);
        ///     Vector2Int gridPosition = worldPosition.CeilToInt();
        ///     </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CeilToInt(this Vector2 vector2)
        {
            return new Vector2Int(Mathf.CeilToInt(vector2.x), Mathf.CeilToInt(vector2.y));
        }

		/// <summary>
		///     Converts a Vector3 to a Vector3Int.
		/// </summary>
		/// <param name="vector3">The Vector3 to convert.</param>
		/// <returns>The input <see cref="Vector3"/> rounded to <see cref="Vector3Int"/>.</returns>
		/// <example>
		///     <code>
		///     Vector3 worldPosition = new Vector3(1, 2, 3);
		///     Vector3Int gridPosition = worldPosition.ToInt();
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Int ToInt(this Vector3 vector3)
		{
			return new Vector3Int(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
		}
        
        /// <summary>
        ///     Floors each component of a <see cref="Vector3"/> to an integer and converts it to a <see cref="Vector3Int"/>.
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> to convert.</param>
        /// <returns>The input <see cref="Vector3"/> floored to a <see cref="Vector3Int"/>.</returns>
        /// <example>
        ///     <code>
        ///     Vector3 worldPosition = new Vector3(1.5f, 2.7f, 3.1f);
        ///     Vector3Int gridPosition = worldPosition.FloorToInt();
        ///     </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int FloorToInt(this Vector3 vector3)
        {
            return new Vector3Int(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.y), Mathf.FloorToInt(vector3.z));
        }

        /// <summary>
        ///     Ceils each component of a <see cref="Vector3"/> to an integer and converts it to a <see cref="Vector3Int"/>.
        /// </summary>
        /// <param name="vector3">The <see cref="Vector3"/> to convert.</param>
        /// <returns>The input <see cref="Vector3"/> ceiled to a <see cref="Vector3Int"/>.</returns>
        /// <example>
        ///     <code>
        ///     Vector3 worldPosition = new Vector3(1.2f, 2.3f, 3.4f);
        ///     Vector3Int gridPosition = worldPosition.CeilToInt();
        ///     </code>
        /// </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CeilToInt(this Vector3 vector3)
        {
            return new Vector3Int(Mathf.CeilToInt(vector3.x), Mathf.CeilToInt(vector3.y), Mathf.CeilToInt(vector3.z));
        }

		/// <summary>
		///     Checks if the Vector2 has a NaN value.
		/// </summary>
		/// <param name="vector2">The Vector2 to check.</param>
		/// <returns><see langword="true"/> if there's a NaN value; otherwise <see langword="false"/>.</returns>
		/// <example>
		///     <code>
		///     Vector2 v = new Vector2(1, 2);
		///     if (v.IsNaN())
		///     {
		///    		Debug.Log("Vector2 has NaN value!");
		///    		return;
		///     }
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Vector2 vector2)
		{
			return float.IsNaN(vector2.x) || float.IsNaN(vector2.y);
		}

		/// <summary>
		///     Checks if the Vector3 has a NaN value.
		/// </summary>
		/// <param name="vector3">The Vector3 to check.</param>
		/// <returns><see langword="true"/> if there's a NaN value; otherwise <see langword="false"/>.</returns>
		/// <example>
		///     <code>
		///     Vector3 v = new Vector3(1, 2, 3);
		///     if (v.IsNaN())
		///     {
		///    		Debug.Log("Vector3 has NaN value!");
		///    		return;
		///     }
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Vector3 vector3)
		{
			return float.IsNaN(vector3.x) || float.IsNaN(vector3.y) || float.IsNaN(vector3.z);
		}

		/// <summary>
		///     Checks if the Vector4 has a NaN value.
		/// </summary>
		/// <param name="vector4">The <see cref="Vector4"/> to check.</param>
		/// <returns><see langword="true"/> if there's a NaN value; otherwise <see langword="false"/>.</returns>
		/// <example>
		///     <code>
		///     Vector4 v = new Vector4(1, 2, 3, 4);
		///     if (v.IsNaN())
		///     {
		///    		Debug.Log("Vector4 has NaN value!");
		///    		return;
		///     }
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Vector4 vector4)
		{
			return float.IsNaN(vector4.x) || float.IsNaN(vector4.y) || float.IsNaN(vector4.z) || float.IsNaN(vector4.w);
		}

		/// <summary>
		///     Checks if the Quaternion has a NaN value.
		/// </summary>
		/// <param name="quaternion">The Quaternion to check.</param>
		/// <returns><see langword="true"/> if there's a NaN value; otherwise <see langword="false"/>.</returns>
		/// <example>
		///     <code>
		///     Quaternion q = new Quaternion(1, 2, 3, 4);
		///     if (q.IsNaN())
		///     {
		///    		Debug.Log("Quaternion has NaN value!");
		///    		return;
		///     }
		///     </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNaN(this Quaternion quaternion)
		{
			return float.IsNaN(quaternion.x) || float.IsNaN(quaternion.y) || float.IsNaN(quaternion.z) || float.IsNaN(quaternion.w);
		}
	}
}