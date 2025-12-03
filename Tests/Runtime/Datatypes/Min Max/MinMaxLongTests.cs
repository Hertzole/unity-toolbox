using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxLongTests : BaseMinMaxTest<MinMaxLong, long>
	{
		/// <inheritdoc />
		protected override long CreateRandomValue()
		{
			return Random.Range(int.MinValue, int.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxLong CreateMinMax(long min, long max)
		{
			return new MinMaxLong(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxLong FromTuple((long min, long max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (long min, long max) CreateSmallValues()
		{
			return (-10L, 20L);
		}

		/// <inheritdoc />
		protected override long GetRandomValue(MinMaxLong value)
		{
			return value.GetRandomValue();
		}
	}
}