using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : Object
	{
		private static T singletonInstance;

		public static T Instance
		{
			get
			{
				if (singletonInstance == null)
				{
					singletonInstance = FindObjectOfType<T>();
				}

				return singletonInstance;
			}
		}
		
		protected virtual bool KeepAlive { get; } = true;

		protected void Awake()
		{
			if (singletonInstance != null && singletonInstance != this)
			{
				Destroy(gameObject);
				return;
			}

			singletonInstance = (T) (object) this;
			
			if (KeepAlive)
			{
				DontDestroyOnLoad(gameObject);
			}
		}

		protected virtual void OnAwake() { }
	}
}