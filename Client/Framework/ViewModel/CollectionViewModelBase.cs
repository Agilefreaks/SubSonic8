using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Results;
using MugenInjection.Attributes;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Framework.ViewModel
{
    public abstract class CollectionViewModelBase<TParameter, TResult> : ViewModelBase, ICollectionViewModel<TParameter>
    {
        private TParameter _parameter;
        private IDefaultBottomBarViewModel _bottomBar;
        private BindableCollection<MenuItemViewModel> _menuItems;

        public TParameter Parameter
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
                Populate();
            }
        }

        public ObservableCollection<object> SelectedItems
        {
            get
            {
                return BottomBar != null ? BottomBar.SelectedItems : new ObservableCollection<object>();
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

        [Inject]
        public IDefaultBottomBarViewModel BottomBar
        {
            get
            {
                return _bottomBar;
            }

            set
            {
                _bottomBar = value;
                NotifyOfPropertyChange();
            }
        }

        protected CollectionViewModelBase()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        public void ChildClick(ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByModelType(subsonicModel);
        }

        public async virtual void Populate()
        {
            await GetResult(Parameter).WithErrorHandler(this).OnSuccess(OnResultSuccessfull).Execute();
            await AfterPopulate(Parameter);
            UpdateDisplayName();
        }

        protected abstract IServiceResultBase<TResult> GetResult(TParameter parameter);

        protected abstract IEnumerable<IMediaModel> GetItemsToDisplay(TResult result);

        protected virtual void OnResultSuccessfull(TResult result)
        {
            PopulateMenuItems(result);
        }

        protected virtual Task AfterPopulate(TParameter parameter)
        {
            return Task.Factory.StartNew(() => { });
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SetAppBottomBar();
        }

        protected virtual void PopulateMenuItems(TResult result)
        {
            MenuItems.Clear();
            var children = GetItemsToDisplay(result);
            MenuItems.AddRange(children.Select(s => s.AsMenuItemViewModel()));
        }

        private void SetAppBottomBar()
        {
            EventAggregator.Publish(new ChangeBottomBarMessage { BottomBarViewModel = BottomBar });
        }
    }
}