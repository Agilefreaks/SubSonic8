using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Index;
using Subsonic8.MenuItem;
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
            if (SubsonicService.IsConfigured)
            {
                await SubsonicService.GetRootIndex().WithErrorHandler(this).OnSuccess(SetMenuItems).Execute();
            }
        }

        public void SetMenuItems(IList<IndexItem> items)
        {
            foreach (var index in items)
            {
                MenuItems.Add(new MenuItemViewModel
                                  {
                                      Title = index.Name,
                                      Subtitle = string.Format("{0} artists", index.Artists.Count),
                                      CoverArtId = index.CoverArt,
                                      Item = index
                                  });
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Populate();
        }

        public void HandleSuccess(IList<IndexItem> result)
        {
            SetMenuItems(result);
        }
    }
}