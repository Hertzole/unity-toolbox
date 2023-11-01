#if TOOLBOX_COLLECTIONS
using NUnit.Framework;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using MathRandom = Unity.Mathematics.Random;
using UnityRandom = UnityEngine.Random;
using SystemRandom = System.Random;

namespace Hertzole.UnityToolbox.Tests
{
	public class NativeListExtensions : BaseTest
	{
		[Test]
		public void GetLargest()
		{
			NativeList<int> list = new NativeList<int>(10, Allocator.Temp);

			for (int i = 0; i < 10; i++)
			{
				list.Add(i);
			}

			list.Shuffle();

			int largest = list.GetLargest();
			list.Dispose();
			Assert.AreEqual(9, largest);
		}

		[Test]
		public void GetSmallest()
		{
			NativeList<int> list = new NativeList<int>(10, Allocator.Temp);

			for (int i = 0; i < 10; i++)
			{
				list.Add(i);
			}

			list.Shuffle();

			int smallest = list.GetSmallest();
			list.Dispose();
			Assert.AreEqual(0, smallest);
		}

		[Test]
		public void GetLargest_Burst()
		{
			NativeList<int> list = new NativeList<int>(Allocator.TempJob);

			for (int i = 0; i < 10; i++)
			{
				list.Add(i);
			}

			list.Shuffle();

			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			GetLargestJob job = new GetLargestJob
			{
				list = list,
				result = result
			};

			job.Schedule().Complete();

			list.Dispose();

			int largest = result[0];
			result.Dispose();
			Assert.AreEqual(9, largest);
		}

		[Test]
		public void GetSmallest_Burst()
		{
			NativeList<int> list = new NativeList<int>(10, Allocator.TempJob);

			for (int i = 0; i < 10; i++)
			{
				list.Add(i);
			}

			list.Shuffle();

			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			GetSmallestJob job = new GetSmallestJob
			{
				list = list,
				result = result
			};

			job.Schedule().Complete();

			list.Dispose();

			int smallest = result[0];
			result.Dispose();
			Assert.AreEqual(0, smallest);
		}

		[Test]
		public void Shuffle_UnityRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			list.Shuffle();

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (list[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			list.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_MathRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			MathRandom random = new MathRandom(1234);
			list.Shuffle(ref random);

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (list[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			list.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_SystemRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			SystemRandom random = new SystemRandom(1234);
			list.Shuffle(random);

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (list[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			list.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void Shuffle_Burst()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.TempJob);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			MathRandom random = new MathRandom(1234);
			ShuffleJob job = new ShuffleJob
			{
				list = list,
				random = random
			};

			job.Schedule().Complete();

			bool allEqual = true;

			for (int i = 0; i < 1000; i++)
			{
				if (list[i] != i)
				{
					allEqual = false;
					break;
				}
			}

			list.Dispose();
			Assert.IsFalse(allEqual);
		}

		[Test]
		public void GetRandom_UnityRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			int random = list.GetRandom();
			list.Dispose();
			Assert.GreaterOrEqual(random, 0);
			Assert.Less(random, 1000);
		}

		[Test]
		public void GetRandom_MathRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			MathRandom random = new MathRandom(1234);
			int randomInt = list.GetRandom(ref random);
			list.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void GetRandom_SystemRandom()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.Temp);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			SystemRandom random = new SystemRandom(1234);
			int randomInt = list.GetRandom(random);
			list.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void GetRandom_Burst()
		{
			NativeList<int> list = new NativeList<int>(1000, Allocator.TempJob);
			NativeArray<int> result = new NativeArray<int>(1, Allocator.TempJob);

			for (int i = 0; i < 1000; i++)
			{
				list.Add(i);
			}

			MathRandom random = new MathRandom(1234);
			GetRandomJob job = new GetRandomJob
			{
				list = list,
				random = random,
				result = result
			};

			job.Schedule().Complete();

			int randomInt = job.result[0];
			list.Dispose();
			result.Dispose();
			Assert.GreaterOrEqual(randomInt, 0);
			Assert.Less(randomInt, 1000);
		}

		[Test]
		public void IsNullOrEmpty_NotCreated()
		{
			NativeList<int> list = default;
			Assert.IsTrue(list.IsNullOrEmpty());
		}

		[Test]
		public void IsNullOrEmpty_CreatedButEmpty()
		{
			NativeList<int> list = new NativeList<int>(0, Allocator.Temp);

			bool isNullOrEmpty = list.IsNullOrEmpty();
			list.Dispose();
			Assert.IsTrue(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedAndNotEmpty()
		{
			NativeList<int> list = new NativeList<int>(1, Allocator.Temp);
			list.Add(1);

			bool isNullOrEmpty = list.IsNullOrEmpty();
			list.Dispose();
			Assert.IsFalse(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedButEmpty_Burst()
		{
			NativeList<int> list = new NativeList<int>(0, Allocator.TempJob);
			NativeArray<bool> result = new NativeArray<bool>(1, Allocator.TempJob);

			IsNullOrEmptyJob job = new IsNullOrEmptyJob
			{
				list = list,
				result = result
			};

			job.Schedule().Complete();

			bool isNullOrEmpty = job.result[0];
			list.Dispose();
			result.Dispose();
			Assert.IsTrue(isNullOrEmpty);
		}

		[Test]
		public void IsNullOrEmpty_CreatedAndNotEmpty_Burst()
		{
			NativeList<int> list = new NativeList<int>(1, Allocator.TempJob);
			NativeArray<bool> result = new NativeArray<bool>(1, Allocator.TempJob);

			list.Add(1);

			IsNullOrEmptyJob job = new IsNullOrEmptyJob
			{
				list = list,
				result = result
			};

			job.Schedule().Complete();

			bool isNullOrEmpty = job.result[0];
			list.Dispose();
			result.Dispose();
			Assert.IsFalse(isNullOrEmpty);
		}

		[BurstCompile(CompileSynchronously = true)]
		private struct GetLargestJob : IJob
		{
			public NativeArray<int> result;
			public NativeList<int> list;

			public void Execute()
			{
				int smallest = list.GetLargest();
				result[0] = smallest;
			}
		}

		[BurstCompile(CompileSynchronously = true, Debug = true)]
		private struct GetSmallestJob : IJob
		{
			public NativeArray<int> result;
			public NativeList<int> list;

			public void Execute()
			{
				int smallest = list.GetSmallest();
				result[0] = smallest;
			}
		}

		[BurstCompile(CompileSynchronously = true)]
		private struct ShuffleJob : IJob
		{
			public MathRandom random;
			public NativeList<int> list;

			public void Execute()
			{
				list.Shuffle(ref random);
			}
		}

		[BurstCompile(CompileSynchronously = true)]
		private struct GetRandomJob : IJob
		{
			public MathRandom random;
			public NativeArray<int> result;
			public NativeList<int> list;

			public void Execute()
			{
				int randomInt = list.GetRandom(ref random);
				result[0] = randomInt;
			}
		}

		[BurstCompile(CompileSynchronously = true)]
		private struct IsNullOrEmptyJob : IJob
		{
			public NativeArray<bool> result;
			public NativeList<int> list;

			public void Execute()
			{
				result[0] = list.IsNullOrEmpty();
			}
		}
	}
}
#endif