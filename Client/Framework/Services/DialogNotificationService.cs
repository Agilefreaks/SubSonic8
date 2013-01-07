using System;
using Caliburn.Micro;
using Subsonic8.Messages;
using Windows.UI.Popups;

namespace Subsonic8.Framework.Services
{
    public class DialogNotificationService : IDialogNotificationService
    {
        private readonly IEventAggregator _eventAggregator;

        public DialogNotificationService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        public async void Show(DialogNotificationOptions options)
        {
            var dialog = new MessageDialog(options.Message);

            foreach (var possibleAction in options.PossibleActions)
            {
                dialog.Commands.Add(new UICommandAdapter(possibleAction));
            }

            await dialog.ShowAsync();
        }

        public void Handle(NotificationMessage message)
        {
            Show(message);
        }
    }
}