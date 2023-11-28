using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hertzole.UnityToolbox
{
	/// <summary>
	///     A reference to a Unity scene.
	/// </summary>
	[Serializable]
	public struct SceneReference : IEquatable<SceneReference>,
		IEquatable<int>
#if UNITY_EDITOR
		,
		ISerializationCallbackReceiver
#endif
	{
		[SerializeField]
		private int sceneIndex;
#if UNITY_EDITOR
		[SerializeField]
		private SceneAsset asset;
#endif

		/// <summary>
		///     The build index of the scene.
		/// </summary>
		public int SceneIndex
		{
			get { return sceneIndex; }
		}

		/// <summary>
		///     The name of the scene.
		/// </summary>
		public string SceneName
		{
			get { return SceneHelper.GetSceneName(sceneIndex); }
		}

		/// <summary>
		///     The path of the scene.
		/// </summary>
		public string ScenePath
		{
			get { return SceneHelper.GetScenePath(sceneIndex); }
		}

		public SceneReference(int sceneIndex)
		{
			this.sceneIndex = sceneIndex;
#if UNITY_EDITOR
			asset = SceneHelper.GetSceneAsset(sceneIndex);
#endif
		}

		public override int GetHashCode()
		{
			return sceneIndex;
		}

		public static bool operator ==(SceneReference left, SceneReference right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SceneReference left, SceneReference right)
		{
			return !left.Equals(right);
		}

		public static bool operator ==(SceneReference left, int right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(SceneReference left, int right)
		{
			return !left.Equals(right);
		}

		public static bool operator ==(int left, SceneReference right)
		{
			return right.Equals(left);
		}

		public static bool operator !=(int left, SceneReference right)
		{
			return !right.Equals(left);
		}

		public static implicit operator int(SceneReference sceneReference)
		{
			return sceneReference.sceneIndex;
		}

		public static implicit operator SceneReference(int sceneIndex)
		{
			return new SceneReference(sceneIndex);
		}

		public override string ToString()
		{
			return SceneHelper.GetSceneName(sceneIndex);
		}

		public override bool Equals(object obj)
		{
			return obj is SceneReference other && Equals(other);
		}

		public bool Equals(int other)
		{
			return sceneIndex == other;
		}

		public bool Equals(SceneReference other)
		{
			return sceneIndex == other.sceneIndex;
		}

#if UNITY_EDITOR
		public void OnBeforeSerialize()
		{
			sceneIndex = SceneHelper.GetSceneIndex(asset);
		}

		public void OnAfterDeserialize() { }
#endif
	}
}