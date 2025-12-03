#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxShort : IMinMaxShort, IEquatable<MinMaxShort>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private short min;
		[SerializeField]
		[FieldOffset(2)]
		private short max;

		/// <inheritdoc />
		public short Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public short Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		public MinMaxShort(short min, short max)
		{
			this.min = min;
			this.max = max;
		}
		
		public readonly void Deconstruct(out short minValue, out short maxValue)
        {
            minValue = min;
            maxValue = max;
        }

		/// <inheritdoc />
		public readonly bool Equals(MinMaxShort other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxShort other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			return (min << 16) | (ushort) max;
		}

		public static bool operator ==(MinMaxShort left, MinMaxShort right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxShort left, MinMaxShort right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxShort((short min, short max) value)
		{
			return new MinMaxShort(value.min, value.max);
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
			return $"MinMaxShort (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}