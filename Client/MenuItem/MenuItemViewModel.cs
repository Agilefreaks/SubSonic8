using Caliburn.Micro;

namespace Client.MenuItem
{
    public class MenuItemViewModel : PropertyChangedBase
    {
        private string _title;
        private string _subtitle;

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
    }
}