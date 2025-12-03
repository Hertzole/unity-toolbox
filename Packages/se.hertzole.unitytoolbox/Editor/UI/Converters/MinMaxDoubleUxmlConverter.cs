#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxDoubleUxmlConverter : MinMaxValueUxmlConverter<MinMaxDouble, double>
	{
		protected override double ParseValue(ReadOnlySpan<char> span)
		{
			return double.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxDouble CreateValue(double min, double max)
		{
			return new MinMaxDouble(min, max);
		}
	}
}
#endif

