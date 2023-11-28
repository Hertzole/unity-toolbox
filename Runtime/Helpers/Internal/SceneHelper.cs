using System;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.UnityToolbox
{
	internal static class SceneHelper
	{
		private static readonly ConcurrentDictionary<int, string> sceneNames = new ConcurrentDictionary<int, string>();
		private static readonly ConcurrentDictionary<int, string> scenePaths = new ConcurrentDictionary<int, string>();

#if UNITY_EDITOR
		private static readonly ConcurrentDictionary<SceneAsset, GUID> sceneGuids = new ConcurrentDictionary<SceneAsset, GUID>();
		private static readonly ConcurrentDictionary<GUID, int> sceneIndexes = new ConcurrentDictionary<GUID, int>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeRuntime()
		{
			sceneNames.Clear();
			scenePaths.Clear();
		}

		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			EditorBuildSettings.sceneListChanged -= Reset;
			EditorBuildSettings.sceneListChanged += Reset;
		}

		private static void Reset()
		{
			sceneNames.Clear();
			sceneGuids.Clear();
			sceneIndexes.Clear();
			scenePaths.Clear();
		}

		public static int GetSceneIndex(SceneAsset asset)
		{
			if (asset == null)
			{
				return -1;
			}

			if (!sceneGuids.TryGetValue(asset, out GUID guid))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				guid = new GUID(AssetDatabase.AssetPathToGUID(path));
				sceneGuids.TryAdd(asset, guid);
			}

			if (!sceneIndexes.TryGetValue(guid, out int index))
			{
				index = GetSceneIndex(guid);
				sceneIndexes.TryAdd(guid, index);
			}

			return sceneIndexes[guid];
		}

		private static int GetSceneIndex(GUID guid)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			int index = 0;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (scene.guid == guid)
				{
					if (!scene.enabled)
					{
						break;
					}

					return index;
				}

				// Only increment if the scene is enabled.
				if (scene.enabled)
				{
					index++;
				}
			}

			return -1;
		}

		public static SceneAsset GetSceneAsset(int index)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			int currentIndex = 0;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (!scene.enabled)
				{
					continue;
				}

				if (currentIndex == index)
				{
					return AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);
				}

				currentIndex++;
			}

			return null;
		}

		public static bool IsSceneEnabled(SceneAsset asset)
		{
			if (asset == null)
			{
				return false;
			}

			if (!sceneGuids.TryGetValue(asset, out GUID guid))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				guid = new GUID(AssetDatabase.AssetPathToGUID(path));
				sceneGuids.TryAdd(asset, guid);
			}

			return IsSceneEnabled(guid);
		}

		private static bool IsSceneEnabled(GUID guid)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (scene.guid == guid)
				{
					return scene.enabled;
				}
			}

			return false;
		}

		public static bool IsSceneAdded(SceneAsset asset)
		{
			if (asset == null)
			{
				return false;
			}

			if (!sceneGuids.TryGetValue(asset, out GUID guid))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				guid = new GUID(AssetDatabase.AssetPathToGUID(path));
				sceneGuids.TryAdd(asset, guid);
			}

			return IsSceneAdded(guid);
		}

		private static bool IsSceneAdded(GUID guid)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (scene.guid == guid)
				{
					return true;
				}
			}

			return false;
		}

		public static void AddScene(SceneAsset asset)
		{
			if (asset == null)
			{
				return;
			}

			if (!sceneGuids.TryGetValue(asset, out GUID guid))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				guid = new GUID(AssetDatabase.AssetPathToGUID(path));
				sceneGuids.TryAdd(asset, guid);
			}

			AddScene(guid);
		}

		private static void AddScene(GUID guid)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (scene.guid == guid)
				{
					return;
				}
			}

			Array.Resize(ref scenes, scenes.Length + 1);
			scenes[scenes.Length - 1] = new EditorBuildSettingsScene(AssetDatabase.GUIDToAssetPath(guid.ToString()), true);
			EditorBuildSettings.scenes = scenes;
			Reset();
			sceneIndexes.TryAdd(guid, scenes.Length - 1);
		}

		public static void EnableScene(SceneAsset asset)
		{
			if (asset == null)
			{
				return;
			}

			if (!sceneGuids.TryGetValue(asset, out GUID guid))
			{
				string path = AssetDatabase.GetAssetPath(asset);
				guid = new GUID(AssetDatabase.AssetPathToGUID(path));
				sceneGuids.TryAdd(asset, guid);
			}

			EnableScene(guid);
		}

		private static void EnableScene(GUID guid)
		{
			EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; i++)
			{
				EditorBuildSettingsScene scene = scenes[i];

				if (scene.guid == guid)
				{
					scene.enabled = true;
					scenes[i] = scene;
					EditorBuildSettings.scenes = scenes;
					return;
				}
			}

			Reset();
		}
#endif

		public static string GetSceneName(int sceneIndex)
		{
			if (sceneIndex < 0)
			{
				return "null";
			}

			if (!sceneNames.TryGetValue(sceneIndex, out string name))
			{
				ReadOnlySpan<char> path = GetScenePath(sceneIndex).AsSpan();
				int start = path.LastIndexOf('/') + 1;
				const int end = 6; // .unity
				ReadOnlySpan<char> nameSpan = path.Slice(start, path.Length - start - end);
				name = nameSpan.ToString();
				sceneNames.TryAdd(sceneIndex, name);
			}

			return name;
		}

		public static string GetScenePath(int sceneIndex)
		{
			if (!scenePaths.TryGetValue(sceneIndex, out string path))
			{
				path = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
				scenePaths.TryAdd(sceneIndex, path);
			}

			return path;
		}
	}
}