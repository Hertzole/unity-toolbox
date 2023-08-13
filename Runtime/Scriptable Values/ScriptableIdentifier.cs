#if TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Scriptable Identifier", menuName = "Toolbox/Scriptable Values/Values/Scriptable Identifier")]
#endif
	public sealed class ScriptableIdentifier : ScriptableValue<Identifier> { }
}
#endif