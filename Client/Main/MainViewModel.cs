namespace Subsonic8.Main
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Services;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.Index;
    using Subsonic8.MenuItem;
    using Subsonic8.Settings;

    public class MainViewModel : CollectionViewModelBase<bool, IList<MusicFolder>>, IMainViewModel
    {
        #region Constructors and Destructors

        public MainViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        #endregion

        #region Public Properties

        [Inject]
        public IResourceService ResourceService { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task Populate()
        {
            ErrorDialogViewModel.Hide();
            if (SubsonicService.HasValidSubsonicUrl)
            {
                if (await ShouldPopulate())
                {
                    await base.Populate();
                }
            }
            else
            {
                await ShowSettingsNotFoundDialog();
                DialogService.ShowSettings<SettingsViewModel>();
            }
        }

        #endregion

        #region Methods

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(IList<MusicFolder> result)
        {
            return result;
        }

        protected override IServiceResultBase<IList<MusicFolder>> GetResult(bool parameter)
        {
            return SubsonicService.GetMusicFolders();
        }

        protected override Task AfterPopulate(bool parameter)
        {
            if (MenuItems.Count == 1)
            {
                NavigationService.NavigateToViewModel<IndexViewModel>(MenuItems[0].Item.Id);
            }

            return base.AfterPopulate(parameter);
        }

        private async Task<bool> ShouldPopulate()
        {
            var populate = true;
            var diagnosticsResult = SubsonicService.Ping();
            await diagnosticsResult.Execute();
            if (diagnosticsResult.Error != null)
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