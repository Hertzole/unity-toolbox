using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     Provides extension methods for approximate equality comparisons of float types.
    /// </summary>
    public static class EqualityExtensions
    {
        private const float DEFAULT_TOLERANCE = 0.0001f;

        /// <summary>
        ///     Determines whether two <see cref="Vector2" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />.
        /// </summary>
        /// <example>
        ///     <code>
        /// Vector2 a = new Vector2(1.0f, 2.0f);
        /// Vector2 b = new Vector2(1.00005f, 2.00005f);
        /// // With the default tolerance they are approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // With a much tighter tolerance they are not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.00001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Vector2" /> instance.</param>
        /// <param name="b">The second <see cref="Vector2" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding components for the vectors to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the vectors are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Vector2 a, Vector2 b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.x, b.x, tolerance) && EqualsApproximately(a.y, b.y, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="Vector3" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />.
        /// </summary>
        /// <example>
        ///     <code>
        /// Vector3 a = new Vector3(1.0f, 2.0f, 3.0f);
        /// Vector3 b = new Vector3(1.00005f, 2.00005f, 2.99995f);
        /// // With the default tolerance they are approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // With a much tighter tolerance they are not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.00001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Vector3" /> instance.</param>
        /// <param name="b">The second <see cref="Vector3" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding components for the vectors to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the vectors are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Vector3 a, Vector3 b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.x, b.x, tolerance) && EqualsApproximately(a.y, b.y, tolerance) && EqualsApproximately(a.z, b.z, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="Vector4" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />.
        /// </summary>
        /// <example>
        ///     <code>
        /// Vector4 a = new Vector4(1.0f, 2.0f, 3.0f, 4.0f);
        /// Vector4 b = new Vector4(1.0f, 2.0001f, 2.9999f, 4.0f);
        /// // Loose tolerance: approximately equal.
        /// bool approxLoose = a.EqualsApproximately(b, 0.0002f); // true
        /// // Tight tolerance: not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.00001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Vector4" /> instance.</param>
        /// <param name="b">The second <see cref="Vector4" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding components for the vectors to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the vectors are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Vector4 a, Vector4 b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.x, b.x, tolerance) && EqualsApproximately(a.y, b.y, tolerance) &&
                   EqualsApproximately(a.z, b.z, tolerance) && EqualsApproximately(a.w, b.w, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="Quaternion" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />. Note that quaternions that represent the same rotation may differ by a sign
        ///     (q and -q); this method performs a component-wise comparison and does not account for that equivalence.
        /// </summary>
        /// <example>
        ///     <code>
        /// Quaternion a = Quaternion.Euler(0, 45, 0);
        /// Quaternion b = Quaternion.Euler(0, 45.00001f, 0);
        /// // Default tolerance: approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // Tight tolerance: not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.000001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Quaternion" /> instance.</param>
        /// <param name="b">The second <see cref="Quaternion" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding components for the quaternions to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the quaternions are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Quaternion a, Quaternion b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.x, b.x, tolerance) && EqualsApproximately(a.y, b.y, tolerance) &&
                   EqualsApproximately(a.z, b.z, tolerance) && EqualsApproximately(a.w, b.w, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="Color" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />.
        /// </summary>
        /// <example>
        ///     <code>
        /// Color a = new Color(1f, 0.5f, 0.25f, 1f);
        /// Color b = new Color(1.00005f, 0.49995f, 0.25001f, 1f);
        /// // Default tolerance: approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // Tight tolerance: not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.00001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Color" /> instance.</param>
        /// <param name="b">The second <see cref="Color" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding channels for the colors to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the colors are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Color a, Color b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.r, b.r, tolerance) && EqualsApproximately(a.g, b.g, tolerance) &&
                   EqualsApproximately(a.b, b.b, tolerance) && EqualsApproximately(a.a, b.a, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="Rect" /> instances are approximately equal within a specified
        ///     <paramref name="tolerance" />. Comparison is performed on width, height and position components.
        /// </summary>
        /// <example>
        ///     <code>
        /// Rect a = new Rect(0f, 0f, 100f, 50f);
        /// Rect b = new Rect(0.00005f, 0.00002f, 100.00003f, 49.99998f);
        /// // Default tolerance: approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // Tight tolerance: not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.000001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="Rect" /> instance.</param>
        /// <param name="b">The second <see cref="Rect" /> instance.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between corresponding components for the rectangles to be
        ///     considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the rectangles are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this Rect a, Rect b, float tolerance = DEFAULT_TOLERANCE)
        {
            return EqualsApproximately(a.width, b.width, tolerance) && EqualsApproximately(a.height, b.height, tolerance) &&
                   EqualsApproximately(a.x, b.x, tolerance) && EqualsApproximately(a.y, b.y, tolerance);
        }

        /// <summary>
        ///     Determines whether two <see cref="float" /> values are approximately equal within a specified
        ///     <paramref name="tolerance" />.
        /// </summary>
        /// <example>
        ///     <code>
        /// float a = 1.0f;
        /// float b = 1.00005f;
        /// // Default tolerance: approximately equal.
        /// bool approxDefault = a.EqualsApproximately(b); // true
        /// // Tight tolerance: not approximately equal.
        /// bool approxTight = a.EqualsApproximately(b, 0.000001f); // false
        /// </code>
        /// </example>
        /// <param name="a">The first <see cref="float" /> value.</param>
        /// <param name="b">The second <see cref="float" /> value.</param>
        /// <param name="tolerance">
        ///     The maximum allowable difference between the values for them to be considered approximately equal.
        /// </param>
        /// <returns><see langword="true" /> if the values are approximately equal; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsApproximately(this float a, float b, float tolerance = DEFAULT_TOLERANCE)
        {
            return Mathf.Abs(a - b) <= tolerance;
        }
    }
}