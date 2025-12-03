#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxIntUxmlConverter : MinMaxValueUxmlConverter<MinMaxInt, int>
	{
		protected override int ParseValue(ReadOnlySpan<char> span)
		{
			return int.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxInt CreateValue(int min, int max)
		{
			return new MinMaxInt(min, max);
		}
	}
}
#endif