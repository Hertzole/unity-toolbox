#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxUShort : IMinMaxUShort, IEquatable<MinMaxUShort>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private ushort min;
		[SerializeField]
		[FieldOffset(2)]
		private ushort max;

		/// <inheritdoc />
		public ushort Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public ushort Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		public MinMaxUShort(ushort min, ushort max)
		{
			this.min = min;
			this.max = max;
		}
		
		public readonly void Deconstruct(out ushort minValue, out ushort maxValue)
        {
            minValue = min;
            maxValue = max;
        }

		/// <inheritdoc />
		public readonly bool Equals(MinMaxUShort other)
		{
			return min == other.min && max == other.max;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxUShort other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			return (min << 16) | max;
		}

		public static bool operator ==(MinMaxUShort left, MinMaxUShort right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxUShort left, MinMaxUShort right)
		{
			return !left.Equals(right);
		}
		
		public static implicit operator MinMaxUShort((ushort min, ushort max) value)
        {
            return new MinMaxUShort(value.min, value.max);
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
			return $"MinMaxUShort (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}