#nullable enable

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct MinMaxFloat : IMinMaxFloat, IEquatable<MinMaxFloat>, IFormattable
	{
		[SerializeField]
		[FieldOffset(0)]
		private float min;
		[SerializeField]
		[FieldOffset(4)]
		private float max;

		/// <inheritdoc />
		public float Min
		{
			readonly get { return min; }
			set { min = value; }
		}
		/// <inheritdoc />
		public float Max
		{
			readonly get { return max; }
			set { max = value; }
		}

		/// <inheritdoc />
		public readonly void Deconstruct(out float minValue, out float maxValue)
		{
			             minValue = min;
            maxValue = max;
		}

		private const float TOLERANCE = 0.0001f;

		public MinMaxFloat(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		/// <inheritdoc />
		public readonly bool Equals(MinMaxFloat other)
		{
			return Math.Abs(min - other.min) < TOLERANCE && Math.Abs(max - other.max) < TOLERANCE;
		}

		/// <inheritdoc />
		public readonly override bool Equals(object? obj)
		{
			return obj is MinMaxFloat other && Equals(other);
		}

		/// <inheritdoc />
		public readonly override int GetHashCode()
		{
			unchecked
			{
				return (min.GetHashCode() * 397) ^ max.GetHashCode();
			}
		}

		public static bool operator ==(MinMaxFloat left, MinMaxFloat right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(MinMaxFloat left, MinMaxFloat right)
		{
			return !left.Equals(right);
		}

		public static implicit operator MinMaxFloat((float min, float max) value)
		{
			return new MinMaxFloat(value.min, value.max);
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
			return $"MinMaxFloat (Min: {min.ToString(format, formatProvider)}, Max: {max.ToString(format, formatProvider)})";
		}
	}
}