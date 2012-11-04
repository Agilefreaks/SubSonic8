using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
using Subsonic8.MenuItem;
using WinRtUtility;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Menu
{
    public class MenuViewModel : ViewModelBase
    {
        private SubsonicServiceConfiguration _serviceConfiguration;
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }        

        public MenuViewModel(INavigationService navigationService) 
            : base(navigationService)
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            MenuItems.Add(new MenuItemViewModel { Title = "Load", Subtitle = "Click to load index" });

            InitializeSubsonicService();
        }

        public IEnumerable<IResult> Click(ItemClickEventArgs eventArgs)
        {
            var subSonicService = new SubsonicService { Configuration = _serviceConfiguration };
            yield return new VisualStateResult("Loading");
            yield return subSonicService;
            yield return new VisualStateResult("LoadingComplete");

            foreach (var index in subSonicService.Result)
            {
                MenuItems.Add(new MenuItemViewModel { Title = index.Name, Subtitle = string.Format("{0} artists", index.Artists.Count) });
            }
        }

        private async void InitializeSubsonicService()
        {
            var storageHelper = new ObjectStorageHelper<SubsonicServiceConfiguration>(StorageType.Roaming);
            _serviceConfiguration = await storageHelper.LoadAsync();
#if DEBUG
            _serviceConfiguration = new SubsonicServiceConfiguration
                            {
                                ServiceUrl = "http://cristibadila.dynalias.com:33770/music/rest/{0}?u={1}&p={2}&v=1.8.0&c=SubSonic8",
                                Username = "media",
                                Password = "media"
                            };
#endif
        }
    }
}