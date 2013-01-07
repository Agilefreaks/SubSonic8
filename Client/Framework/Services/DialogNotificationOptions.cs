using System.Collections.Generic;
using System.Linq;

namespace Subsonic8.Framework.Services
{
    public class DialogNotificationOptions
    {
        private List<PossibleAction> _possibleActions;

        public string Message { get; set; }

        public List<PossibleAction> PossibleActions
        {
            get
            {
                return _possibleActions.Any() 
                    ? _possibleActions 
                    : new List<PossibleAction> { new PossibleAction("Ok", () => {})};
            }

            set { _possibleActions = value; }
        }

        public DialogNotificationOptions()
        {
            _possibleActions = new List<PossibleAction>();
        }
    }
}