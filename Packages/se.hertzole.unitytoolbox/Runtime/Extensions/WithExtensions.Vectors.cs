using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    public static partial class WithExtensions
    {
        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector2" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector2" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>A new <see cref="Vector2" /> with the X component set to <paramref name="x" /> and the Y component preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector2" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithX(this Vector2 vector, float x)
        {
            return new Vector2(x, vector.y);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector2" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector2" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>A new <see cref="Vector2" /> with the Y component set to <paramref name="y" /> and the X component preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector2" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithY(this Vector2 vector, float y)
        {
            return new Vector2(vector.x, y);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector2Int" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector2Int" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>
        ///     A new <see cref="Vector2Int" /> with the X component set to <paramref name="x" /> and the Y component
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector2Int" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int WithX(this Vector2Int vector, int x)
        {
            return new Vector2Int(x, vector.y);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector2Int" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector2Int" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>
        ///     A new <see cref="Vector2Int" /> with the Y component set to <paramref name="y" /> and the X component
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector2Int" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int WithY(this Vector2Int vector, int y)
        {
            return new Vector2Int(vector.x, y);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>A new <see cref="Vector3" /> with the X component set to <paramref name="x" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>A new <see cref="Vector3" /> with the Y component set to <paramref name="y" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3" /> with the Z component replaced by <paramref name="z" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3" /> to copy.</param>
        /// <param name="z">The new Z component value.</param>
        /// <returns>A new <see cref="Vector3" /> with the Z component set to <paramref name="z" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3Int" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3Int" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>
        ///     A new <see cref="Vector3Int" /> with the X component set to <paramref name="x" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3Int" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int WithX(this Vector3Int vector, int x)
        {
            return new Vector3Int(x, vector.y, vector.z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3Int" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3Int" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>
        ///     A new <see cref="Vector3Int" /> with the Y component set to <paramref name="y" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3Int" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int WithY(this Vector3Int vector, int y)
        {
            return new Vector3Int(vector.x, y, vector.z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector3Int" /> with the Z component replaced by <paramref name="z" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector3Int" /> to copy.</param>
        /// <param name="z">The new Z component value.</param>
        /// <returns>
        ///     A new <see cref="Vector3Int" /> with the Z component set to <paramref name="z" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector3Int" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int WithZ(this Vector3Int vector, int z)
        {
            return new Vector3Int(vector.x, vector.y, z);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector4" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector4" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>A new <see cref="Vector4" /> with the X component set to <paramref name="x" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector4" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 WithX(this Vector4 vector, float x)
        {
            return new Vector4(x, vector.y, vector.z, vector.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector4" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector4" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>A new <see cref="Vector4" /> with the Y component set to <paramref name="y" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector4" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 WithY(this Vector4 vector, float y)
        {
            return new Vector4(vector.x, y, vector.z, vector.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector4" /> with the Z component replaced by <paramref name="z" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector4" /> to copy.</param>
        /// <param name="z">The new Z component value.</param>
        /// <returns>A new <see cref="Vector4" /> with the Z component set to <paramref name="z" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector4" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 WithZ(this Vector4 vector, float z)
        {
            return new Vector4(vector.x, vector.y, z, vector.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Vector4" /> with the W component replaced by <paramref name="w" />.
        /// </summary>
        /// <param name="vector">The original <see cref="Vector4" /> to copy.</param>
        /// <param name="w">The new W component value.</param>
        /// <returns>A new <see cref="Vector4" /> with the W component set to <paramref name="w" /> and other components preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="vector" />; it returns a new <see cref="Vector4" />
        ///     instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 WithW(this Vector4 vector, float w)
        {
            return new Vector4(vector.x, vector.y, vector.z, w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Quaternion" /> with the X component replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="quaternion">The original <see cref="Quaternion" /> to copy.</param>
        /// <param name="x">The new X component value.</param>
        /// <returns>
        ///     A new <see cref="Quaternion" /> with the X component set to <paramref name="x" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="quaternion" />; it returns a new
        ///     <see cref="Quaternion" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion WithX(this Quaternion quaternion, float x)
        {
            return new Quaternion(x, quaternion.y, quaternion.z, quaternion.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Quaternion" /> with the Y component replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="quaternion">The original <see cref="Quaternion" /> to copy.</param>
        /// <param name="y">The new Y component value.</param>
        /// <returns>
        ///     A new <see cref="Quaternion" /> with the Y component set to <paramref name="y" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="quaternion" />; it returns a new
        ///     <see cref="Quaternion" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion WithY(this Quaternion quaternion, float y)
        {
            return new Quaternion(quaternion.x, y, quaternion.z, quaternion.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Quaternion" /> with the Z component replaced by <paramref name="z" />.
        /// </summary>
        /// <param name="quaternion">The original <see cref="Quaternion" /> to copy.</param>
        /// <param name="z">The new Z component value.</param>
        /// <returns>
        ///     A new <see cref="Quaternion" /> with the Z component set to <paramref name="z" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="quaternion" />; it returns a new
        ///     <see cref="Quaternion" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion WithZ(this Quaternion quaternion, float z)
        {
            return new Quaternion(quaternion.x, quaternion.y, z, quaternion.w);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Quaternion" /> with the W component replaced by <paramref name="w" />.
        /// </summary>
        /// <param name="quaternion">The original <see cref="Quaternion" /> to copy.</param>
        /// <param name="w">The new W component value.</param>
        /// <returns>
        ///     A new <see cref="Quaternion" /> with the W component set to <paramref name="w" /> and other components
        ///     preserved.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="quaternion" />; it returns a new
        ///     <see cref="Quaternion" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Quaternion WithW(this Quaternion quaternion, float w)
        {
            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, w);
        }
    }
}