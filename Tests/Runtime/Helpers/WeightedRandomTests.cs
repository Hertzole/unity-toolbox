using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Collections;

namespace Hertzole.UnityToolbox.Tests
{
    public partial class WeightedRandomTests
    {
        private static int[] TestData
        {
            get { return new[] { 1, 2, 3 }; }
        }
        private const int LOOPS = 100_000;

        [Test]
        public void GetRandomIndex_UnityRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex(ints, i => i));
        }

        [Test]
        public void GetRandomIndex_GenericEnumerable_UnityRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex<int, NativeArray<int>>(ints, i => i));
        }

        [Test]
        public void GetRandomIndex_IEnumerable_UnityRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex((IEnumerable<int>) ints, i => i));
        }

        [Test]
        public void GetRandomIndex_SystemRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random();

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex(ints, i => i, random));
        }

        [Test]
        public void GetRandomIndex_GenericEnumerable_SystemRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);
            Random random = new Random();

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex<int, NativeArray<int>>(ints, i => i, random));
        }

        [Test]
        public void GetRandomIndex_IEnumerable_SystemRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random();

            // Act
            RunIndexTest(list, ints => WeightedRandom.GetRandomIndex((IEnumerable<int>) ints, i => i, random));
        }

        [Test]
        public void GetRandom_UnityRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom(ints, i => i));
        }

        [Test]
        public void GetRandom_GenericEnumerable_UnityRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom<int, NativeArray<int>>(ints, i => i));
        }

        [Test]
        public void GetRandom_IEnumerable_UnityRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom((IEnumerable<int>) ints, i => i));
        }

        [Test]
        public void GetRandom_SystemRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random();

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom(ints, i => i, random));
        }

        [Test]
        public void GetRandom_GenericEnumerable_SystemRandom()
        {
            // Arrange
            using NativeArray<int> list = new NativeArray<int>(TestData, Allocator.Temp);
            Random random = new Random();

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom<int, NativeArray<int>>(ints, i => i, random));
        }

        [Test]
        public void GetRandom_IEnumerable_SystemRandom()
        {
            // Arrange
            List<int> list = new List<int>(TestData);
            Random random = new Random();

            // Act
            RunRandomTest(list, ints => WeightedRandom.GetRandom((IEnumerable<int>) ints, i => i, random));
        }

        [Test]
        public void GetRandomIndex_Empty_ReturnsNegativeIndex()
        {
            // Act
            int index = WeightedRandom.GetRandomIndex(new List<int>(), i => i);

            // Assert
            Assert.That(index, Is.EqualTo(-1));
        }

        private static void RunIndexTest<TContainer>(TContainer container, Func<TContainer, int> getRandomIndex)
        {
            // Arrange
            List<int> hits = new List<int> { 0, 0, 0 };

            // Act
            for (int i = 0; i < LOOPS; i++)
            {
                int index = getRandomIndex(container);
                hits[index]++;
            }

            // Assert

            // Make sure items at index 0 is hit less than items at index 1 and 2
            Assert.That(hits[0], Is.LessThan(hits[1]));
            Assert.That(hits[0], Is.LessThan(hits[2]));

            // Make sure items at index 1 is hit more than items at index 0 but less than items at index 2
            Assert.That(hits[1], Is.GreaterThan(hits[0]));
            Assert.That(hits[1], Is.LessThan(hits[2]));

            // Make sure items at index 2 is hit more than items at index 0 and 2
            Assert.That(hits[2], Is.GreaterThan(hits[0]));
            Assert.That(hits[2], Is.GreaterThan(hits[1]));
        }

        private static void RunRandomTest<TContainer>(TContainer container, Func<TContainer, int> getRandom)
        {
            // Arrange
            Dictionary<int, int> hits = new Dictionary<int, int>();

            // Act
            for (int i = 0; i < LOOPS; i++)
            {
                int index = getRandom(container);
                hits.TryAdd(index, 0);
                hits[index]++;
            }

            // Assert

            // Make sure items at key 1 is hit less than items at key 2 and 3
            Assert.That(hits[1], Is.LessThan(hits[2]));
            Assert.That(hits[1], Is.LessThan(hits[3]));

            // Make sure items at key 2 is hit more than items at key 1 but less than items at key 3
            Assert.That(hits[2], Is.GreaterThan(hits[1]));
            Assert.That(hits[2], Is.LessThan(hits[3]));

            // Make sure items at key 3 is hit more than items at key 1 and 2
            Assert.That(hits[3], Is.GreaterThan(hits[1]));
            Assert.That(hits[3], Is.GreaterThan(hits[2]));
        }
    }
}