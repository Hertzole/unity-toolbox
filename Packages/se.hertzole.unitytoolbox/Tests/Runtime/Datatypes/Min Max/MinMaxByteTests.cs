using Random = UnityEngine.Random;

namespace Hertzole.UnityToolbox.Tests
{
	public class MinMaxByteTests : BaseMinMaxTest<MinMaxByte, byte>
	{
		/// <inheritdoc />
		protected override byte CreateRandomValue()
		{
			return (byte) Random.Range(byte.MinValue, byte.MaxValue);
		}

		/// <inheritdoc />
		protected override MinMaxByte CreateMinMax(byte min, byte max)
		{
			return new MinMaxByte(min, max);
		}

		/// <inheritdoc />
		protected override MinMaxByte FromTuple((byte min, byte max) value)
		{
			return value;
		}

		/// <inheritdoc />
		protected override (byte min, byte max) CreateSmallValues()
		{
			return (10, 20);
		}

		/// <inheritdoc />
		protected override byte GetRandomValue(MinMaxByte value)
		{
			return value.GetRandomValue();
		}
	}
}