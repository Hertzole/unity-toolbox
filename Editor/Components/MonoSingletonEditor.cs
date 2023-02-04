using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	public class MonoSingletonEditor : UnityEditor.Editor
	{
		protected SerializedProperty keepAlive;
		protected SerializedProperty destroyStrategy;

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

			root.Add(keepAliveField);
			root.Add(destroyStrategyField);

			return root;
		}
	}
}