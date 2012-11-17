using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Index;
using Subsonic8.MenuItem;
using WinRtUtility;
using Windows.UI.Xaml.Controls;

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

        public void IndexClick(ItemClickEventArgs eventArgs)
        {
            NavigationService.NavigateToViewModel<IndexViewModel>(((MenuItemViewModel)eventArgs.ClickedItem).Item as IndexItem);
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

            if (getIndexResult.Error != null)
            {
                yield return new MessageDialogResult(getIndexResult.Error.ToString(), "This is a sad day");
            }
            else
            {
                foreach (var index in getIndexResult.Result)
                {
                    MenuItems.Add(new MenuItemViewModel { Title = index.Name, Subtitle = string.Format("{0} artists", index.Artists.Count), Item = index });
                }                
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