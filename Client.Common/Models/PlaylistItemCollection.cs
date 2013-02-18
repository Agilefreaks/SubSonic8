using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Client.Common.Models
{
    public class PlaylistItemCollection : ObservableCollection<PlaylistItem>
    {
        public void AddRange(IEnumerable<PlaylistItem> items)
        {
            foreach (var playlistItem in items)
            {
                Add(playlistItem);
            }
        }
    }
}