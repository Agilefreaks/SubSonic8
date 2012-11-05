using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
using Subsonic8.MenuItem;
using WinRtUtility;

namespace Subsonic8.Main
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISubsonicService _subsonicService;
        private SubsonicServiceConfiguration _serviceConfiguration;

        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MainViewModel(INavigationService navigationService, ISubsonicService subsonicService) 
            : base(navigationService)
        {
            _subsonicService = subsonicService;
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Populate().Execute();
        }

        private IEnumerable<IResult> Populate()
        {
            InitializeSubsonicService();

            var getIndexResult = _subsonicService.GetRootIndex();
            yield return getIndexResult;

            foreach (var index in getIndexResult.Result)
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
            _subsonicService.Configuration = _serviceConfiguration;
        }
    }
}