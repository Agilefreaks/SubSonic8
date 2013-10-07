namespace Client.Common.Results
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Results;
    using Windows.UI.Popups;

    public class MessageDialogResult : ResultBase
    {
        #region Fields

        private readonly string _content;

        private readonly string _title;

        #endregion

        #region Constructors and Destructors

        public MessageDialogResult(string content, string title)
        {
            _content = content;
            _title = title;
        }

        #endregion

        #region Methods

        protected override async Task ExecuteCore()
        {
            await new MessageDialog(_content, _title).ShowAsync();
        }

        #endregion
    }
}