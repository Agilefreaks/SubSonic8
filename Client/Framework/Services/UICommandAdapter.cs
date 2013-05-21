namespace Subsonic8.Framework.Services
{
    using Windows.UI.Popups;

    public class UICommandAdapter : IUICommand
    {
        #region Constructors and Destructors

        public UICommandAdapter(PossibleAction possibleAction)
        {
            Label = possibleAction.ActionName;

            Invoked = command => possibleAction.Action();
        }

        #endregion

        #region Public Properties

        public object Id { get; set; }

        public UICommandInvokedHandler Invoked { get; set; }

        public string Label { get; set; }

        #endregion
    }
}