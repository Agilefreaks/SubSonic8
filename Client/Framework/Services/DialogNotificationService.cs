namespace Subsonic8.Framework.Services
{
    using System;
    using System.Threading.Tasks;
    using Windows.UI.Popups;

    public class DialogNotificationService : IDialogNotificationService
    {
        #region Public Methods and Operators

        public async Task Show(DialogNotificationOptions options)
        {
            var dialog = new MessageDialog(options.Message);

            foreach (var possibleAction in options.PossibleActions)
            {
                dialog.Commands.Add(new UICommandAdapter(possibleAction));
            }

            await dialog.ShowAsync();
        }

        #endregion
    }
}