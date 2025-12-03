using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public static class MinMaxExtensions
	{
		public static byte GetRandomValue(this MinMaxByte minMax)
		{
			return (byte) Random.Range(minMax.Min, minMax.Max);
		}

		public static sbyte GetRandomValue(this MinMaxSByte minMax)
		{
			return (sbyte) Random.Range(minMax.Min, minMax.Max);
		}

		public static short GetRandomValue(this MinMaxShort minMax)
		{
			return (short) Random.Range(minMax.Min, minMax.Max);
		}

		public static ushort GetRandomValue(this MinMaxUShort minMax)
		{
			return (ushort) Random.Range(minMax.Min, minMax.Max);
		}

		public static int GetRandomValue(this MinMaxInt minMax)
		{
			return Random.Range(minMax.Min, minMax.Max);
		}

		public static uint GetRandomValue(this MinMaxUInt minMax)
		{
			return (uint) Random.Range((int) minMax.Min, (int) minMax.Max);
		}

		public static long GetRandomValue(this MinMaxLong minMax)
		{
			ulong range = (ulong) (minMax.Max - minMax.Min);
			ulong randomOffset = (ulong) Random.Range(0, (int) range);
			return minMax.Min + (long) randomOffset;
		}

		public static ulong GetRandomValue(this MinMaxULong minMax)
		{
			ulong range = minMax.Max - minMax.Min;
			ulong randomOffset = (ulong) Random.Range(0, (int) range);
			return minMax.Min + randomOffset;
		}

		public static float GetRandomValue(this MinMaxFloat minMax)
		{
			return Random.Range(minMax.Min, minMax.Max);
		}

		public static double GetRandomValue(this MinMaxDouble minMax)
		{
			float randomValue = Random.Range((float) minMax.Min, (float) minMax.Max);
			return randomValue;
		}
	}
}