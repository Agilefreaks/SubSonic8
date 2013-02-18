namespace Client.Common.EventAggregatorMessages
{
    public class PlaylistStateChangedMessage
    {
        private readonly bool _hasElements;

        public bool HasElements
        {
            get { return _hasElements; }
        }

        public PlaylistStateChangedMessage()
        {
        }

        public PlaylistStateChangedMessage(bool hasElements)
        {
            _hasElements = hasElements;
        }
    }
}