using System.Collections.Generic;
using Subsonic8.PlaylistItem;

namespace Subsonic8.Messages
{
    public class RemoveFromPlaylistMessage
    {
        public List<PlaylistItemViewModel> Queue { get; set; }
    }
}
