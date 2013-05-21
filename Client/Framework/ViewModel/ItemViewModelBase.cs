namespace Subsonic8.Framework.ViewModel
{
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Services;

    public class ItemViewModelBase : PropertyChangedBase
    {
        #region Fields

        private string _coverArt;

        private string _coverArtUrl;

        private ISubsonicModel _item;

        private ISubsonicService _subsonicService;

        private string _title;

        #endregion

        #region Public Properties

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

        public ISubsonicService SubsonicService
        {
            get
            {
                return _subsonicService ?? (_subsonicService = IoC.Get<ISubsonicService>());
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

        #endregion
    }
}