namespace Subsonic8.MenuItem
{
    using Subsonic8.Framework.ViewModel;

    public class MenuItemViewModel : ItemViewModelBase, IMenuItemViewModel
    {
        #region Fields

        private string _subtitle;

        private string _type;

        #endregion

        #region Public Properties

        public string Subtitle
        {
            get
            {
                return _subtitle;
            }

            set
            {
                _subtitle = value;
                NotifyOfPropertyChange();
            }
        }

        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }

        #endregion
    }
}