namespace Client.Common.EventAggregatorMessages
{
    public class PlayItemAtIndexMessage
    {
        public int Index { get; set; }

        public PlayItemAtIndexMessage()
        {
        }

        public PlayItemAtIndexMessage(int index)
        {
            Index = index;
        }
    }
}
