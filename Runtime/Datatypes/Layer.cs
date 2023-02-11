using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Helper type for easily selecting a layer in the inspector.
	/// </summary>
	[Serializable]
	public struct Layer : IEquatable<Layer>, IEquatable<int>
	{
		[SerializeField]
		private int value;

		public static implicit operator int(Layer layer)
		{
			return layer.value;
		}

		public static implicit operator Layer(int layer)
		{
			return new Layer { value = layer };
		}

		public override bool Equals(object obj)
		{
			return obj is Layer other && Equals(other);
		}

		public override int GetHashCode()
		{
			return value;
		}

		public static bool operator ==(Layer left, Layer right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Layer left, Layer right)
		{
			return !left.Equals(right);
		}

		public bool Equals(int other)
		{
			return value == other;
		}

		public bool Equals(Layer other)
		{
			return value == other.value;
		}
	}
}