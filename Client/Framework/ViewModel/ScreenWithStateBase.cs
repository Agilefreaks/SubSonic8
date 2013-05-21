namespace Subsonic8.Framework.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;

    public class ScreenWithStateBase : Screen, IHaveState
    {
        #region Public Properties

        public string Parameter { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
        }

        public virtual void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
        }

        #endregion
    }
}