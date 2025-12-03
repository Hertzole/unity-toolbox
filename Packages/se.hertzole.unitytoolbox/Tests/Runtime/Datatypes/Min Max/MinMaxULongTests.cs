using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxULongTests : BaseMinMaxTest<MinMaxULong, ulong>
	{
		/// <inheritdoc />
		protected override ulong CreateRandomValue()
		{
			return (ulong) Random.Range(0, int.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxULong CreateMinMax(ulong min, ulong max)
		{
			return new MinMaxULong(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxULong FromTuple((ulong min, ulong max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (ulong min, ulong max) CreateSmallValues()
		{
			return (10ul, 20ul);
		}

		/// <inheritdoc />
		protected override ulong GetRandomValue(MinMaxULong value)
		{
			return value.GetRandomValue();
		}
	}
}