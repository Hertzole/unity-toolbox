#if ODIN_INSPECTOR
using BaseScript = Sirenix.OdinInspector.Editor.OdinEditor;

#else
using BaseScript = UnityEditor.Editor;
#endif

namespace Hertzole.UnityToolbox.Editor
{
	public abstract class ToolboxEditor : BaseScript
	{
#if ODIN_INSPECTOR
		protected override void OnEnable()
		{
			base.OnEnable();
#else
		protected virtual void OnEnable()
		{
#endif
		}
	}
}