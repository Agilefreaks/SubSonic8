using System.Linq;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Playlists
{
    public class ManagePlaylistsViewModel : PlaylistViewModelBase, IManagePlaylistsViewModel
    {
        [Inject]
        public IPlaylistManagementService PlaylistManagementService { get; set; }

        public ManagePlaylistsViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Load remote playlist";
        }

        public override async void ChildClick(ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;
            await SubsonicService.GetPlaylist(subsonicModel.Id).WithErrorHandler(this).OnSuccess(LoadPlaylist).Execute();
        }

        public void LoadPlaylist(Playlist playlist)
        {
            EventAggregator.Publish(new StopMessage());
            PlaylistManagementService.Clear();
            GoBack();
            var playlistItemCollection = new PlaylistItemCollection();
            playlistItemCollection.AddRange(playlist.Entries.Select(e => e.AsPlaylistItem(SubsonicService)));
            PlaylistManagementService.LoadPlaylist(playlistItemCollection);
        }
    }
}