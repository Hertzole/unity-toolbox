#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxByte : IMinMaxByte, IEquatable<MinMaxByte>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private byte min;
		[SerializeField]
		[FieldOffset(1)]
		private byte max;

		/// <inheritdoc />
		public byte Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public byte Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		public MinMaxByte(byte min, byte max)
		{
			this.min = min;
			this.max = max;
		}

		public readonly void Deconstruct(out byte minValue, out byte maxValue)
		{
			minValue = min;
			maxValue = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxByte other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxByte other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			return (min << 8) | max;
		}

		public static bool operator ==(MinMaxByte left, MinMaxByte right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxByte left, MinMaxByte right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxByte((byte min, byte max) value)
		{
			return new MinMaxByte(value.min, value.max);
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

			return $"MinMaxByte (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}