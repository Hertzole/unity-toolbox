#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxUIntUxmlConverter : MinMaxValueUxmlConverter<MinMaxUInt, uint>
	{
		protected override uint ParseValue(ReadOnlySpan<char> span)
		{
			return uint.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxUInt CreateValue(uint min, uint max)
		{
			return new MinMaxUInt(min, max);
		}
	}
}
#endif

