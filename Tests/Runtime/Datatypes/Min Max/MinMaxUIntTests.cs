using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxUIntTests : BaseMinMaxTest<MinMaxUInt, uint>
	{
		/// <inheritdoc />
		protected override uint CreateRandomValue()
		{
			return (uint) Random.Range(0, int.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxUInt CreateMinMax(uint min, uint max)
		{
			return new MinMaxUInt(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxUInt FromTuple((uint min, uint max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (uint min, uint max) CreateSmallValues()
		{
			return (10u, 20u);
		}

		/// <inheritdoc />
		protected override uint GetRandomValue(MinMaxUInt value)
		{
			return value.GetRandomValue();
		}
	}
}