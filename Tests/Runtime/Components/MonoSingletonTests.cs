using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.UnityToolbox.Tests
{
	[TestFixture]
	public class MonoSingletonTests : BaseTest
	{
		[UnityTest]
		public IEnumerator DestroyNewestComponent()
		{
			TestSingleton prefab = CreateGameObject<TestSingleton>("Prefab");
			prefab.DestroyStrategy = SingletonDestroyStrategy.DestroyNewestComponent;

			Assert.IsNotNull(prefab);
			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);

			yield return null;

			TestSingleton newSingleton = CreatePrefab(prefab);
			GameObject newSingletonGameObject = newSingleton.gameObject;

			yield return null;

			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);
			Assert.IsNotNull(newSingletonGameObject);
			Assert.IsNull(newSingleton);
		}

		[UnityTest]
		public IEnumerator DestroyOldestComponent()
		{
			TestSingleton prefab = CreateGameObject<TestSingleton>("Prefab");
			GameObject prefabGameObject = prefab.gameObject;
			prefab.DestroyStrategy = SingletonDestroyStrategy.DestroyOldestComponent;

			Assert.IsNotNull(prefab);
			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);

			yield return null;

			TestSingleton newSingleton = CreatePrefab(prefab);
			GameObject newSingletonGameObject = newSingleton.gameObject;

			yield return null;

			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(newSingleton, TestSingleton.Instance);
			Assert.IsNotNull(newSingletonGameObject);
			Assert.IsNotNull(prefabGameObject);
			Assert.IsNull(prefab);
		}

		[UnityTest]
		public IEnumerator DestroyNewestGameObject()
		{
			TestSingleton prefab = CreateGameObject<TestSingleton>("Prefab");
			prefab.DestroyStrategy = SingletonDestroyStrategy.DestroyNewestGameObject;

			Assert.IsNotNull(prefab);
			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);

			yield return null;

			TestSingleton newSingleton = CreatePrefab(prefab);
			GameObject newSingletonGameObject = newSingleton.gameObject;

			yield return null;

			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);
			Assert.IsNull(newSingletonGameObject);
			Assert.IsNull(newSingleton);
		}

		[UnityTest]
		public IEnumerator DestroyOldestGameObject()
		{
			TestSingleton prefab = CreateGameObject<TestSingleton>("Prefab");
			GameObject prefabGameObject = prefab.gameObject;
			prefab.DestroyStrategy = SingletonDestroyStrategy.DestroyOldestGameObject;

			Assert.IsNotNull(prefab);
			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);

			yield return null;

			TestSingleton newSingleton = CreatePrefab(prefab);
			GameObject newSingletonGameObject = newSingleton.gameObject;

			yield return null;

			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(newSingleton, TestSingleton.Instance);
			Assert.IsNotNull(newSingletonGameObject);
			Assert.IsNull(prefabGameObject);
			Assert.IsNull(prefab);
		}

		[UnityTest]
		public IEnumerator KeepBoth()
		{
			TestSingleton prefab = CreateGameObject<TestSingleton>("Prefab");
			GameObject prefabGameObject = prefab.gameObject;
			prefab.DestroyStrategy = SingletonDestroyStrategy.KeepBoth;

			Assert.IsNotNull(prefab);
			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(prefab, TestSingleton.Instance);

			yield return null;

			TestSingleton newSingleton = CreatePrefab(prefab);
			GameObject newSingletonGameObject = newSingleton.gameObject;

			yield return null;

			Assert.IsNotNull(TestSingleton.Instance);
			Assert.AreEqual(newSingleton, TestSingleton.Instance);
			Assert.IsNotNull(newSingletonGameObject);
			Assert.IsNotNull(newSingleton);
			Assert.IsNotNull(prefab);
			Assert.IsNotNull(prefab);
			Assert.IsNotNull(prefabGameObject);
		}
	}
}