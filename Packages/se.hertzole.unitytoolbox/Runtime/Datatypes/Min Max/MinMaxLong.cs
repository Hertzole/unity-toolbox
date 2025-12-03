#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxLong : IMinMaxLong, IEquatable<MinMaxLong>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private long min;
		[SerializeField]
		[FieldOffset(8)]
		private long max;

		/// <inheritdoc />
		public long Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public long Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out long minValue, out long maxValue)
		{
			             minValue = min;
            maxValue = max;
		}

		public MinMaxLong(long min, long max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxLong other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxLong other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (min.GetHashCode() * 397) ^ max.GetHashCode();
			}
		}

		public static bool operator ==(MinMaxLong left, MinMaxLong right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxLong left, MinMaxLong right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxLong((long min, long max) value)
		{
			return new MinMaxLong(value.min, value.max);
		}


		/// <inheritdoc />
		public readonly override string ToString()
		{
			return ToString(null);
		}

		/// <inheritdoc />
		public readonly string ToString(string? format, IFormatProvider? formatProvider = null)
		{
			if (string.IsNullOrEmpty(format))
			{
				format = "G";
			}

			formatProvider ??= CultureInfo.CurrentCulture;
			return $"MinMaxLong (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}