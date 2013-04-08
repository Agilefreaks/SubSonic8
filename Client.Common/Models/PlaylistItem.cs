using System;
using System.Text.RegularExpressions;
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

        [XmlIgnore]
        public string OriginalCoverArtUrl
        {
            get
            {
                var regex = new Regex(@"&size=[\d]{1,}");
                var strings = regex.Split(CoverArtUrl);
                var result = strings[0];
                var uri = new Uri(result, UriKind.RelativeOrAbsolute);
                if (uri.IsAbsoluteUri)
                {
                    result += string.Format("&size={0}", (int)ImageType.Original);
                }

                return result;
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

        [XmlIgnore]
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

        public PlaylistItem()
        {
            PlayingState = PlaylistItemState.NotPlaying;
            CoverArtUrl = SubsonicService.CoverArtPlaceholder;
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