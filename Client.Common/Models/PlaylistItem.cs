using System;
using System.Xml.Serialization;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Services;

namespace Client.Common.Models
{
    public class PlaylistItem : PropertyChangedBase
    {
        private Uri _uri;
        private PlaylistItemState _playingState;
        private int _duration;
        private string _artist;
        private string _coverArtUrl;
        private string _title;
        private PlaylistItemTypeEnum _type;

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

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
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

        [XmlIgnore]
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

        public string UriAsString
        {
            get { return Uri.ToString(); }
            set { Uri = new Uri(value); }
        }

        public string CoverArtUrl
        {
            get
            {
                return _coverArtUrl;
            }

            set
            {
                if (value == _coverArtUrl) return;
                _coverArtUrl = value;
                NotifyOfPropertyChange();
            }
        }

        public PlaylistItemTypeEnum Type
        {
            get
            {
                return _type;
            }

            set
            {
                if (value == _type) return;
                _type = value;
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

        public void InitializeFromSong(Song result, ISubsonicService subsonicService)
        {
            Artist = result.Artist;
            Title = result.Title;
            Uri = result.Type == SubsonicModelTypeEnum.Video
                      ? subsonicService.GetUriForVideoWithId(result.Id)
                      : subsonicService.GetUriForFileWithId(result.Id);
            CoverArtUrl = subsonicService.GetCoverArtForId(result.CoverArt);
            PlayingState = PlaylistItemState.NotPlaying;
            Duration = result.Duration;
            Type = result.Type == SubsonicModelTypeEnum.Video
                       ? PlaylistItemTypeEnum.Video
                       : PlaylistItemTypeEnum.Audio;
        }
    }
}