namespace Subsonic8.Main
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Models.Subsonic;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.Index;
    using Subsonic8.MenuItem;
    using Subsonic8.Settings;
    using Windows.UI.Xaml.Controls;

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        #region Constructors and Destructors

        public MainViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        #endregion

        #region Public Properties

        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public bool Parameter { get; set; }

        [Inject]
        public IResourceService ResourceService { get; set; }

        #endregion

        #region Public Methods and Operators

        public void HandleSuccess(IList<MusicFolder> result)
        {
            SetMenuItems(result);
        }

        public void IndexClick(ItemClickEventArgs eventArgs)
        {
            var item = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateToViewModel<IndexViewModel>(item.Id);
        }

        public async void Populate()
        {
            if (SubsonicService.HasValidSubsonicUrl)
            {
                if (await ShouldPopulate())
                {
                    await
                        SubsonicService.GetMusicFolders()
                                       .WithErrorHandler(ErrorDialogViewModel)
                                       .OnSuccess(SetMenuItems)
                                       .Execute();
                    if (MenuItems.Count == 1)
                    {
                        NavigationService.NavigateToViewModel<IndexViewModel>(MenuItems[0].Item.Id);
                    }
                }
            }
            else
            {
                await ShowSettingsNotFoundDialog();
                DialogService.ShowSettings<SettingsViewModel>();
            }
        }

        public void SetMenuItems(IList<MusicFolder> items)
        {
            MenuItems.AddRange(items.Select(s => s.AsMenuItemViewModel()));
        }

        #endregion

        #region Methods

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Populate();
        }

        private async Task<bool> ShouldPopulate()
        {
            var populate = true;
            var diagnosticsResult = SubsonicService.Ping();
            await diagnosticsResult.Execute();
            if (diagnosticsResult.Error == null)
            {
                if (diagnosticsResult.ApiError != null)
                {
                    populate = false;
                    ErrorDialogViewModel.HandleError(diagnosticsResult.ApiError.Message);
                }
            }
            else
            {
                populate = false;
                ErrorDialogViewModel.HandleError(diagnosticsResult.Error);
            }

            return populate;
        }

        private async Task ShowSettingsNotFoundDialog()
        {
            var message = ResourceService.GetStringResource("ShellStrings/NotConfigured");
            await NotificationService.Show(new DialogNotificationOptions { Message = message });
        }

        #endregion
    }
}