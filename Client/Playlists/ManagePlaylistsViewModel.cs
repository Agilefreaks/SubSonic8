namespace Subsonic8.Playlists
{
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml.Controls;

    public class ManagePlaylistsViewModel : PlaylistViewModelBase, IManagePlaylistsViewModel
    {
        #region Constructors and Destructors

        public ManagePlaylistsViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Load remote playlist";
        }

        #endregion

        #region Public Properties

        [Inject]
        public IPlaylistManagementService PlaylistManagementService { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task ChildClick(ItemClickEventArgs eventArgs)
        {
            var subsonicModel = ((MenuItemViewModel)eventArgs.ClickedItem).Item;
            await
                SubsonicService.GetPlaylist(subsonicModel.Id)
                               .WithErrorHandler(ErrorHandler)
                               .OnSuccess(LoadPlaylist)
                               .Execute();
        }

        public void LoadPlaylist(Playlist playlist)
        {
            EventAggregator.Publish(new StopMessage());
            PlaylistManagementService.Clear();
            GoBack();
            var playlistItemCollection = new PlaylistItemCollection();
            playlistItemCollection.AddRange(playlist.Entries.Select(e => e.AsPlaylistItem(SubsonicService)));
            PlaylistManagementService.LoadPlaylist(playlistItemCollection);
            if (playlistItemCollection.Count > 0)
            {
                EventAggregator.Publish(new StartPlaybackMessage(playlistItemCollection.First()));
            }
        }

        #endregion
    }
}