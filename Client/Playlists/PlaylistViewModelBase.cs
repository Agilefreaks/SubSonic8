using System.Collections.Generic;
using System.Linq;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.BottomBar;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Playlists
{
    public abstract class PlaylistViewModelBase : CollectionViewModelBase<object, PlaylistCollection>, IPlaylistViewModel
    {
        public async void DeletePlaylist(int id)
        {
            await SubsonicService.DeletePlaylist(id).WithErrorHandler(this).OnSuccess(result => Populate()).Execute();
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

        private IPlaylistBottomBarViewModel GetPlaylistBottomBar()
        {
            var playlistBottomBarViewModel = IoCService.Get<IPlaylistBottomBarViewModel>();
            playlistBottomBarViewModel.DeletePlaylistAction = DeletePlaylist;

            return playlistBottomBarViewModel;
        }
    }
}