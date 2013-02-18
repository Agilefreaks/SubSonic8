using System.Collections.Generic;
using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class AddItemsMessage
    {
        public List<PlaylistItem> Queue { get; set; }

        public AddItemsMessage()
        {
            Queue = new List<PlaylistItem>();
        }
    }
}