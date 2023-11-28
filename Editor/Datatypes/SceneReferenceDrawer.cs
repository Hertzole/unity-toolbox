using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hertzole.UnityToolbox.Editor
{
	[CustomPropertyDrawer(typeof(SceneReference))]
	internal sealed class SceneReferenceDrawer : ToolboxPropertyDrawer
	{
		private bool enabled;
		private bool added;

		private const string NOT_ENABLED_ERROR = "This scene is not enabled in the build settings.";
		private const string NOT_ADDED_ERROR = "This scene is not added to the build settings.";

		protected override void DrawGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty asset = property.FindPropertyRelative("asset");

			enabled = SceneHelper.IsSceneEnabled((SceneAsset) asset.objectReferenceValue);
			added = SceneHelper.IsSceneAdded((SceneAsset) asset.objectReferenceValue);

			GUIContent newLabel = EditorGUI.BeginProperty(position, label, property);

			Rect rect = position;

			if (!enabled || !added)
			{
				rect.height = 38;
				rect.width -= 42;

				if (!added)
				{
					EditorGUI.HelpBox(rect, NOT_ADDED_ERROR, MessageType.Error);
				}
				else
				{
					EditorGUI.HelpBox(rect, NOT_ENABLED_ERROR, MessageType.Error);
				}

				rect.x += rect.width;
				rect.width = 42;

				if (GUI.Button(rect, "Fix"))
				{
					FixScene((SceneAsset) asset.objectReferenceValue);
				}

				rect.y += 40;
			}

			rect.x = position.x;
			rect.width = position.width;
			rect.height = EditorGUIUtility.singleLineHeight;

			asset.objectReferenceValue = EditorGUI.ObjectField(rect, newLabel, asset.objectReferenceValue, typeof(SceneAsset), false);

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float extra = 0;
			if (!enabled || !added)
			{
				extra = 40;
			}

			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("asset")) + extra;
		}

		protected override VisualElement CreateGUI(SerializedProperty property, string label)
		{
			VisualElement container = new VisualElement();

			VisualElement errorContainer = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					flexGrow = 1
				}
			};

			HelpBox errorBox = new HelpBox(NOT_ADDED_ERROR, HelpBoxMessageType.Error)
			{
				style =
				{
					flexGrow = 1
				}
			};

			Button fixButton = new Button(() =>
			{
				FixScene((SceneAsset) property.FindPropertyRelative("asset").objectReferenceValue);
				errorContainer.style.display = DisplayStyle.None;
			})
			{
				text = "Fix",
				style = { width = 42 }
			};

			ObjectField field = new ObjectField(label)
			{
				objectType = typeof(SceneAsset)
			};

			field.BindProperty(property.FindPropertyRelative("asset"));

			field.RegisterValueChangedCallback(evt =>
			{
				bool newEnabled = SceneHelper.IsSceneEnabled((SceneAsset) evt.newValue);
				bool newAdded = SceneHelper.IsSceneAdded((SceneAsset) evt.newValue);

				errorContainer.style.display = newEnabled && newAdded ? DisplayStyle.None : DisplayStyle.Flex;

				if (!newEnabled || !newAdded)
				{
					if (!newAdded)
					{
						errorBox.text = NOT_ADDED_ERROR;
					}
					else
					{
						errorBox.text = NOT_ENABLED_ERROR;
					}
				}
			});

			field.MakePropertyField();

			container.Add(errorContainer);

			errorContainer.Add(errorBox);
			errorContainer.Add(fixButton);

			container.Add(field);

			errorContainer.style.display = DisplayStyle.None;

			return container;
		}

		private static void FixScene(SceneAsset asset)
		{
			bool enabled = SceneHelper.IsSceneEnabled(asset);
			bool added = SceneHelper.IsSceneAdded(asset);

			if (!added)
			{
				SceneHelper.AddScene(asset);
			}
			else if (!enabled)
			{
				SceneHelper.EnableScene(asset);
			}
		}
	}
}