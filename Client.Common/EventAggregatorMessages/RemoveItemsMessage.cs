using System.Collections.Generic;
using Client.Common.Models;

namespace Client.Common.EventAggregatorMessages
{
    public class RemoveItemsMessage
    {
        public List<PlaylistItem> Queue { get; set; }
    }
}
