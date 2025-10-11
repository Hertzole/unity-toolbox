using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     Extension methods for the Color type.
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        ///     Converts a <see cref="Color" /> to a hex string.
        /// </summary>
        /// <param name="color">The <see cref="Color" /> to get a hex string from.</param>
        /// <param name="displayAlpha">If <see langword="true" />, the alpha value will be included in the hex string.</param>
        /// <param name="uppercase">
        ///     If <see langword="true" />, the hex string will use uppercase characters. Otherwise, they will
        ///     be lowercase.
        /// </param>
        /// <returns>The color as a hex string.</returns>
        /// <example>
        ///     <code>
        ///     Color color = Color.red;
        ///     string hex = color.ToHex();
        ///     Debug.Log(hex); // #FF0000
        ///     </code>
        /// </example>
        public static string ToHex(this Color color, bool displayAlpha = false, bool uppercase = true)
        {
            return ToHex((Color32) color, displayAlpha, uppercase);
        }

        /// <summary>
        ///     Converts a <see cref="Color32" /> to a hex string.
        /// </summary>
        /// <param name="color">The <see cref="Color32" /> to get a hex string from.</param>
        /// <param name="displayAlpha">If <see langword="true" />, the alpha value will be included in the hex string.</param>
        /// <param name="uppercase">
        ///     If <see langword="true" />, the hex string will use uppercase characters. Otherwise, they will
        ///     be lowercase.
        /// </param>
        /// <returns>The color as a hex string.</returns>
        /// <example>
        ///     <code>
        ///     Color32 color = new Color32(255, 0, 0, 255);
        ///     string hex = color.ToHex();
        ///     Debug.Log(hex); // #FF0000
        ///     </code>
        /// </example>
        public static string ToHex(this Color32 color, bool displayAlpha = false, bool uppercase = true)
        {
            Span<char> hexBuffer = stackalloc char[displayAlpha ? 9 : 7]; // 8 characters for #RRGGBBAA
            ToHex(color, hexBuffer, displayAlpha, uppercase);
            return hexBuffer.ToString();
        }

        /// <summary>
        ///     Converts a <see cref="Color" /> to a hex string and puts the result in the provided span.
        /// </summary>
        /// <param name="color">The <see cref="Color" /> to get a hex string from.</param>
        /// <param name="destination">The destination <see cref="Span{T}" /> to write the hex string to.</param>
        /// <param name="displayAlpha">If <see langword="true" />, the alpha value will be included in the hex string.</param>
        /// <param name="uppercase">
        ///     If <see langword="true" />, the hex string will use uppercase characters. Otherwise, they will
        ///     be lowercase.
        /// </param>
        /// <returns>The color as a hex string.</returns>
        /// <example>
        ///     <code>
        ///     Span&lt;char&gt; hexBuffer = stackalloc char[7]; // 7 characters for #RRGGBB
        ///     Color color = new Color(1f, 0, 0, 1f);
        ///     color.ToHex(hexBuffer);
        ///     Debug.Log(hexBuffer.ToString()); // #FF0000
        ///     </code>
        /// </example>
        public static void ToHex(this Color color, Span<char> destination, bool displayAlpha = false, bool uppercase = true)
        {
            ToHex((Color32) color, destination, displayAlpha, uppercase);
        }

        /// <summary>
        ///     Converts a <see cref="Color32" /> to a hex string and puts the result in the provided span.
        /// </summary>
        /// <param name="color">The <see cref="Color32" /> to get a hex string from.</param>
        /// <param name="destination">The destination <see cref="Span{T}" /> to write the hex string to.</param>
        /// <param name="displayAlpha">If <see langword="true" />, the alpha value will be included in the hex string.</param>
        /// <param name="uppercase">
        ///     If <see langword="true" />, the hex string will use uppercase characters. Otherwise, they will
        ///     be lowercase.
        /// </param>
        /// <returns>The color as a hex string.</returns>
        /// <example>
        ///     <code>
        ///     Span&lt;char&gt; hexBuffer = stackalloc char[7]; // 7 characters for #RRGGBB
        ///     Color32 color = new Color32(255, 0, 0, 255);
        ///     color.ToHex(hexBuffer);
        ///     Debug.Log(hexBuffer.ToString()); // #FF0000
        ///     </code>
        /// </example>
        public static void ToHex(this Color32 color, Span<char> destination, bool displayAlpha = false, bool uppercase = true)
        {
            int requiredLength = displayAlpha ? 9 : 7;
            if (destination.Length < requiredLength)
            {
                throw new ArgumentException($"The destination span is too small. It must be at least {requiredLength} characters long.", nameof(destination));
            }

            destination[0] = '#';
            ByteToHex(color.r, destination, 1, in uppercase);
            ByteToHex(color.g, destination, 3, in uppercase);
            ByteToHex(color.b, destination, 5, in uppercase);
            if (displayAlpha)
            {
                ByteToHex(color.a, destination, 7, in uppercase);
            }
        }

        // Taken from https://github.com/dotnet/runtime/blob/0edefd061f456fa6021b36e341b392bbd6eff85c/src/libraries/Common/src/System/HexConverter.cs#L85
        private static void ByteToHex(byte value, Span<char> buffer, in int startingIndex, in bool uppercase)
        {
            uint casingValue = uppercase ? 0 : 0x2020U;

            uint difference = ((value & 0xF0U) << 4) + (value & 0x0FU) - 0x8989U;
            uint packedResult = ((((uint) -(int) difference & 0x7070U) >> 4) + difference + 0xB9B9U) | casingValue;

            buffer[startingIndex + 1] = (char) (packedResult & 0xFF);
            buffer[startingIndex] = (char) (packedResult >> 8);
        }
    }
}