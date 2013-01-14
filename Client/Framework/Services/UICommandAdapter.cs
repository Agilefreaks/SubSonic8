using Windows.UI.Popups;

namespace Subsonic8.Framework.Services
{
    public class UICommandAdapter : IUICommand
    {
        public object Id { get; set; }

        public UICommandInvokedHandler Invoked { get; set; }

        public string Label { get; set; }

        public UICommandAdapter(PossibleAction possibleAction)
        {
            Label = possibleAction.ActionName;

            Invoked = command => possibleAction.Action();
        }
    }
}