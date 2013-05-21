namespace Subsonic8.Playlists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Services;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml.Controls;

    public class SavePlaylistViewModel : PlaylistViewModelBase, ISavePlaylistViewModel
    {
        #region Fields

        private bool _canEdit;

        private string _playlistName;

        #endregion

        #region Constructors and Destructors

        public SavePlaylistViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Save remote playlist";
        }

        #endregion

        #region Public Properties

        public bool CanEdit
        {
            get
            {
                return _canEdit;
            }

            set
            {
                if (value.Equals(_canEdit))
                {
                    return;
                }

                _canEdit = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public bool CanSave
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PlaylistName) && CanEdit;
            }
        }

        [Inject]
        public IDialogNotificationService DialogNotificationService { get; set; }

        [Inject]
        public IPlaylistManagementService PlaylistManagementService { get; set; }

        public string PlaylistName
        {
            get
            {
                return _playlistName;
            }

            set
            {
                if (value == _playlistName)
                {
                    return;
                }

                _playlistName = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Cancel()
        {
            GoBack();
        }

        public override void ChildClick(ItemClickEventArgs eventArgs)
        {
            PlaylistName = ((MenuItemViewModel)eventArgs.ClickedItem).Item.Name;
        }

        public void PlaylistNameChanged(TextBox sender)
        {
            PlaylistName = sender.Text;
        }

        public async void Save()
        {
            CanEdit = false;
            var existingEntry = MenuItems.FirstOrDefault(item => item.Item.Name == PlaylistName);
            if (existingEntry != null)
            {
                await
                    SubsonicService.GetPlaylist(existingEntry.Item.Id)
                                   .WithErrorHandler(this)
                                   .OnSuccess(async result => await UpdatePlaylist(result))
                                   .Execute();
            }
            else
            {
                var songIds = GetSongIdsForActivePlaylist();
                await
                    SubsonicService.CreatePlaylist(PlaylistName, songIds)
                                   .WithErrorHandler(this)
                                   .OnSuccess(OnSaveFinished)
                                   .Execute();
            }
        }

        #endregion

        #region Methods

        protected override void OnActivate()
        {
            base.OnActivate();
            CanEdit = true;
        }

        private static int ExtractId(PlaylistItem item)
        {
            return int.Parse(item.Uri.ExtractParamterFromQuery("id"));
        }

        private IEnumerable<int> GetSongIdsForActivePlaylist()
        {
            return PlaylistManagementService.Items.Select(ExtractId);
        }

        private void OnSaveFinished(bool result)
        {
            CanEdit = true;
            if (!result)
            {
                DialogNotificationService.Show(
                    new DialogNotificationOptions { Message = "Could not save playlist. Please retry." });
            }
            else
            {
                GoBack();
            }
        }

        private async Task UpdatePlaylist(Playlist playlist)
        {
            var songIds = GetSongIdsForActivePlaylist().ToList();
            var songIdsInPlaylist = playlist.Entries.Select(entry => entry.Id).ToList();
            var songIdsToAdd = songIds.Where(songId => !songIdsInPlaylist.Contains(songId));
            var songIndexesToRemove =
                songIdsInPlaylist.Where(songId => !songIds.Contains(songId))
                                 .Select(songId => songIdsInPlaylist.IndexOf(songId));
            await
                SubsonicService.UpdatePlaylist(playlist.Id, songIdsToAdd, songIndexesToRemove)
                               .WithErrorHandler(this)
                               .OnSuccess(OnSaveFinished)
                               .Execute();
        }

        #endregion
    }
}