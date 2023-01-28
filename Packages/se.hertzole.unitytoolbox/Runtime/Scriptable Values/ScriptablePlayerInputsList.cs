#if TOOLBOX_INPUT_SYSTEM && TOOLBOX_SCRIPTABLE_VALUES
using AuroraPunks.ScriptableValues;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Player Inputs List", menuName = "Toolbox/Scriptable Values/Collections/Player Inputs List", order = -1000)]
#endif
	public sealed class ScriptablePlayerInputsList : ScriptableList<IHasPlayerInput> { }
}
#endif