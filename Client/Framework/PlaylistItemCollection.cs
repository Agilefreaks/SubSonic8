using System.Collections.ObjectModel;
using System.ComponentModel;
using Subsonic8.PlaylistItem;

namespace Subsonic8.Framework
{
    public class PlaylistItemCollection : ObservableCollection<PlaylistItemViewModel>
    {
        public event PropertyChangedEventHandler PlayingStateChanged;

        protected virtual void OnPlayingStateChanged(PropertyChangedEventArgs e)
        {
            var handler = PlayingStateChanged;
            if (handler != null) handler(this, e);
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((PlaylistItemViewModel) item).PropertyChanged -= NotifyPlaying;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((PlaylistItemViewModel) item).PropertyChanged += NotifyPlaying;
                }
            }
        }

        private void NotifyPlaying(object sender, PropertyChangedEventArgs e)
        {
            OnPlayingStateChanged(e);
        }
    }
}
