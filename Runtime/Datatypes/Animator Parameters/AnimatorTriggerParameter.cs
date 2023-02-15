#if TOOLBOX_ANIMATION
using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	public struct AnimatorTriggerParameter : IEquatable<AnimatorTriggerParameter>, ISerializationCallbackReceiver
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

		public AnimatorTriggerParameter(string name)
		{
			this.name = name;
			hash = Animator.StringToHash(name);
		}

		public static implicit operator int(AnimatorTriggerParameter parameter)
		{
			return parameter.hash;
		}

		public override bool Equals(object obj)
		{
			return obj is AnimatorTriggerParameter other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((name != null ? name.GetHashCode() : 0) * 397) ^ hash;
			}
		}

		public static bool operator ==(AnimatorTriggerParameter left, AnimatorTriggerParameter right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(AnimatorTriggerParameter left, AnimatorTriggerParameter right)
		{
			return !left.Equals(right);
		}

		public bool Equals(AnimatorTriggerParameter other)
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