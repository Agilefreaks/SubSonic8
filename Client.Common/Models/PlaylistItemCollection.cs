namespace Client.Common.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class PlaylistItemCollection : ObservableCollection<PlaylistItem>
    {
        #region Public Methods and Operators

        public void AddRange(IEnumerable<PlaylistItem> items)
        {
            foreach (var playlistItem in items)
            {
                Add(playlistItem);
            }
        }

        #endregion
    }
}