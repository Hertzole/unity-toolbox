using NUnit.Framework;

namespace Hertzole.UnityToolbox.Tests
{
	public class CircularObjectPoolTests : BaseTest
	{
		[Test]
		public void Get_ReturnsObject()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj = pool.Get();
			Assert.IsTrue(obj.IsActive);
			Assert.AreEqual(pool.CountAll, 1);
			Assert.AreEqual(pool.CountActive, 1);
			Assert.AreEqual(pool.CountInactive, 0);
		}

		[Test]
		public void Get_ReturnsDifferentObjects()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj1 = pool.Get();
			PoolObject obj2 = pool.Get();
			Assert.AreNotSame(obj1, obj2);
			Assert.IsTrue(obj1.IsActive);
			Assert.IsTrue(obj2.IsActive);
			Assert.AreEqual(pool.CountAll, 2);
			Assert.AreEqual(pool.CountActive, 2);
			Assert.AreEqual(pool.CountInactive, 0);
		}

		[Test]
		public void Get_ReturnsSameObjects()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj1 = pool.Get();
			pool.Release(obj1);
			PoolObject obj2 = pool.Get();
			Assert.AreSame(obj1, obj2);
			Assert.IsTrue(obj1.IsActive);
			Assert.IsTrue(obj2.IsActive);
			Assert.AreEqual(pool.CountAll, 1);
			Assert.AreEqual(pool.CountActive, 1);
			Assert.AreEqual(pool.CountInactive, 0);
		}

		[Test]
		public void Release_ReturnsObjectToPool()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj = pool.Get();
			pool.Release(obj);
			Assert.IsFalse(obj.IsActive);
			Assert.AreEqual(pool.CountAll, 1);
			Assert.AreEqual(pool.CountActive, 0);
			Assert.AreEqual(pool.CountInactive, 1);
		}

		[Test]
		public void Release_ReturnsObjectToPool_WhenObjectIsNotFromPool()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj = new PoolObject
			{
				IsActive = true
			};

			pool.Release(obj);
			Assert.IsTrue(obj.IsActive);
			Assert.AreEqual(pool.CountAll, 0);
			Assert.AreEqual(pool.CountActive, 0);
			Assert.AreEqual(pool.CountInactive, 0);
		}

		[Test]
		public void Get_Circular()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(1);
			PoolObject obj1 = pool.Get();
			PoolObject obj2 = pool.Get();
			Assert.AreSame(obj1, obj2);
			Assert.IsTrue(obj1.IsActive);
			Assert.IsTrue(obj2.IsActive);
			Assert.AreEqual(pool.CountAll, 1);
			Assert.AreEqual(pool.CountActive, 1);
			Assert.AreEqual(pool.CountInactive, 0);
		}

		[Test]
		public void Get_DifferentAfterClear()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(1);
			PoolObject obj1 = pool.Get();
			pool.Clear();
			PoolObject obj2 = pool.Get();
			Assert.AreNotSame(obj1, obj2);
			Assert.AreEqual(pool.CountAll, 1);
			Assert.AreEqual(pool.CountActive, 1);
			Assert.AreEqual(pool.CountInactive, 0);
		}
		
		[Test]
		public void ReleaseAll_ReleasesAllObjects()
		{
			CircularObjectPool<PoolObject> pool = CreatePool(10);
			PoolObject obj1 = pool.Get();
			PoolObject obj2 = pool.Get();
			pool.ReleaseAll();
			Assert.IsFalse(obj1.IsActive);
			Assert.IsFalse(obj2.IsActive);
			Assert.AreEqual(pool.CountAll, 2);
			Assert.AreEqual(pool.CountActive, 0);
			Assert.AreEqual(pool.CountInactive, 2);
		}

		private static CircularObjectPool<PoolObject> CreatePool(int maxCount)
		{
			return new CircularObjectPool<PoolObject>(maxCount, () => new PoolObject(), o => o.IsActive = true, o => o.IsActive = false);
		}

		public class PoolObject
		{
			public bool IsActive { get; set; }
		}
	}
}