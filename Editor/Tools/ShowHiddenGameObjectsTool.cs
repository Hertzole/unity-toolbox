using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hertzole.UnityToolbox.Editor
{
	public static class ShowHiddenGameObjectsTool
	{
		[MenuItem("Tools/Unity Toolbox/Show Hidden Game Objects")]
		public static void ShowHiddenGameObjects()
		{
			int sceneCount = SceneManager.sceneCount;

			for (int i = 0; i < sceneCount; i++)
			{
				ShowHiddenGameObjectsInScene(SceneManager.GetSceneAt(i));
			}
		}

		private static void ShowHiddenGameObjectsInScene(Scene scene)
		{
			GameObject[] rootObjects = scene.GetRootGameObjects();

			for (int i = 0; i < rootObjects.Length; i++)
			{
				ShowHiddenGameObjectsInGameObject(rootObjects[i]);
			}
		}

		private static void ShowHiddenGameObjectsInGameObject(GameObject gameObject)
		{
			if ((gameObject.hideFlags & HideFlags.HideInHierarchy) != 0)
			{
				gameObject.hideFlags &= ~HideFlags.HideInHierarchy;
			}

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				ShowHiddenGameObjectsInGameObject(gameObject.transform.GetChild(i).gameObject);
			}
		}
	}
}