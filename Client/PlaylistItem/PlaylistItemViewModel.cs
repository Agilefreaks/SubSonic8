using System;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.PlaylistItem
{
    public class PlaylistItemViewModel : ItemViewModelBase
    {
        private string _artist;
        private int _duration;
        private Uri _uri;
        private PlaylistItemState _playingState;

        public string Artist
        {
            get
            {
                return _artist;
            }

            set
            {
                _artist = value;
                NotifyOfPropertyChange();
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }

            set
            {
                _duration = value;
                NotifyOfPropertyChange();
            }
        }

        public Uri Uri
        {
            get
            {
                return _uri;
            }

            set
            {
                _uri = value;
                NotifyOfPropertyChange();
            }
        }

        public PlaylistItemState PlayingState
        {
            get
            {
                return _playingState;
            }

            set
            {
                _playingState = value;
                NotifyOfPropertyChange();
            }
        }

        public PlaylistItemViewModel()
        {
            PlayingState = PlaylistItemState.NotPlaying;
        }
    }
}
