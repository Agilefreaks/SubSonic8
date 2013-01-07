using System;

namespace Subsonic8.Framework.Services
{
    public class PossibleAction
    {
        public string ActionName { get; private set; }

        public Action Action { get; private set; }

        public PossibleAction(string name, Action action)
        {
            ActionName = name;

            Action = action;
        }    
    }
}