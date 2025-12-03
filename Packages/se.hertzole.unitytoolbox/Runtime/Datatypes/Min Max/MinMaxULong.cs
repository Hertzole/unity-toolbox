#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxULong : IMinMaxULong, IEquatable<MinMaxULong>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private ulong min;
		[SerializeField]
		[FieldOffset(8)]
		private ulong max;

		/// <inheritdoc />
		public ulong Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public ulong Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out ulong minValue, out ulong maxValue)
		{
			             minValue = min;
            maxValue = max;
		}

		public MinMaxULong(ulong min, ulong max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxULong other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxULong other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (min.GetHashCode() * 397) ^ max.GetHashCode();
			}
		}

		public static bool operator ==(MinMaxULong left, MinMaxULong right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxULong left, MinMaxULong right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxULong((ulong min, ulong max) value)
		{
			return new MinMaxULong(value.min, value.max);
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
			return $"MinMaxULong (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}