using System;
using System.Collections.Generic;

namespace Subsonic8.Framework.ViewModel
{
    public interface IHaveState
    {
        void LoadState(string parameter, Dictionary<string, object> statePageState);

        void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes);
    }
}