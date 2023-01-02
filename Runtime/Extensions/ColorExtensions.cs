using UnityEngine;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Extension methods for the Color type.
	/// </summary>
	public static class ColorExtensions
	{
		/// <summary>
		///     Converts a color to a hex string.
		/// </summary>
		/// <param name="color">The color to get a hex string from.</param>
		/// <param name="displayAlpha">If true, the alpha value will be included in the hex string.</param>
		/// <returns>The color as a hex string.</returns>
		/// <example>
		///     <code>
		/// 	Color color = Color.red;
		///  string hex = color.ToHex();
		///  Debug.Log(hex); // #FF0000
		///  </code>
		/// </example>
		public static string ToHex(this Color color, bool displayAlpha = false)
		{
			return ToHex((Color32) color, displayAlpha);
		}

		/// <summary>
		///     Converts a color to a hex string.
		/// </summary>
		/// <param name="color">The color to get a hex string from.</param>
		/// <param name="displayAlpha">If true, the alpha value will be included in the hex string.</param>
		/// <returns>The color as a hex string.</returns>
		/// <example>
		///     <code>
		/// 	Color32 color = new Color32(255, 0, 0, 255);
		///  string hex = color.ToHex();
		///  Debug.Log(hex); // #FF0000
		///  </code>
		/// </example>
		public static string ToHex(this Color32 color, bool displayAlpha = false)
		{
			if (displayAlpha)
			{
				return $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
			}

			return $"#{color.r:X2}{color.g:X2}{color.b:X2}";
		}
	}
}