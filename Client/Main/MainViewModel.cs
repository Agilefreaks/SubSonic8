using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.Services;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Index;
using Subsonic8.MenuItem;
using Subsonic8.Settings;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Main
{
    public class MainViewModel : ViewModelBase, IMainViewModel, IResultHandler<IList<IndexItem>>
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MainViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        public void IndexClick(ItemClickEventArgs eventArgs)
        {
            var item = ((MenuItemViewModel)eventArgs.ClickedItem).Item;
            NavigationService.NavigateToViewModel<IndexViewModel>(item);
        }

        public async void Populate()
        {
            if (SubsonicService.HasValidSubsonicUrl)
            {
                await SubsonicService.GetRootIndex().WithErrorHandler(this).OnSuccess(SetMenuItems).Execute();
            }
            else
            {
                await ShowSettingsNotFoundDialog();
                DialogService.ShowSettings<SettingsViewModel>();
            }
        }

        public void SetMenuItems(IList<IndexItem> items)
        {
            MenuItems.AddRange(items.Select(s => s.AsMenuItemViewModel()));
        }

        public void HandleSuccess(IList<IndexItem> result)
        {
            SetMenuItems(result);
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