namespace Client.Common.EventAggregatorMessages
{
    public class PlayItemAtIndexMessage
    {
        #region Constructors and Destructors

        public PlayItemAtIndexMessage()
        {
        }

        public PlayItemAtIndexMessage(int index)
        {
            Index = index;
        }

        #endregion

        #region Public Properties

        public int Index { get; set; }

        #endregion
    }
}