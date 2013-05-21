namespace Client.Common.EventAggregatorMessages
{
    public class PlaylistStateChangedMessage
    {
        #region Fields

        private readonly bool _hasElements;

        #endregion

        #region Constructors and Destructors

        public PlaylistStateChangedMessage(bool hasElements)
        {
            _hasElements = hasElements;
        }

        #endregion

        #region Public Properties

        public bool HasElements
        {
            get
            {
                return _hasElements;
            }
        }

        #endregion
    }
}