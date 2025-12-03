#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxDouble : IMinMaxDouble, IEquatable<MinMaxDouble>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private double min;
		[SerializeField]
		[FieldOffset(8)]
		private double max;

		/// <inheritdoc />
		public double Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public double Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		private const double TOLERANCE = 0.000001;

		public MinMaxDouble(double min, double max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out double minValue, out double maxValue)
		{
			minValue = min;
			maxValue = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxDouble other)
		{
			return Math.Abs(min - other.min) < TOLERANCE && Math.Abs(max - other.max) < TOLERANCE;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxDouble other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (min.GetHashCode() * 397) ^ max.GetHashCode();
			}
		}

		public static bool operator ==(MinMaxDouble left, MinMaxDouble right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxDouble left, MinMaxDouble right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxDouble((double min, double max) value)
		{
			return new MinMaxDouble(value.min, value.max);
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
			return $"MinMaxDouble (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}