using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;
using Subsonic8.Playback;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.MusicDirectory
{
    public class MusicDirectoryViewModel : ViewModelBase
    {
        private readonly ISubsonicService _subsonicService;
        private Client.Common.Models.Subsonic.MusicDirectory _parameter;
        private BindableCollection<MenuItemViewModel> _menuItems;

        public Client.Common.Models.Subsonic.MusicDirectory Parameter
        {
            get
            {
                return _parameter;
            }
            set
            {
                if (Equals(value, _parameter)) return;
                _parameter = value;
                NotifyOfPropertyChange();
                PopulateMenuItems();
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
                if (Equals(value, _menuItems)) return;
                _menuItems = value;
                NotifyOfPropertyChange();
            }
        }

        public MusicDirectoryViewModel(INavigationService navigationService, ISubsonicService subsonicService) 
            : base(navigationService)
        {
            _subsonicService = subsonicService;
        }

        public IEnumerable<IResult> MusicDirectoryClick(ItemClickEventArgs eventArgs)
        {
            var musicDirectoryChild = (MusicDirectoryChild) ((MenuItemViewModel) eventArgs.ClickedItem).Item;
            if (musicDirectoryChild.IsDirectory)
            {
                var getMusicDirectoryResult = _subsonicService.GetMusicDirectory(musicDirectoryChild.Id);
                yield return getMusicDirectoryResult;
                NavigationService.NavigateToViewModel<MusicDirectoryViewModel>(getMusicDirectoryResult.Result);
            }
            else
            {
                NavigationService.NavigateToViewModel<PlaybackViewModel>(musicDirectoryChild);
            }
        }        

        private void PopulateMenuItems()
        {
            var menuItemViewModels = new BindableCollection<MenuItemViewModel>();
            if (Parameter != null)
            {
                foreach (var child in Parameter.Children)
                {
                    menuItemViewModels.Add(new MenuItemViewModel { Title = child.Artist, Subtitle = child.Title, Item = child });
                }
            }

            MenuItems = menuItemViewModels;
        }
    }
}