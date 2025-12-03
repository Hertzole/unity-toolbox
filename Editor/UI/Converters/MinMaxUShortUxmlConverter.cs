#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxUShortUxmlConverter : MinMaxValueUxmlConverter<MinMaxUShort, ushort>
	{
		protected override ushort ParseValue(ReadOnlySpan<char> span)
		{
			return ushort.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxUShort CreateValue(ushort min, ushort max)
		{
			return new MinMaxUShort(min, max);
		}
	}
}
#endif

