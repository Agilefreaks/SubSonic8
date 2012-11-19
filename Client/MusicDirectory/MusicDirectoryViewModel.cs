using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Subsonic8.MenuItem;
using Subsonic8.Playback;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.MusicDirectory
{
    public class MusicDirectoryViewModel : ViewModelBase, IMusicDirectoryViewModel
    {
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

        public MusicDirectoryViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        public async void MusicDirectoryClick(ItemClickEventArgs eventArgs)
        {
            var musicDirectoryChild = (MusicDirectoryChild) ((MenuItemViewModel) eventArgs.ClickedItem).Item;
            
            if (musicDirectoryChild.IsDirectory)
            {
                var getMusicDirectoryResult = SubsonicService.GetMusicDirectory(musicDirectoryChild.Id);
                await getMusicDirectoryResult.Execute();
                NavigationService.NavigateToViewModel<MusicDirectoryViewModel>(getMusicDirectoryResult.Result);
            }
            else
            {
                NavigationService.NavigateToViewModel<PlaybackViewModel>(musicDirectoryChild);
            }
        }        

        private void PopulateMenuItems()
        {
            foreach (var child in Parameter.Children)
            {
                MenuItems.Add(new MenuItemViewModel { Title = child.Artist, Subtitle = child.Title, Item = child });
            }
        }
    }
}