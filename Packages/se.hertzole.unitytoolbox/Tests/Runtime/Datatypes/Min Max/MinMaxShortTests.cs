using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxShortTests : BaseMinMaxTest<MinMaxShort, short>
	{
		/// <inheritdoc />
		protected override short CreateRandomValue()
		{
			return (short) Random.Range(short.MinValue, short.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxShort CreateMinMax(short min, short max)
		{
			return new MinMaxShort(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxShort FromTuple((short min, short max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (short min, short max) CreateSmallValues()
		{
			return (-10, 20);
		}

		/// <inheritdoc />
		protected override short GetRandomValue(MinMaxShort value)
		{
			return value.GetRandomValue();
		}
	}
}