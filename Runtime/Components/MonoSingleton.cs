using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	public enum SingletonDestroyStrategy
	{
		DestroyNewest = 0,
		DestroyOldest = 1,
		KeepBoth = 2
	}

	public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
	{
		[Header("Singleton Settings")]
		[SerializeField]
		private bool keepAlive = true;
		[SerializeField]
		private SingletonDestroyStrategy destroyStrategy = default;

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
					case SingletonDestroyStrategy.DestroyNewest:
						Destroy(gameObject);
						return;
					case SingletonDestroyStrategy.DestroyOldest:
						if (singletonInstance is Component comp)
						{
							Destroy(comp.gameObject);
						}

						break;
					case SingletonDestroyStrategy.KeepBoth:
						return;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			singletonInstance = (T) (object) this;

			if (keepAlive)
			{
				DontDestroyOnLoad(gameObject);
			}
		}

		protected virtual void OnAwake() { }
	}
}