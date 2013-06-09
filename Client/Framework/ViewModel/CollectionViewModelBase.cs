namespace Subsonic8.Framework.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Results;
    using MugenInjection.Attributes;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Services;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml.Controls;

    public abstract class CollectionViewModelBase<TParameter, TResult> : ViewModelBase, ICollectionViewModel<TParameter>
    {
        #region Fields

        private IBottomBarViewModel _bottomBar;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", 
            Justification = "Reviewed. Suppression is OK here.")]
        private IIoCService _ioCService;

        private BindableCollection<MenuItemViewModel> _menuItems;

        private TParameter _parameter;

        #endregion

        #region Constructors and Destructors

        protected CollectionViewModelBase()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        #endregion

        #region Public Properties

        public IBottomBarViewModel BottomBar
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

        [Inject]
        public IIoCService IoCService
        {
            get
            {
                return _ioCService;
            }

            set
            {
                _ioCService = value;
                LoadBottomBar();
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
                if (Equals(value, _menuItems))
                {
                    return;
                }

                _menuItems = value;
                NotifyOfPropertyChange(() => MenuItems);
            }
        }

        public TParameter Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (Equals(value, _parameter))
                {
                    return;
                }

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

        #endregion

        #region Public Methods and Operators

        public virtual void ChildClick(ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByModelType(subsonicModel);
        }

        public virtual async void Populate()
        {
            await GetResult(Parameter).WithErrorHandler(ErrorDialogViewModel).OnSuccess(OnResultSuccessfull).Execute();
            await AfterPopulate(Parameter);
            UpdateDisplayName();
        }

        #endregion

        #region Methods

        protected virtual Task AfterPopulate(TParameter parameter)
        {
            return Task.Factory.StartNew(() => { });
        }

        protected abstract IEnumerable<IMediaModel> GetItemsToDisplay(TResult result);

        protected abstract IServiceResultBase<TResult> GetResult(TParameter parameter);

        protected virtual void LoadBottomBar()
        {
            BottomBar = IoCService.Get<IDefaultBottomBarViewModel>();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            SetAppBottomBar();
        }

        protected virtual void OnResultSuccessfull(TResult result)
        {
            PopulateMenuItems(result);
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

        #endregion
    }
}