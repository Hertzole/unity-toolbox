#if TOOLBOX_SCRIPTABLE_VALUES
using Hertzole.ScriptableValues;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
#if UNITY_EDITOR
	[CreateAssetMenu(fileName = "New Spawnpoints List", menuName = "Toolbox/Scriptable Values/Collections/Spawnpoints List", order = -1000)]
#endif
	public sealed class ScriptableSpawnpointsList : ScriptableList<Spawnpoint> { }
}
#endif