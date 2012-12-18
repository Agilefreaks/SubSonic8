using Caliburn.Micro;
using Client.Common.Services;

namespace Subsonic8.Framework.ViewModel
{
    public class ItemViewModelBase : PropertyChangedBase
    {
        private string _title;
        private string _coverArt;
        private string _coverArtUrl;
        public const string CoverArtPlaceholder = @"/Assets/CoverArtPlaceholder.jpg";

        public ISubsonicService SubsonicService { get; set; }

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

        public string CoverArt
        {
            get
            {
                if(string.IsNullOrEmpty(_coverArtUrl))
                {
                    _coverArtUrl = SubsonicService.GetCoverArtForId(_coverArt);
                    
                    if(string.IsNullOrEmpty(_coverArtUrl))
                    {
                        _coverArtUrl = CoverArtPlaceholder;
                    }
                }
                return _coverArtUrl;
            }

            set
            {
                _coverArt = value;
                _coverArtUrl = null;
                NotifyOfPropertyChange();
            }
        }

        public ItemViewModelBase()
        {
            SubsonicService = IoC.Get<ISubsonicService>();
        }
    }
}
