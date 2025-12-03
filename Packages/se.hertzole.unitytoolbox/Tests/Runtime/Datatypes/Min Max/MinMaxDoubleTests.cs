using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxDoubleTests : BaseMinMaxTest<MinMaxDouble, double>
	{
		/// <inheritdoc />
		protected override double CreateRandomValue()
		{
			return Random.Range(-10_000f, 10_000f);
		}

		/// <inheritdoc />
		protected override MinMaxDouble CreateMinMax(double min, double max)
		{
			return new MinMaxDouble(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxDouble FromTuple((double min, double max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (double min, double max) CreateSmallValues()
		{
			return (-10.5, 20.5);
		}

		/// <inheritdoc />
		protected override double GetRandomValue(MinMaxDouble value)
		{
			return value.GetRandomValue();
		}
	}
}