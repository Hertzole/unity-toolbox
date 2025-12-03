#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxULongUxmlConverter : MinMaxValueUxmlConverter<MinMaxULong, ulong>
	{
		protected override ulong ParseValue(ReadOnlySpan<char> span)
		{
			return ulong.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxULong CreateValue(ulong min, ulong max)
		{
			return new MinMaxULong(min, max);
		}
	}
}
#endif

