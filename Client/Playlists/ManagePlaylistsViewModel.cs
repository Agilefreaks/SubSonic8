using System.Collections.Generic;
using System.Linq;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Playlists
{
    public class ManagePlaylistsViewModel : CollectionViewModelBase<object, PlaylistCollection>, IManagePlaylistsViewModel
    {
        [Inject]
        public IPlaylistManagementService PlaylistManagementService { get; set; }

        public ManagePlaylistsViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Load remote playlist";
        }

        public override async void ChildClick(Windows.UI.Xaml.Controls.ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;

            await SubsonicService.GetPlaylist(subsonicModel.Id).WithErrorHandler(this).OnSuccess(LoadPlaylist).Execute();
        }

        protected override void OnViewLoaded(object view)
        {
            MenuItems.Clear();
            base.OnViewLoaded(view);
        }

        public void LoadPlaylist(Playlist playlist)
        {
            EventAggregator.Publish(new StopMessage());
            PlaylistManagementService.Clear();
            var playlistItemCollection = new PlaylistItemCollection();
            playlistItemCollection.AddRange(playlist.Entries.Select(e => e.AsPlaylistItem(SubsonicService)));
            PlaylistManagementService.LoadPlaylist(playlistItemCollection);
            GoBack();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Populate();
        }

        protected override IServiceResultBase<PlaylistCollection> GetResult(object parameter)
        {
            return SubsonicService.GetAllPlaylists();
        }

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(PlaylistCollection result)
        {
            return result.Playlists.Select(x => new GenericMediaModel(x));
        }
    }
}