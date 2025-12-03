#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxInt : IMinMaxInt, IEquatable<MinMaxInt>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private int min;
		[SerializeField]
		[FieldOffset(4)]
		private int max;

		/// <inheritdoc />
		public int Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public int Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		public MinMaxInt(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out int minValue, out int maxValue)
		{
            minValue = min;
            maxValue = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxInt other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxInt other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (min * 397) ^ max;
			}
		}

		public static bool operator ==(MinMaxInt left, MinMaxInt right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxInt left, MinMaxInt right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxInt((int min, int max) value)
		{
			return new MinMaxInt(value.min, value.max);
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
			return $"MinMaxInt (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}