namespace Subsonic8.Playlists
{
    using System.Collections.Generic;
    using System.Linq;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Subsonic8.BottomBar;
    using Subsonic8.Framework.ViewModel;

    public abstract class PlaylistViewModelBase : CollectionViewModelBase<object, PlaylistCollection>, 
                                                  IPlaylistViewModel
    {
        #region Methods

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(PlaylistCollection result)
        {
            return result.Playlists.Select(x => new GenericMediaModel(x));
        }

        protected override IServiceResultBase<PlaylistCollection> GetResult(object parameter)
        {
            return SubsonicService.GetAllPlaylists();
        }

        protected override void LoadBottomBar()
        {
            BottomBar = BottomBar ?? GetPlaylistBottomBar();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Populate();
        }

        protected override void OnViewLoaded(object view)
        {
            MenuItems.Clear();
            base.OnViewLoaded(view);
        }

        private IPlaylistBottomBarViewModel GetPlaylistBottomBar()
        {
            var playlistBottomBarViewModel = IoCService.Get<IPlaylistBottomBarViewModel>();
            playlistBottomBarViewModel.OnPlaylistDeleted = Populate;

            return playlistBottomBarViewModel;
        }

        #endregion
    }
}