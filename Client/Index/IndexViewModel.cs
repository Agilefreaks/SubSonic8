using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;
using Subsonic8.MusicDirectory;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Index
{
    public class IndexViewModel : ViewModelBase
    {
        private readonly ISubsonicService _subsonicService;
        private BindableCollection<MenuItemViewModel> _menuItems;
        private IndexItem _parameter;

        public IndexItem Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                _parameter = value;
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

        public IndexViewModel(INavigationService navigationService, ISubsonicService subsonicService) 
            : base(navigationService)
        {
            _subsonicService = subsonicService;
        }

        public IEnumerable<IResult> ArtistClick(ItemClickEventArgs eventArgs)
        {
            var artist = (Artist)((MenuItemViewModel) eventArgs.ClickedItem).Item;
            var getMusicDirectoryResult = _subsonicService.GetMusicDirectory(artist.Id);
            yield return getMusicDirectoryResult;

            NavigationService.NavigateToViewModel<MusicDirectoryViewModel>(getMusicDirectoryResult.Result);
        }

        private void PopulateMenuItems()
        {
            var menuItemViewModels = new BindableCollection<MenuItemViewModel>();
            if (Parameter != null)
            {
                foreach (var artist in Parameter.Artists)
                {
                    menuItemViewModels.Add(new MenuItemViewModel { Title = artist.Name, Item = artist });
                }
            }

            MenuItems = menuItemViewModels;
        }
    }
}