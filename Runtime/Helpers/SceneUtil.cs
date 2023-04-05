using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Hertzole.UnityToolbox
{
	public static class SceneUtil
	{
		public static void FindAllObjectsInScene<T>(Scene scene, List<T> result)
		{
			result.Clear();

			using (ListPool<GameObject>.Get(out List<GameObject> sceneRootObjects))
			{
				sceneRootObjects.Clear();
				scene.GetRootGameObjects(sceneRootObjects);

				using (ListPool<T>.Get(out List<T> sceneObjects))
				{
					foreach (GameObject rootObject in sceneRootObjects)
					{
						sceneObjects.Clear();
						rootObject.GetComponentsInChildren(sceneObjects);

						result.AddRange(sceneObjects);
					}
				}
			}
		}
	}
}