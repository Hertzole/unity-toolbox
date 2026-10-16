using System;
using System.Text;
using NUnit.Framework;

namespace Hertzole.UnityToolbox.Tests
{
    public class IdentifierTests
    {
        private const string ID = "Identify_Me";

        [Test]
        public void Constructor_Equals()
        {
            // Arrange
            Identifier id1 = new Identifier(ID);
            Identifier id2 = new Identifier(ID.AsSpan());

            // Assert
            AssertEquals(id1, id2);
            AssertHash(id1, ID);
            AssertHash(id2, ID);
        }

        [Test]
        public void Constructor_String_Empty_IsZero()
        {
            // Arrange
            Identifier id = new Identifier(string.Empty);

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void Constructor_String_Null_IsZero()
        {
            // Arrange
            Identifier id = new Identifier(null);

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
#if UNITY_EDITOR
            Assert.That(id.StringValue, Is.EqualTo(string.Empty));
#endif
        }

        [Test]
        public void Constructor_Span_Empty_IsZero()
        {
            //  Arrange
            Identifier id = new Identifier(ReadOnlySpan<char>.Empty);

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void Operator_Equals()
        {
            // Arrange
            Identifier id1 = ID;
            Identifier id2 = ID.AsSpan();

            // Assert
            AssertEquals(id1, id2);
            AssertHash(id1, ID);
            AssertHash(id2, ID);
        }

        [Test]
        public void Operator_String_Empty_IsZero()
        {
            // Arrange
            Identifier id = new Identifier(string.Empty);

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void Operator_String_Null_IsZero()
        {
            // Arrange
            Identifier id = new Identifier(null);

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
#if UNITY_EDITOR
            Assert.That(id.StringValue, Is.EqualTo(string.Empty));
#endif
        }

        [Test]
        public void Operator_Span_Empty_IsZero()
        {
            // Arrange
            Identifier id = ReadOnlySpan<char>.Empty;

            // Assert
            Assert.That(id.GetHashCode(), Is.EqualTo(0));
        }

        [Test]
        public void FromUtf8_Equals()
        {
            // Arrange
            Identifier a = new Identifier(ID);
            Identifier b = Identifier.FromUtf8(Encoding.UTF8.GetBytes(ID));

            AssertEquals(a, b);
            AssertHash(a, ID);
            AssertHash(b, ID);
        }

        private static void AssertEquals(Identifier a, Identifier b)
        {
            Assert.That(a.Equals(b), Is.True, "Identifiers should be equal.");
            Assert.That(a.Equals((object) b), Is.True, "Identifiers should be equal.");
            Assert.That(a == b, Is.True, "Identifiers should be equal.");
            Assert.That(a != b, Is.False);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()), "Hashcodes should be equal.");
#if UNITY_EDITOR
            Assert.That(a.StringValue, Is.EqualTo(b.StringValue), "String values should be equal.");
#endif
        }

        private static void AssertHash(Identifier id, string stringValue)
        {
            int hash = Identifier.ToStableHash(stringValue);
            Assert.That(id.GetHashCode(), Is.EqualTo(hash));
            Assert.That(id.Equals(hash), Is.True);
        }
    }
}