using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
using Client.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Client.Menu
{
    public class MenuViewModel : ViewModelBase
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MenuViewModel(INavigationService navigationService, SubsonicService subsonicService) : base(navigationService)
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            MenuItems.Add(new MenuItemViewModel { Title = "Load", Subtitle = "Click to load index" });
        }

        public IEnumerable<IResult> Click(ItemClickEventArgs eventArgs)
        {
            var subSonicService = new SubsonicService();
            yield return new VisualStateResult("Loading");
            yield return subSonicService;
            yield return new VisualStateResult("LoadingComplete");

            foreach (var index in subSonicService.Result)
            {
                MenuItems.Add(new MenuItemViewModel { Title = index.Name, Subtitle = string.Format("{0} artists", index.Artists.Count) });
            }
        }
    }
}