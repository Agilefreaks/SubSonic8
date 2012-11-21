using Caliburn.Micro;

namespace Subsonic8.MenuItem
{
    public class MenuItemViewModel : PropertyChangedBase
    {
        private string _title;
        private string _subtitle;
        private object _item;
        private string _type;

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

        public object Item
        {
            get
            {
                return _item;
            }

            set
            {
                if (Equals(value, _item)) return;
                _item = value;
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
                if (value == _type) return;
                _type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }
    }
}