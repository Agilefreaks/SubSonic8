using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Results;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class DetailViewModelBase<T> : ViewModelBase, IDetailViewModel<T>
        where T : ISubsonicModel
    {
        private BindableCollection<MenuItemViewModel> _menuItems;
        private ISubsonicModel _parameter;
        private T _item;

        public ISubsonicModel Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (Equals(value, _parameter)) return;
                
                _parameter = value;
                NotifyOfPropertyChange();
               
                LoadModel();
                UpdateDisplayName();
            }
        }

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
                PopulateMenuItems();
            }
        }

        public BindableCollection<MenuItemViewModel> MenuItems
        {
            get
            {
                return _menuItems;
            }

            set
            {
                if (Equals(value, _menuItems)) return;
                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        protected DetailViewModelBase()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
            UpdateDisplayName = () => DisplayName = _parameter.GetDescription().Item1;
        }

        public void ChildClick(ItemClickEventArgs eventArgs)
        {
            var navigableEntity = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByEntityType(navigableEntity);
        }

        protected abstract IServiceResultBase<T> GetResult(int id);

        protected abstract IEnumerable<ISubsonicModel> GetItemsToDisplay();

        private async void LoadModel()
        {
            var getModel = GetResult(Parameter.Id);
            await getModel.Execute();
            Item = getModel.Result;
        }

        private void PopulateMenuItems()
        {
            var children = GetItemsToDisplay();
            MenuItems.AddRange(children.Select(s => s.AsMenuItemViewModel()));
        }
    }
}