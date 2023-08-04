using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     How a singleton should be treated if another one is found in the scene.
	/// </summary>
	public enum SingletonDestroyStrategy
	{
		/// <summary>
		///     Destroy newest component only removes the component from the newest object.
		/// </summary>
		DestroyNewestComponent = 0,
		/// <summary>
		///     Destroy oldest component only removes the component from the oldest object.
		/// </summary>
		DestroyOldestComponent = 1,
		/// <summary>
		///     Keep both keeps both components.
		/// </summary>
		KeepBoth = 2,
		/// <summary>
		///     Destroy newest game object destroys the game object with the newest component.
		/// </summary>
		DestroyNewestGameObject = 3,
		/// <summary>
		///     Destroy oldest game object destroys the game object with the oldest component.
		/// </summary>
		DestroyOldestGameObject = 4
	}

	/// <summary>
	///     Base class for a singleton that inherits from MonoBehaviour and exists in the scene.
	/// </summary>
	/// <example>
	///     <code>
	/// 		public class MySingleton : MonoSingleton&lt;MySingleton&gt;
	/// 		{
	/// 			public void DoSomething() { }
	/// 		}
	/// 		// somewhere else...
	/// 		MySingleton.Instance.DoSomething();
	///  </code>
	/// </example>
	/// <typeparam name="T">Must be the same type as your class.</typeparam>
	[DisallowMultipleComponent]
	public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
	{
		[SerializeField]
		[Tooltip("If keep alive is true, the singleton will not be destroyed when a new scene is loaded.")]
		internal bool keepAlive = true;
		[SerializeField]
		[Tooltip(
			"What should happen if there are multiple instances of the singleton.\nDestroy newest component only removes the component from the newest object.\nDestroy oldest component only removes the component from the oldest object.\nKeep both keeps both components.\nDestroy newest game object destroys the game object with the newest component.\nDestroy oldest game object destroys the game object with the oldest component.")]
		internal SingletonDestroyStrategy destroyStrategy = SingletonDestroyStrategy.DestroyNewestGameObject;

		private static T singletonInstance;

		/// <summary>
		///     If keep alive is true, the singleton will not be destroyed when a new scene is loaded.
		/// </summary>
		public bool KeepAlive
		{
			get { return keepAlive; }
			set { keepAlive = value; }
		}
		/// <summary>
		///     What should happen if there are multiple instances of the singleton.
		/// </summary>
		public SingletonDestroyStrategy DestroyStrategy
		{
			get { return destroyStrategy; }
			set { destroyStrategy = value; }
		}

		/// <summary>
		///     The current instance of the singleton in the scene.
		/// </summary>
		/// <exception cref="System.NullReferenceException">If the singleton can't be found in the scene.</exception>
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

		/// <summary>
		///     If you need to use Awake, override this method instead.
		/// </summary>
		protected virtual void OnAwake() { }
	}
}