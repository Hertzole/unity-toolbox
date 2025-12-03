#if TOOLBOX_UXML_ATTRIBUTES
using System;
using System.Globalization;

namespace Hertzole.UnityToolbox.Editor
{
	public sealed class MinMaxSByteUxmlConverter : MinMaxValueUxmlConverter<MinMaxSByte, sbyte>
	{
		protected override sbyte ParseValue(ReadOnlySpan<char> span)
		{
			return sbyte.Parse(span, NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		protected override MinMaxSByte CreateValue(sbyte min, sbyte max)
		{
			return new MinMaxSByte(min, max);
		}
	}
}
#endif

