using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Hertzole.UnityToolbox.Tests
{
	public abstract class BaseTest
	{
		private readonly List<Object> objects = new List<Object>();

		[SetUp]
		public void SetUp()
		{
			Assert.AreEqual(0, objects.Count);
		}

		[TearDown]
		public void TearDown()
		{
			for (int i = objects.Count - 1; i >= 0; i--)
			{
				if (objects[i] != null)
				{
					Object.DestroyImmediate(objects[i]);
				}

				objects.RemoveAt(i);
			}

			Assert.AreEqual(0, objects.Count);
		}

		protected GameObject CreateGameObject(string name = "GameObject")
		{
			GameObject go = new GameObject(name);
			objects.Add(go);
			return go;
		}

		protected T CreateGameObject<T>(string name = "GameObject") where T : Component
		{
			GameObject go = new GameObject(name);
			objects.Add(go);
			return go.AddComponent<T>();
		}

		protected T CreatePrefab<T>(T prefab, string name = null) where T : Component
		{
			T instance = Object.Instantiate(prefab);
			instance.name = name ?? $"{prefab.name} (Clone)";
			objects.Add(instance);
			return instance;
		}
	}
}