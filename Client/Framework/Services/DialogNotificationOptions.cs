namespace Subsonic8.Framework.Services
{
    using System.Collections.Generic;
    using System.Linq;

    public class DialogNotificationOptions
    {
        #region Fields

        private List<PossibleAction> _possibleActions;

        #endregion

        #region Constructors and Destructors

        public DialogNotificationOptions()
        {
            _possibleActions = new List<PossibleAction>();
        }

        #endregion

        #region Public Properties

        public string Message { get; set; }

        public List<PossibleAction> PossibleActions
        {
            get
            {
                return _possibleActions.Any()
                           ? _possibleActions
                           : new List<PossibleAction> { new PossibleAction("Ok", () => { }) };
            }

            set
            {
                _possibleActions = value;
            }
        }

        #endregion
    }
}