using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;
using Subsonic8.MusicDirectory;
using Windows.UI.Xaml.Controls;
using Models = Client.Common.Models.Subsonic;

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

        public async void ArtistClick(ItemClickEventArgs eventArgs)
        {
            var artist = (Models.Artist)((MenuItemViewModel) eventArgs.ClickedItem).Item;
            var getMusicDirectoryResult = SubsonicService.GetMusicDirectory(artist.Id);
            
            await getMusicDirectoryResult.Execute();

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