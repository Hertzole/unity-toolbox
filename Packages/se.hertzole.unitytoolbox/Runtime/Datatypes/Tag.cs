using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     Helper type for easily selecting a tag in the inspector.
	/// </summary>
	[Serializable]
	public struct Tag : IEquatable<Tag>, IEquatable<string>
	{
		[SerializeField]
		private string value;

		public override bool Equals(object obj)
		{
			return obj is Tag other && Equals(other);
		}

		public override int GetHashCode()
		{
			return value != null ? value.GetHashCode() : 0;
		}

		public static bool operator ==(Tag left, Tag right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Tag left, Tag right)
		{
			return !left.Equals(right);
		}

		public static implicit operator string(Tag tag)
		{
			return tag.value;
		}

		public static implicit operator Tag(string tag)
		{
			return new Tag { value = tag };
		}

		public bool Equals(string other)
		{
			return value == other;
		}

		public bool Equals(Tag other)
		{
			return value == other.value;
		}
	}
}