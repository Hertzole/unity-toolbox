#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxSByte : IMinMaxSByte, IEquatable<MinMaxSByte>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private sbyte min;
		[SerializeField]
		[FieldOffset(1)]
		private sbyte max;

		/// <inheritdoc />
		public sbyte Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public sbyte Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		public MinMaxSByte(sbyte min, sbyte max)
		{
			this.min = min;
			this.max = max;
		}

		public readonly void Deconstruct(out sbyte minValue, out sbyte maxValue)
		{
			minValue = min;
			maxValue = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxSByte other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxSByte other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			return (min << 8) | (byte) max;
		}

		public static bool operator ==(MinMaxSByte left, MinMaxSByte right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxSByte left, MinMaxSByte right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxSByte((sbyte min, sbyte max) value)
		{
			return new MinMaxSByte(value.min, value.max);
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
			return $"MinMaxSByte (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}