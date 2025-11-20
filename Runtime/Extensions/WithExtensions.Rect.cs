using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    public static partial class WithExtensions
    {
        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the X (x) position replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="x">The new X (x) position.</param>
        /// <returns>A new <see cref="Rect" /> with the X position set to <paramref name="x" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="Rect" /> instance.
        ///     See also <see cref="WithY(Rect,float)" />, <see cref="WithPosition(Rect,Vector2)" />, and
        ///     <see cref="WithSize(Rect,Vector2)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithX(this Rect rect, float x)
        {
            return new Rect(x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the Y (y) position replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="y">The new Y (y) position.</param>
        /// <returns>A new <see cref="Rect" /> with the Y position set to <paramref name="y" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="Rect" /> instance.
        ///     See also <see cref="WithX(Rect,float)" />, <see cref="WithPosition(Rect,Vector2)" />, and
        ///     <see cref="WithSize(Rect,Vector2)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithY(this Rect rect, float y)
        {
            return new Rect(rect.x, y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the width replaced by <paramref name="width" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="width">The new width value.</param>
        /// <returns>A new <see cref="Rect" /> with the width set to <paramref name="width" /> and other values preserved.</returns>
        /// <remarks>
        ///     Width should be non-negative in typical use. This method does not modify the input <paramref name="rect" />; it
        ///     returns a new <see cref="Rect" />.
        ///     See also <see cref="WithHeight(Rect,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithWidth(this Rect rect, float width)
        {
            return new Rect(rect.x, rect.y, width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the height replaced by <paramref name="height" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="height">The new height value.</param>
        /// <returns>A new <see cref="Rect" /> with the height set to <paramref name="height" /> and other values preserved.</returns>
        /// <remarks>
        ///     Height should be non-negative in typical use. This method does not modify the input <paramref name="rect" />; it
        ///     returns a new <see cref="Rect" />.
        ///     See also <see cref="WithWidth(Rect,float)" />.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithHeight(this Rect rect, float height)
        {
            return new Rect(rect.x, rect.y, rect.width, height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the position replaced by <paramref name="position" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="position">
        ///     The new position as a <see cref="Vector2" />. The X component becomes the rect's x, the Y
        ///     component becomes the rect's y.
        /// </param>
        /// <returns>A new <see cref="Rect" /> with the position set to <paramref name="position" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="Rect" />. It is
        ///     equivalent to calling <see cref="WithX(Rect,float)" /> and <see cref="WithY(Rect,float)" /> together.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithPosition(this Rect rect, Vector2 position)
        {
            return new Rect(position.x, position.y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="Rect" /> with the size replaced by <paramref name="size" />.
        /// </summary>
        /// <param name="rect">The original <see cref="Rect" /> to copy.</param>
        /// <param name="size">The new size as a <see cref="Vector2" />, where X is width and Y is height.</param>
        /// <returns>A new <see cref="Rect" /> with the size set to <paramref name="size" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="Rect" />. It is
        ///     equivalent to calling <see cref="WithWidth(Rect,float)" /> and <see cref="WithHeight(Rect,float)" /> together.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect WithSize(this Rect rect, Vector2 size)
        {
            return new Rect(rect.x, rect.y, size.x, size.y);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the X (x) position replaced by <paramref name="x" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="x">The new X (x) position.</param>
        /// <returns>A new <see cref="RectInt" /> with the X position set to <paramref name="x" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithX(this RectInt rect, int x)
        {
            return new RectInt(x, rect.y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the Y (y) position replaced by <paramref name="y" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="y">The new Y (y) position.</param>
        /// <returns>A new <see cref="RectInt" /> with the Y position set to <paramref name="y" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithY(this RectInt rect, int y)
        {
            return new RectInt(rect.x, y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the width replaced by <paramref name="width" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="width">The new width value.</param>
        /// <returns>A new <see cref="RectInt" /> with the width set to <paramref name="width" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithWidth(this RectInt rect, int width)
        {
            return new RectInt(rect.x, rect.y, width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the height replaced by <paramref name="height" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="height">The new height value.</param>
        /// <returns>A new <see cref="RectInt" /> with the height set to <paramref name="height" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithHeight(this RectInt rect, int height)
        {
            return new RectInt(rect.x, rect.y, rect.width, height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the position replaced by <paramref name="position" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="position">The new position as a <see cref="Vector2Int" />, where X becomes rect.x and Y becomes rect.y.</param>
        /// <returns>A new <see cref="RectInt" /> with the position set to <paramref name="position" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithPosition(this RectInt rect, Vector2Int position)
        {
            return new RectInt(position.x, position.y, rect.width, rect.height);
        }

        /// <summary>
        ///     Returns a copy of the specified <see cref="RectInt" /> with the size replaced by <paramref name="size" />.
        /// </summary>
        /// <param name="rect">The original <see cref="RectInt" /> to copy.</param>
        /// <param name="size">The new size as a <see cref="Vector2Int" />, where X is width and Y is height.</param>
        /// <returns>A new <see cref="RectInt" /> with the size set to <paramref name="size" /> and other values preserved.</returns>
        /// <remarks>
        ///     This method does not modify the input <paramref name="rect" />; it returns a new <see cref="RectInt" /> instance.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectInt WithSize(this RectInt rect, Vector2Int size)
        {
            return new RectInt(rect.x, rect.y, size.x, size.y);
        }
    }
}