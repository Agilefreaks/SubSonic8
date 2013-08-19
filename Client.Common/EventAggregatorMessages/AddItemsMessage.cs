namespace Client.Common.EventAggregatorMessages
{
    using System.Collections.Generic;
    using Client.Common.Models;

    public class AddItemsMessage
    {
        #region Constructors and Destructors

        public AddItemsMessage()
        {
            Queue = new List<PlaylistItem>();
        }

        #endregion

        #region Public Properties

        public List<PlaylistItem> Queue { get; set; }

        public bool StartPlaying { get; set; }

        #endregion
    }
}