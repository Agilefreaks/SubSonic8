namespace Client.Common.EventAggregatorMessages
{
    public class PlayFailedMessage
    {
        public PlayFailedMessage()
        {
        }

        public PlayFailedMessage(string errorMessage, object originalSource)
        {
            ErrorMessage = errorMessage;
            OriginalSource = originalSource;
        }

        public string ErrorMessage { get; set; }

        public object OriginalSource { get; set; }
    }
}