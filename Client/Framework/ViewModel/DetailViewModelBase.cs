﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Results;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class DetailViewModelBase<T> : ViewModelBase, IDetailViewModel<T>
        where T : class, ISubsonicModel
    {
        private BindableCollection<MenuItemViewModel> _menuItems;
        private object _parameter;
        private T _item;

        public object Parameter
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
            UpdateDisplayName = () => DisplayName = Item == null ? string.Empty : Item.Name;
        }

        public void ChildClick(ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByModelType(subsonicModel);
        }

        protected abstract IServiceResultBase<T> GetResult(int id);

        protected abstract IEnumerable<ISubsonicModel> GetItemsToDisplay();

        protected async virtual void LoadModel()
        {
            if (!(Parameter is int)) return;

            var id = (int)Parameter;
            await GetResult(id).WithErrorHandler(this).OnSuccess(result => Item = result).Execute();
            await AfterLoadModel(id);
            UpdateDisplayName();
        }

        protected virtual Task AfterLoadModel(int id)
        {
            return new Task(() => { });
        }

        private void PopulateMenuItems()
        {
            var children = GetItemsToDisplay();
            MenuItems.AddRange(children.Select(s => s.AsMenuItemViewModel()));
        }
    }
}