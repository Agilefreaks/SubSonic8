using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Index;
using Subsonic8.MenuItem;
using Subsonic8.Settings;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Main
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MainViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
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
                await SubsonicService.GetMusicFolders().WithErrorHandler(this).OnSuccess(SetMenuItems).Execute();
            }
            else
            {
                await ShowSettingsNotFoundDialog();
                DialogService.ShowSettings<SettingsViewModel>();
            }
        }

        public void HandleSuccess(IList<MusicFolder> result)
        {
            SetMenuItems(result);
        }

        public void SetMenuItems(IList<MusicFolder> items)
        {
            MenuItems.AddRange(items.Select(s => s.AsMenuItemViewModel()));
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Populate();
        }

        private async Task ShowSettingsNotFoundDialog()
        {
            var resMap = Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap;
            var message = resMap.GetValue("ShellStrings/NotConfigured").ValueAsString;
            await NotificationService.Show(new DialogNotificationOptions
            {
                Message = message
            });
        }
    }
}