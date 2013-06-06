namespace Client.Common.EventAggregatorMessages
{
    public class PlayFailedMessage
    {
        #region Constructors and Destructors

        public PlayFailedMessage()
        {
        }

        public PlayFailedMessage(string errorMessage, object originalSource)
        {
            ErrorMessage = errorMessage;
            OriginalSource = originalSource;
        }

        #endregion

        #region Public Properties

        public string ErrorMessage { get; set; }

        public object OriginalSource { get; set; }

        #endregion
    }
}