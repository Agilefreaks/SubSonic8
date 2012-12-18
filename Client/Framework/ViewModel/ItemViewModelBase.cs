using Caliburn.Micro;
using Client.Common.Services;

namespace Subsonic8.Framework.ViewModel
{
    public class ItemViewModelBase : PropertyChangedBase
    {
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

        public ItemViewModelBase()
        {
            SubsonicService = IoC.Get<ISubsonicService>();
        }
    }
}
