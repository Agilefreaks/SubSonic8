using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;
using Subsonic8.MusicDirectory;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Index
{
    public class IndexViewModel : ViewModelBase, IIndexViewModel
    {
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

        public IndexViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        public IEnumerable<IResult> ArtistClick(ItemClickEventArgs eventArgs)
        {
            var artist = (Artist)((MenuItemViewModel) eventArgs.ClickedItem).Item;
            var getMusicDirectoryResult = SubsonicService.GetMusicDirectory(artist.Id);
            yield return getMusicDirectoryResult;

            NavigationService.NavigateToViewModel<MusicDirectoryViewModel>(getMusicDirectoryResult.Result);
        }

        private void PopulateMenuItems()
        {
            foreach (var artist in Parameter.Artists)
            {
                MenuItems.Add(new MenuItemViewModel { Title = artist.Name, Item = artist });
            }
        }
    }
}