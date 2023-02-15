#if TOOLBOX_ANIMATION
using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	public struct AnimatorFloatParameter : IEquatable<AnimatorFloatParameter>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private string name;
		[SerializeField]
		private int hash;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				hash = Animator.StringToHash(name);
			}
		}

		public int Hash { get { return hash; } }

		public AnimatorFloatParameter(string name)
		{
			this.name = name;
			hash = Animator.StringToHash(name);
		}

		public static implicit operator int(AnimatorFloatParameter parameter)
		{
			return parameter.hash;
		}

		public override bool Equals(object obj)
		{
			return obj is AnimatorFloatParameter other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((name != null ? name.GetHashCode() : 0) * 397) ^ hash;
			}
		}

		public static bool operator ==(AnimatorFloatParameter left, AnimatorFloatParameter right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AnimatorFloatParameter left, AnimatorFloatParameter right)
		{
			return !left.Equals(right);
		}

		public bool Equals(AnimatorFloatParameter other)
		{
			return hash == other.hash && name == other.name;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			// Does nothing.

		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			hash = Animator.StringToHash(name);
		}
	}
}
#endif