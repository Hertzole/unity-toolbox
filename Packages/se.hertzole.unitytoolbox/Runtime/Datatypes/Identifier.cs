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

#if UNITY_EDITOR
        /// <summary>
        ///     The EDITOR ONLY string value of this identifier. This CAN NOT be used at runtime.
        /// </summary>
        public string StringValue
        {
            get { return stringValue; }
        }
#endif

        public Identifier(string stringValue)
        {
#if UNITY_EDITOR
            this.stringValue = stringValue;
#endif

            value = ToStableHash(stringValue);
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

        /// <inheritdoc />
        public override string ToString()
        {
            return
#if UNITY_EDITOR
                $"Identifier ({value}, editor string: {stringValue})";
#else
				$"Identifier ({value})";
#endif
        }

        public bool Equals(Identifier other)
        {
            return value == other.value;
        }

        public bool Equals(int other)
        {
            return value == other;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier other && Equals(other);
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