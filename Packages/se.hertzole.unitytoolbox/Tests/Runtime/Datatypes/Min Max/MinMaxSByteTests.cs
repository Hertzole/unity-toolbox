using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxSByteTests : BaseMinMaxTest<MinMaxSByte, sbyte>
	{
		/// <inheritdoc />
		protected override sbyte CreateRandomValue()
		{
			return (sbyte) Random.Range(sbyte.MinValue, sbyte.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxSByte CreateMinMax(sbyte min, sbyte max)
		{
			return new MinMaxSByte(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxSByte FromTuple((sbyte min, sbyte max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (sbyte min, sbyte max) CreateSmallValues()
		{
			return (-10, 20);
		}

		/// <inheritdoc />
		protected override sbyte GetRandomValue(MinMaxSByte value)
		{
			return value.GetRandomValue();
		}
	}
}