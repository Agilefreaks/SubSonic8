using System.Collections.Generic;
using System.Linq;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.BottomBar;
using Subsonic8.Framework.Extensions;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

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

        public async void DeletePlaylist(int id)
        {
            await SubsonicService.DeletePlaylist(id).WithErrorHandler(this).OnSuccess(result => Populate()).Execute();
        }

        protected override void OnViewLoaded(object view)
        {
            MenuItems.Clear();
            base.OnViewLoaded(view);
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

        protected override void LoadBottomBar()
        {
            BottomBar = BottomBar ?? GetPlaylistBottomBar();
        }

        private IPlaylistBottomBarViewModel GetPlaylistBottomBar()
        {
            var playlistBottomBarViewModel = IoCService.Get<IPlaylistBottomBarViewModel>();
            playlistBottomBarViewModel.DeletePlaylistAction = DeletePlaylist;

            return playlistBottomBarViewModel;
        }
    }
}