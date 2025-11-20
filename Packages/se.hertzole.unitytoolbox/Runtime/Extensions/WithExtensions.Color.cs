using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    public static partial class WithExtensions
    {
        /// <summary>
        ///     Returns a copy of the specified <see cref="Color" /> with the red component replaced by <paramref name="red" />.
        /// </summary>
        /// <param name="color">The original <see cref="Color" /> to copy.</param>
        /// <param name="red">The new red component value. Expected range is 0 to 1.</param>
        /// <returns>
        ///     A new <see cref="Color" /> with the same green, blue and alpha components as the original, and the red
        ///     component set to <paramref name="red" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color" />.
        ///     See also <see cref="WithGreen(Color,float)" />, <see cref="WithBlue(Color,float)" /> and
        ///     <see cref="WithAlpha(Color,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithRed(this Color color, float red)
        {
            return new Color(red, color.g, color.b, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color" /> with the green component replaced by <paramref name="green" />
        ///     .
        /// </summary>
        /// <param name="color">The original <see cref="Color" /> to copy.</param>
        /// <param name="green">The new green component value. Expected range is 0 to 1.</param>
        /// <returns>
        ///     A new <see cref="Color" /> with the same red, blue and alpha components as the original, and the green
        ///     component set to <paramref name="green" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color" />.
        ///     See also <see cref="WithRed(Color,float)" />, <see cref="WithBlue(Color,float)" /> and
        ///     <see cref="WithAlpha(Color,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithGreen(this Color color, float green)
        {
            return new Color(color.r, green, color.b, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color" /> with the blue component replaced by <paramref name="blue" />.
        /// </summary>
        /// <param name="color">The original <see cref="Color" /> to copy.</param>
        /// <param name="blue">The new blue component value. Expected range is 0 to 1.</param>
        /// <returns>
        ///     A new <see cref="Color" /> with the same red, green and alpha components as the original, and the blue
        ///     component set to <paramref name="blue" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color" />.
        ///     See also <see cref="WithRed(Color,float)" />, <see cref="WithGreen(Color,float)" /> and
        ///     <see cref="WithAlpha(Color,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithBlue(this Color color, float blue)
        {
            return new Color(color.r, color.g, blue, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color" /> with the alpha component replaced by <paramref name="alpha" />
        ///     .
        /// </summary>
        /// <param name="color">The original <see cref="Color" /> to copy.</param>
        /// <param name="alpha">The new alpha component value. Expected range is 0 to 1.</param>
        /// <returns>
        ///     A new <see cref="Color" /> with the same red, green and blue components as the original, and the alpha
        ///     component set to <paramref name="alpha" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color" />.
        ///     See also <see cref="WithRed(Color,float)" />, <see cref="WithGreen(Color,float)" /> and
        ///     <see cref="WithBlue(Color,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color32" /> with the red component replaced by <paramref name="red" />.
        /// </summary>
        /// <param name="color">The original <see cref="Color32" /> to copy.</param>
        /// <param name="red">The new red component value. Expected range is 0 to 255.</param>
        /// <returns>
        ///     A new <see cref="Color32" /> with the same green, blue and alpha components as the original, and the red
        ///     component set to <paramref name="red" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color32" />.
        ///     See also <see cref="WithGreen(Color32,byte)" />, <see cref="WithBlue(Color32,byte)" /> and
        ///     <see cref="WithAlpha(Color32,byte)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 WithRed(this Color32 color, byte red)
        {
            return new Color32(red, color.g, color.b, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color32" /> with the green component replaced by
        ///     <paramref name="green" />.
        /// </summary>
        /// <param name="color">The original <see cref="Color32" /> to copy.</param>
        /// <param name="green">The new green component value. Expected range is 0 to 255.</param>
        /// <returns>
        ///     A new <see cref="Color32" /> with the same red, blue and alpha components as the original, and the green
        ///     component set to <paramref name="green" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color32" />.
        ///     See also <see cref="WithRed(Color32,byte)" />, <see cref="WithBlue(Color32,byte)" /> and
        ///     <see cref="WithAlpha(Color32,byte)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 WithGreen(this Color32 color, byte green)
        {
            return new Color32(color.r, green, color.b, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color32" /> with the blue component replaced by <paramref name="blue" />
        ///     .
        /// </summary>
        /// <param name="color">The original <see cref="Color32" /> to copy.</param>
        /// <param name="blue">The new blue component value. Expected range is 0 to 255.</param>
        /// <returns>
        ///     A new <see cref="Color32" /> with the same red, green and alpha components as the original, and the blue
        ///     component set to <paramref name="blue" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color32" />.
        ///     See also <see cref="WithRed(Color32,byte)" />, <see cref="WithGreen(Color32,byte)" /> and
        ///     <see cref="WithAlpha(Color32,byte)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 WithBlue(this Color32 color, byte blue)
        {
            return new Color32(color.r, color.g, blue, color.a);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Color32" /> with the alpha component replaced by
        ///     <paramref name="alpha" />.
        /// </summary>
        /// <param name="color">The original <see cref="Color32" /> to copy.</param>
        /// <param name="alpha">The new alpha component value. Expected range is 0 to 255.</param>
        /// <returns>
        ///     A new <see cref="Color32" /> with the same red, green and blue components as the original, and the alpha
        ///     component set to <paramref name="alpha" />.
        /// </returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="color" /> instance; it returns a new <see cref="Color32" />.
        ///     See also <see cref="WithRed(Color32,byte)" />, <see cref="WithGreen(Color32,byte)" /> and
        ///     <see cref="WithBlue(Color32,byte)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color32 WithAlpha(this Color32 color, byte alpha)
        {
            return new Color32(color.r, color.g, color.b, alpha);
        }
    }
}