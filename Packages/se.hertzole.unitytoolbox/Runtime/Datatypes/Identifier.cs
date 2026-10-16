#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace Hertzole.UnityToolbox
{
    /// <summary>
    ///     A unique identifier that can be serialized and used in the inspector. It hashes a string to an <see cref="int" />
    ///     for faster checks in lists and dictionaries. The string is removed in builds to save memory.
    /// </summary>
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
        ///     Gets the editor only string value of this identifier. This cannot be used at runtime.
        /// </summary>
        /// <value>The editor only string value of this identifier.</value>
        public string StringValue
        {
            get { return stringValue; }
        }
#endif

        /// <summary>
        ///     Creates a new identifier with a hash from the provided string value.
        /// </summary>
        /// <param name="stringValue">The string to hash.</param>
        public Identifier(string? stringValue)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("Identifier capture string (Editor only)");
            this.stringValue = stringValue ?? string.Empty;
            Profiler.EndSample();
#endif

            value = ToStableHash(stringValue);
        }

        /// <summary>
        ///     Creates a new identifier with a hash from the provided span value.
        /// </summary>
        /// <param name="value">The span to hash.</param>
        public Identifier(ReadOnlySpan<char> value)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("Identifier capture string (Editor only)");
            stringValue = value.IsEmpty ? string.Empty : value.ToString();
            Profiler.EndSample();
#endif

            this.value = ToStableHash(value);
        }

        private Identifier(ReadOnlySpan<byte> utf8Value)
        {
            if (utf8Value.IsEmpty)
            {
                value = 0;
#if UNITY_EDITOR
                Profiler.BeginSample("Identifier capture string (Editor only)");
                stringValue = string.Empty;
                Profiler.EndSample();
#endif
            }
            else
            {
                int chars = Encoding.UTF8.GetCharCount(utf8Value);
                Span<char> charsSpan = stackalloc char[chars];
                Encoding.UTF8.GetChars(utf8Value, charsSpan);
                value = ToStableHash(charsSpan);
#if UNITY_EDITOR
                Profiler.BeginSample("Identifier capture string (Editor only)");
                stringValue = charsSpan.ToString();
                Profiler.EndSample();
#endif
            }
        }

        /// <summary>
        ///     Creates a new identifier with a hash from the provided byte span. The byte span should ideally represent a UTF-8
        ///     string.
        /// </summary>
        /// <param name="utf8Value">The byte span to hash.</param>
        /// <returns>A new <see cref="Identifier" /> created from <paramref name="utf8Value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Identifier FromUtf8(ReadOnlySpan<byte> utf8Value)
        {
            return new Identifier(utf8Value);
        }

        /// <summary>
        ///     Hashes a string value into a stable hash. The hash is guaranteed to be the same for the same string value every
        ///     time.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>The hash value.</returns>
        public static int ToStableHash(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty)
            {
                return 0;
            }

            unchecked
            {
                int hash = 23;
                for (int i = 0; i < value.Length; i++)
                {
                    hash = hash * 31 + value[i];
                }

                return hash;
            }
        }

        /// <summary>
        ///     Returns a debug representation of the identifier.
        /// </summary>
        /// <returns>A debug representation of the identifier.</returns>
        public override string ToString()
        {
            return
#if UNITY_EDITOR
                $"Identifier ({value}, editor string: {stringValue})";
#else
				$"Identifier ({value})";
#endif
        }

        /// <summary>
        ///     Determines whether this instance is equal to another <see cref="Identifier" />.
        /// </summary>
        /// <param name="other">The <see cref="Identifier" /> to compare with.</param>
        /// <returns>
        ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise
        ///     <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Identifier other)
        {
            return value == other.value;
        }

        /// <summary>
        ///     Determines whether this instance is equal to the specified integer.
        /// </summary>
        /// <param name="other">The integer to compare with.</param>
        /// <returns>
        ///     <see langword="true" /> if this instance is equal to <paramref name="other" />; otherwise
        ///     <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(int other)
        {
            return value == other;
        }

        /// <summary>
        ///     Determines whether this instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="obj" /> is an <see cref="Identifier" /> equal to this instance;
        ///     otherwise <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is Identifier other && Equals(other);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return value;
        }

        /// <summary>
        ///     Determines whether two <see cref="Identifier" /> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Identifier" /> to compare.</param>
        /// <param name="right">The second <see cref="Identifier" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="left" /> is equal to <paramref name="right" />; otherwise
        ///     <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Identifier left, Identifier right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two <see cref="Identifier" /> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Identifier" /> to compare.</param>
        /// <param name="right">The second <see cref="Identifier" /> to compare.</param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="left" /> is not equal to <paramref name="right" />; otherwise
        ///     <see langword="false" />.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Identifier left, Identifier right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Implicitly converts a string to an <see cref="Identifier" />.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>An <see cref="Identifier" /> created from <paramref name="value" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Identifier(string value)
        {
            return new Identifier(value);
        }

        /// <summary>
        ///     Implicitly converts a read-only span of characters to an <see cref="Identifier" />.
        /// </summary>
        /// <param name="value">The span to convert.</param>
        /// <returns>An <see cref="Identifier" /> created from <paramref name="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Identifier(ReadOnlySpan<char> value)
        {
            return new Identifier(value);
        }

#if UNITY_EDITOR
        /// <summary>
        ///     Called before serialization. Recomputes the hash from the stored string value.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            value = ToStableHash(stringValue);
        }

        /// <summary>
        ///     Called after deserialization. Does nothing.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Does nothing.
        }
#endif
    }
}