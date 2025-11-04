#if TOOLBOX_SCRIPTABLE_VALUES
using System;

namespace Hertzole.UnityToolbox
{
    [Obsolete("Use IMatcher instead. IScriptableMatch will be removed in future versions.")]
    public interface IScriptableMatch : IDisposable
    {
        void Initialize();

        event Action OnValueChanged;

        bool Matches();
    }
}
#endif