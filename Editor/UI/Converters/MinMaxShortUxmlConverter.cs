#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxShortUxmlConverter : MinMaxValueUxmlConverter<MinMaxShort, short>
	{
		protected override short ParseValue(ReadOnlySpan<char> span)
		{
			return short.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxShort CreateValue(short min, short max)
		{
			return new MinMaxShort(min, max);
		}
	}
}
#endif

