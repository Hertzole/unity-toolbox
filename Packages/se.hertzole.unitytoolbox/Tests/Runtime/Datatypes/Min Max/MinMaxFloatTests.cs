using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxFloatTests : BaseMinMaxTest<MinMaxFloat, float>
	{
		/// <inheritdoc />
		protected override float CreateRandomValue()
		{
			return Random.Range(-10_000f, 10_000f);
		}

		/// <inheritdoc />
		protected override MinMaxFloat CreateMinMax(float min, float max)
		{
			return new MinMaxFloat(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxFloat FromTuple((float min, float max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (float min, float max) CreateSmallValues()
		{
			return (-10.5f, 20.5f);
		}

		/// <inheritdoc />
		protected override float GetRandomValue(MinMaxFloat value)
		{
			return value.GetRandomValue();
		}
	}
}