using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Subsonic8.MusicDirectory;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Index
{
    public class IndexViewModel : ViewModelBase, IIndexViewModel
    {
        private BindableCollection<MenuItemViewModel> _menuItems;
        private string _parameter;

        public string Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                _parameter = value;
                var indexItem = IndexItem.Deserialize(value);
                if (indexItem != null)
                {
                    PopulateMenuItems(indexItem);
                }
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
            var artist = (Client.Common.Models.Subsonic.Artist)((MenuItemViewModel)eventArgs.ClickedItem).Item;

            await SubsonicService.GetMusicDirectory(artist.Id)
                                 .WithErrorHandler(this)
                                 .OnSuccess(result => NavigationService.NavigateToViewModel<MusicDirectoryViewModel>(result))
                                 .Execute();
        }

        private void PopulateMenuItems(IndexItem indexItem)
        {
            foreach (var artist in indexItem.Artists)
            {
                MenuItems.Add(new MenuItemViewModel { Title = artist.Name, Item = artist });
            }
        }
    }
}