using System;
using System.Collections.Generic;
using Caliburn.Micro;

namespace Subsonic8.Framework.ViewModel
{
    public class ScreenWithStateBase : Screen, IHaveState
    {
        public String Parameter { get; set; }

        public virtual void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
        }

        public virtual void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
        }
    }
}