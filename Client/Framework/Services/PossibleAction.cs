namespace Subsonic8.Framework.Services
{
    using System;

    public class PossibleAction
    {
        #region Constructors and Destructors

        public PossibleAction(string name, Action action)
        {
            ActionName = name;

            Action = action;
        }

        #endregion

        #region Public Properties

        public Action Action { get; private set; }

        public string ActionName { get; private set; }

        #endregion
    }
}