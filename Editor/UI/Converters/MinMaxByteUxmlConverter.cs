#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxByteUxmlConverter : MinMaxValueUxmlConverter<MinMaxByte, byte>
	{
		protected override byte ParseValue(ReadOnlySpan<char> span)
		{
			return byte.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxByte CreateValue(byte min, byte max)
		{
			return new MinMaxByte(min, max);
		}
	}
}
#endif

