#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxFloatUxmlConverter : MinMaxValueUxmlConverter<MinMaxFloat, float>
	{
		protected override float ParseValue(ReadOnlySpan<char> span)
		{
			return float.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxFloat CreateValue(float min, float max)
		{
			return new MinMaxFloat(min, max);
		}
	}
}
#endif

