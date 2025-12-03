using UnityEngine;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxUShortTests : BaseMinMaxTest<MinMaxUShort, ushort>
	{
		/// <inheritdoc />
		protected override ushort CreateRandomValue()
		{
			return (ushort) Random.Range(ushort.MinValue, ushort.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxUShort CreateMinMax(ushort min, ushort max)
		{
			return new MinMaxUShort(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxUShort FromTuple((ushort min, ushort max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (ushort min, ushort max) CreateSmallValues()
		{
			return (10, 20);
		}

		/// <inheritdoc />
		protected override ushort GetRandomValue(MinMaxUShort value)
		{
			return value.GetRandomValue();
		}
	}
}