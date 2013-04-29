using Client.Common.Models;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class DetailViewModelBase<T> : CollectionViewModelBase<int, T>, IDetailViewModel<T>
        where T : class, ISubsonicModel
    {
        private T _item;

        public T Item
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
                PopulateMenuItems(Item);
            }
        }

        protected DetailViewModelBase()
        {
            UpdateDisplayName = () => DisplayName = Item == null ? string.Empty : Item.Name;
        }

        protected override void OnResultSuccessfull(T result)
        {
            Item = result;
        }
    }
}