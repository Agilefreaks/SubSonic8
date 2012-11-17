using System;
using Caliburn.Micro;
using Windows.UI.Popups;

namespace Client.Common.Results
{
    public class MessageDialogResult : ResultBase
    {
        private readonly string _content;
        private readonly string _title;

        public MessageDialogResult(string content, string title)
        {
            _content = content;
            _title = title;
        }

        public async override void Execute(ActionExecutionContext context)
        {
            var dialog = new MessageDialog(_content, _title);

            await dialog.ShowAsync();

            OnCompleted();
        }         
    }
}