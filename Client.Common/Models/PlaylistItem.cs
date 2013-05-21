namespace Client.Common.Models
{
    using System;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;
    using Caliburn.Micro;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class PlaylistItem : PropertyChangedBase
    {
        #region Fields

        private string _artist;

        private string _coverArtUrl;

        private int _duration;

        private PlaylistItemState _playingState;

        private string _title;

        private PlaylistItemTypeEnum _type;

        private Uri _uri;

        #endregion

        #region Constructors and Destructors

        public PlaylistItem()
        {
            PlayingState = PlaylistItemState.NotPlaying;
            CoverArtUrl = SubsonicService.CoverArtPlaceholder;
        }

        #endregion

        #region Public Properties

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

        public string CoverArtUrl
        {
            get
            {
                return _coverArtUrl;
            }

            set
            {
                if (value == _coverArtUrl)
                {
                    return;
                }

                _coverArtUrl = value;
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

        public PlaylistItemTypeEnum Type
        {
            get
            {
                return _type;
            }

            set
            {
                if (value == _type)
                {
                    return;
                }

                _type = value;
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
            get
            {
                return Uri.ToString();
            }

            set
            {
                Uri = new Uri(value);
            }
        }

        #endregion
    }
}