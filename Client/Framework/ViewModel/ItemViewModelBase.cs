using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Services;

namespace Subsonic8.Framework.ViewModel
{
    public class ItemViewModelBase : PropertyChangedBase
    {
        private string _title;
        private string _coverArt;
        private string _coverArtUrl;
        private ISubsonicModel _item; 

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
                return _coverArtUrl ?? (_coverArtUrl = SubsonicService.GetCoverArtForId(_coverArt));
            }
        }

        public string CoverArtId
        {
            get
            {
                return _coverArt;
            }

            set
            {
                _coverArt = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CoverArt);
            }
        }

        public ISubsonicModel Item
        {
            get
            {
                return _item;
            }

            set
            {
                _item = value;
                NotifyOfPropertyChange();
            }
        }

        public ItemViewModelBase()
        {
            SubsonicService = IoC.Get<ISubsonicService>();
        }
    }
}
