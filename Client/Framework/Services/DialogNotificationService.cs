using System;
using Windows.UI.Popups;

namespace Subsonic8.Framework.Services
{
    public class DialogNotificationService : IDialogNotificationService
    {
        public async void Show(DialogNotificationOptions options)
        {
            var dialog = new MessageDialog(options.Message);

            foreach (var possibleAction in options.PossibleActions)
            {
                dialog.Commands.Add(new UICommandAdapter(possibleAction));
            }

            await dialog.ShowAsync();
        }
    }
}