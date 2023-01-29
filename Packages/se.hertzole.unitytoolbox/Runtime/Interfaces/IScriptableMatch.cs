#if TOOLBOX_SCRIPTABLE_VALUES
using System;

namespace Hertzole.UnityToolbox
{
	public interface IScriptableMatch : IDisposable
	{
		void Initialize();

		event Action OnValueChanged;

		bool Matches();
	}
}
#endif