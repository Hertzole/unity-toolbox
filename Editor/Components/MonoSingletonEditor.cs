using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomEditor(typeof(MonoSingleton<>), true)]
	[CanEditMultipleObjects]
	public class MonoSingletonEditor : UnityEditor.Editor
	{
		protected SerializedProperty keepAlive;
		protected SerializedProperty destroyStrategy;

		private static readonly string[] ignoreProperties =
			{ "m_Script", nameof(keepAlive), nameof(destroyStrategy) };

		protected virtual bool CreateDefaultInspector
		{
			get { return true; }
		}

		protected virtual void OnEnable()
		{
			keepAlive = serializedObject.FindProperty(nameof(keepAlive));
			destroyStrategy = serializedObject.FindProperty(nameof(destroyStrategy));
		}

		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement
			{
				name = "singleton-root"
			};

			PropertyField keepAliveField = new PropertyField(keepAlive);
			keepAliveField.Bind(serializedObject);

			PropertyField destroyStrategyField = new PropertyField(destroyStrategy);
			destroyStrategyField.Bind(serializedObject);

			root.Add(new Label("Singleton Settings")
			{
				style =
				{
					unityFontStyleAndWeight = FontStyle.Bold,
					marginLeft = 3,
					marginTop = 2
				}
			});

			root.Add(keepAliveField);
			root.Add(destroyStrategyField);

			if (CreateDefaultInspector)
			{
				root.Add(VisiaulElementUtilities.VerticalSpace());

				InspectorUtilities.CreateDefaultInspector(serializedObject, root, ignoreProperties);
			}

			return root;
		}
	}
}