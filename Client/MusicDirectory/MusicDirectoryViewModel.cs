using Caliburn.Micro;
using Client.Common;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Subsonic8.Album;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Subsonic8.Playback;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.MusicDirectory
{
    public class MusicDirectoryViewModel : ViewModelBase, IMusicDirectoryViewModel
    {
        private ISubsonicModel _parameter;

        private BindableCollection<MenuItemViewModel> _menuItems;
        private Client.Common.Models.Subsonic.MusicDirectory _musicDirectory;

        public ISubsonicModel Parameter
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
                LoadModel();
            }
        }

        public Client.Common.Models.Subsonic.MusicDirectory MusicDirectory
        {
            get
            {
                return _musicDirectory;
            }

            set
            {
                if (Equals(value, _musicDirectory)) return;
                _musicDirectory = value;
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

        public void MusicDirectoryClick(ItemClickEventArgs eventArgs)
        {
            var navigableEntity = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByEntityType(navigableEntity);
        }

        private async void LoadModel()
        {
            var getMusicDirectory = SubsonicService.GetMusicDirectory(Parameter.Id);
            await getMusicDirectory.Execute();
            MusicDirectory = getMusicDirectory.Result;
        }

        private void PopulateMenuItems()
        {
            foreach (var child in MusicDirectory.Children)
            {
                MenuItems.Add(new MenuItemViewModel { Title = child.Artist, Subtitle = child.Title, Item = child });
            }
        }
    }
}