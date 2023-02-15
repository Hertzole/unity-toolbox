#if TOOLBOX_TMP
using TMPro;
using UnityEngine;

namespace Hertzole.UnityToolbox
{
	public sealed class VersionLabel : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text label = default;

		private void Awake()
		{
			label.text = Application.version;
		}
	}
}
#endif