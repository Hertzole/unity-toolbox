using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public static partial class Extensions
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
		///     Converts a Vector3 to a Vector3Int.
		/// </summary>
		/// <param name="vector3">The Vector3 to convert.</param>
		/// <returns>The input Vector2 rounded to Vector2Int.</returns>
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
		///     Checks if the Vector2 has a NaN value.
		/// </summary>
		/// <param name="vector2">The Vector2 to check.</param>
		/// <returns>True if there's a NaN value, otherwise false.</returns>
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
		/// <returns>True if there's a NaN value, otherwise false.</returns>
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
		/// <param name="vector4">The Vector2 to check.</param>
		/// <returns>True if there's a NaN value, otherwise false.</returns>
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
		/// <returns>True if there's a NaN value, otherwise false.</returns>
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