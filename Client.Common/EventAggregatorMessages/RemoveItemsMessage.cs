namespace Client.Common.EventAggregatorMessages
{
    using System.Collections.Generic;
    using Client.Common.Models;

    public class RemoveItemsMessage
    {
        #region Public Properties

        public List<PlaylistItem> Queue { get; set; }

        #endregion
    }
}