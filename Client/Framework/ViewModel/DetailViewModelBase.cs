namespace Subsonic8.Framework.ViewModel
{
    using Client.Common.Models;

    public abstract class DetailViewModelBase<T> : CollectionViewModelBase<int, T>, IDetailViewModel<T>
        where T : class, ISubsonicModel
    {
        #region Fields

        private T _item;

        #endregion

        #region Constructors and Destructors

        protected DetailViewModelBase()
        {
            UpdateDisplayName = () => DisplayName = Item == null ? string.Empty : Item.Name;
        }

        #endregion

        #region Public Properties

        public T Item
        {
            get
            {
                return _item;
            }

            set
            {
                if (Equals(value, _item))
                {
                    return;
                }

                _item = value;
                NotifyOfPropertyChange();
                PopulateMenuItems(Item);
            }
        }

        #endregion

        #region Methods

        protected override void OnResultSuccessfull(T result)
        {
            Item = result;
        }

        #endregion
    }
}