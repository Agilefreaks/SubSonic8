using System.Linq;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Album
{
    public class AlbumViewModel : ViewModelBase, IAlbumViewModel
    {
        private ISubsonicModel _parameter;
        private Client.Common.Models.Subsonic.Album _album;

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

        public Client.Common.Models.Subsonic.Album Album 
        {
            get
            {
                return _album;
            }
             
            set
            {
                if (Equals(value, _album)) return;
                _album = value;
                NotifyOfPropertyChange();
                PopulateMenuItems();
            }
        }

        public BindableCollection<MenuItemViewModel> MenuItems { get; set; }

        private async void LoadModel()
        {
            var getAlbumResult = SubsonicService.GetAlbum(Parameter.Id);
            await getAlbumResult.Execute();
            Album = getAlbumResult.Result;
        }

        public AlbumViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        private void PopulateMenuItems()
        {
            MenuItems.AddRange(Album.Songs.Select(s => s.AsMenuItemViewModel()));
        }

        public void SongClick(ItemClickEventArgs eventArgs)
        {
            var navigableEntity = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            NavigationService.NavigateByEntityType(navigableEntity);
        }       
    }
}
