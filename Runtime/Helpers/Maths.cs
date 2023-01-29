using System.Runtime.CompilerServices;

namespace Hertzole.UnityToolbox
{
	public static class Maths
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RollOver(int value, int max)
		{
			if (value < 0)
			{
				return max - 1;
			}

			if (value >= max)
			{
				return 0;
			}

			return value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RollOver(int value, int min, int max)
		{
			if (value < min)
			{
				return max - 1;
			}

			if (value >= max)
			{
				return min;
			}

			return value;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Percent(float value, float max)
		{
			return value / max;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Percent(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}
	}
}