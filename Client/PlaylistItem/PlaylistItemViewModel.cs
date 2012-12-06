using System;
using Caliburn.Micro;

namespace Subsonic8.PlaylistItem
{
    public class PlaylistItemViewModel : PropertyChangedBase
    {
        private Uri _coverArt;
        private string _artist;
        private string _title;
        private int _duration;
        private Uri _uri;

        public Uri CoverArt
        {
            get { return _coverArt; }

            set
            {
                _coverArt = value;
                NotifyOfPropertyChange();
            }
        }

        public string Artist
        {
            get { return _artist; }
            set
            {
                _artist = value;
                NotifyOfPropertyChange();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        public int Duration
        {
            get { return _duration; }

            set
            {
                _duration = value;
                NotifyOfPropertyChange();
            }
        }

        public Uri Uri
        {
            get { return _uri; }

            set
            {
                _uri = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
