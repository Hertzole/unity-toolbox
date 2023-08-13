using System;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	[Serializable]
	public struct Identifier : IEquatable<Identifier>,
		IEquatable<int>
#if UNITY_EDITOR
		,
		ISerializationCallbackReceiver
#endif
	{
#if UNITY_EDITOR
		[SerializeField]
		private string stringValue;
#endif
		[SerializeField]
		private int value;

		public Identifier(string stringValue)
		{
#if UNITY_EDITOR
			this.stringValue = stringValue;
#endif

			value = ToStableHash(stringValue);
		}

		public override int GetHashCode()
		{
			return value;
		}

		public static bool operator ==(Identifier left, Identifier right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Identifier left, Identifier right)
		{
			return !left.Equals(right);
		}

		private static int ToStableHash(string stringValue)
		{
			if (string.IsNullOrEmpty(stringValue))
			{
				return 0;
			}
			
			unchecked
			{
				int hash = 23;
				for (int i = 0; i < stringValue.Length; i++)
				{
					hash = hash * 31 + stringValue[i];
				}

				return hash;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is Identifier other && Equals(other);
		}

		public bool Equals(Identifier other)
		{
			return value == other.value;
		}

		public bool Equals(int other)
		{
			return value == other;
		}

#if UNITY_EDITOR
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			value = ToStableHash(stringValue);
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Does nothing.
		}
#endif
	}
}