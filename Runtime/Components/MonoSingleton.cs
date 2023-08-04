using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	public enum SingletonDestroyStrategy
	{
		DestroyNewestComponent = 0,
		DestroyOldestComponent = 1,
		KeepBoth = 2,
		DestroyNewestGameObject = 3,
		DestroyOldestGameObject = 4
	}

	[DisallowMultipleComponent]
	public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
	{
		[SerializeField]
		[Tooltip("If keep alive is true, the singleton will not be destroyed when a new scene is loaded.")]
		internal bool keepAlive = true;
		[SerializeField]
		[Tooltip("What should happen if there are multiple instances of the singleton.\nDestroy newest component only removes the component from the newest object.\nDestroy oldest component only removes the component from the oldest object.\nKeep both keeps both components.\nDestroy newest game object destroys the game object with the newest component.\nDestroy oldest game object destroys the game object with the oldest component.")]
		internal SingletonDestroyStrategy destroyStrategy = SingletonDestroyStrategy.DestroyNewestGameObject;

		private static T singletonInstance;

		public static T Instance
		{
			get
			{
				if (singletonInstance == null)
				{
#if UNITY_2023_1_OR_NEWER
					singletonInstance = FindFirstObjectByType<T>();
#else
					singletonInstance = FindObjectOfType<T>();
#endif
				}

				return singletonInstance;
			}
		}

		protected void Awake()
		{
			if (singletonInstance != null && singletonInstance != this)
			{
				switch (destroyStrategy)
				{
					case SingletonDestroyStrategy.DestroyNewestComponent:
						Destroy(this);
						return;
					case SingletonDestroyStrategy.DestroyOldestComponent:
						if (singletonInstance is Component comp)
						{
							singletonInstance = (T) (object) comp;
							Destroy(comp);
						}

						break;
					case SingletonDestroyStrategy.KeepBoth:
						break;
					case SingletonDestroyStrategy.DestroyNewestGameObject:
						Destroy(gameObject);
						return;
					case SingletonDestroyStrategy.DestroyOldestGameObject:
						if (singletonInstance is Component comp2)
						{
							singletonInstance = (T) (object) comp2;
							Destroy(comp2.gameObject);
						}

						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			singletonInstance = (T) (object) this;

			if (keepAlive)
			{
				DontDestroyOnLoad(gameObject);
			}

			OnAwake();
		}

		protected virtual void OnAwake() { }
	}
}