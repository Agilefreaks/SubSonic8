using Client.Common.Models;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.MenuItem
{
    public class MenuItemViewModel : ItemViewModelBase, IMenuItemViewModel
    {
        private string _subtitle;
        private string _type;

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
                if (value == _type) return;
                _type = value;
                NotifyOfPropertyChange(() => Type);
            }
        }
    }
}