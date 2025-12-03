using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxIntTests : BaseMinMaxTest<MinMaxInt, int>
	{
		/// <inheritdoc />
		protected override int CreateRandomValue()
		{
			return Random.Range(int.MinValue, int.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxInt CreateMinMax(int min, int max)
		{
			return new MinMaxInt(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxInt FromTuple((int min, int max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (int min, int max) CreateSmallValues()
		{
			return (-10, 20);
		}

		/// <inheritdoc />
		protected override int GetRandomValue(MinMaxInt value)
		{
			return value.GetRandomValue();
		}
	}
}