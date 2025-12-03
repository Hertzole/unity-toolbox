#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxUInt : IMinMaxUInt, IEquatable<MinMaxUInt>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private uint min;
		[SerializeField]
		[FieldOffset(4)]
		private uint max;

		/// <inheritdoc />
		public uint Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public uint Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out uint minValue, out uint maxValue)
		{
			             minValue = min;
            maxValue = max;
		}

		public MinMaxUInt(uint min, uint max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxUInt other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxUInt other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (int) ((min * 397) ^ max);
			}
		}

		public static bool operator ==(MinMaxUInt left, MinMaxUInt right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxUInt left, MinMaxUInt right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxUInt((uint min, uint max) value)
		{
			return new MinMaxUInt(value.min, value.max);
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
			return $"MinMaxUInt (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}