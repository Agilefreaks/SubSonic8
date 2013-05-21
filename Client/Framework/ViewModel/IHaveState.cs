namespace Subsonic8.Framework.ViewModel
{
    using System;
    using System.Collections.Generic;

    public interface IHaveState
    {
        #region Public Methods and Operators

        void LoadState(string parameter, Dictionary<string, object> statePageState);

        void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes);

        #endregion
    }
}