#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxLongUxmlConverter : MinMaxValueUxmlConverter<MinMaxLong, long>
	{
		protected override long ParseValue(ReadOnlySpan<char> span)
		{
			return long.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxLong CreateValue(long min, long max)
		{
			return new MinMaxLong(min, max);
		}
	}
}
#endif

