using Client.Common.Models;
using Client.Common.ViewModels;
using PropertyChangedBase = Caliburn.Micro.PropertyChangedBase;

namespace Subsonic8.MenuItem
{
    public class MenuItemViewModel : PropertyChangedBase, IMenuItemViewModel
    {
        private string _title;
        private string _subtitle;
        private ISubsonicModel _item;
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

        public ISubsonicModel Item
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