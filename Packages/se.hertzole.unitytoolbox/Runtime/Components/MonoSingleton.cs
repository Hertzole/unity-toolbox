#nullable enable

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
		///     Only removes the component from the newest object.
		/// </summary>
		DestroyNewestComponent = 0,
		/// <summary>
		///     Only removes the component from the oldest object.
		/// </summary>
		DestroyOldestComponent = 1,
		/// <summary>
		///     Keeps both components on both objects.
		/// </summary>
		KeepBoth = 2,
		/// <summary>
		///     Destroys the game object with the newest component.
		/// </summary>
		DestroyNewestGameObject = 3,
		/// <summary>
		///     Destroys the game object with the oldest component.
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
#if UNITY_EDITOR
	[DisallowMultipleComponent]
#endif
	public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
	{
		[SerializeField]
		[EditorTooltip("If keep alive is true, the singleton will not be destroyed when a new scene is loaded.")]
		internal bool keepAlive = true;
		[SerializeField]
		[EditorTooltip(
			"What should happen if there are multiple instances of the singleton.\nDestroy newest component only removes the component from the newest object.\nDestroy oldest component only removes the component from the oldest object.\nKeep both keeps both components.\nDestroy newest game object destroys the game object with the newest component.\nDestroy oldest game object destroys the game object with the oldest component.")]
		internal SingletonDestroyStrategy destroyStrategy = SingletonDestroyStrategy.DestroyNewestGameObject;

		private static T? singletonInstance;

		/// <summary>
		///     If keep alive is <see langword="true"/>, the singleton will not be destroyed when a new scene is loaded.
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
	    ///     Gets a value indicating whether this object is the singleton instance.
	    /// </summary>
	    /// <value><see langword="true"/> if this object is the singleton instance; otherwise <see langword="false"/>.</value>
	    public bool IsSingletonInstance { get; private set; }

		/// <summary>
		///     Gets the current instance of the singleton in the scene.
		/// </summary>
		/// <value>The current instance of the singleton in the scene, or <see langword="null"/> if none is found.</value>
		public static T? Instance
		{
			get
			{
				if (singletonInstance == null)
				{
#if UNITY_2023_1_OR_NEWER
					singletonInstance = FindAnyObjectByType<T>();
#else
					singletonInstance = FindObjectOfType<T>();
#endif
				}

				return singletonInstance;
			}
		}

		/// <summary>
		///     Initializes the singleton instance. If another instance already exists, applies the configured <see cref="DestroyStrategy"/>.
		/// </summary>
		protected void Awake()
		{
			if (singletonInstance != null && singletonInstance != this)
			{
				switch (destroyStrategy)
				{
					case SingletonDestroyStrategy.DestroyNewestComponent:
						IsSingletonInstance = false;
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
						IsSingletonInstance = false;
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
						Debug.LogError($"Invalid singleton destroy strategy: {destroyStrategy}");
						return;
				}
			}

			singletonInstance = (T) (object) this;
			IsSingletonInstance = true;

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