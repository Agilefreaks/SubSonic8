using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Common;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Services;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Playlists
{
    public class SavePlaylistViewModel : PlaylistViewModelBase, ISavePlaylistViewModel
    {
        private string _playlistName;
        private bool _canEdit;

        public string PlaylistName
        {
            get
            {
                return _playlistName;
            }
            set
            {
                if (value == _playlistName) return;
                _playlistName = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        public bool CanSave
        {
            get { return !string.IsNullOrWhiteSpace(PlaylistName) && CanEdit; }
        }

        public bool CanEdit
        {
            get
            {
                return _canEdit;
            }
            set
            {
                if (value.Equals(_canEdit)) return;
                _canEdit = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(() => CanSave);
            }
        }

        [Inject]
        public IPlaylistManagementService PlaylistManagementService { get; set; }

        [Inject]
        public IDialogNotificationService DialogNotificationService { get; set; }

        public SavePlaylistViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Save remote playlist";
        }

        public override void ChildClick(ItemClickEventArgs eventArgs)
        {
            PlaylistName = ((MenuItemViewModel)eventArgs.ClickedItem).Item.Name;
        }

        public void Cancel()
        {
            GoBack();
        }

        public async void Save()
        {
            CanEdit = false;
            var existingEntry = MenuItems.FirstOrDefault(item => item.Item.Name == PlaylistName);
            if (existingEntry != null)
            {
                await SubsonicService.GetPlaylist(existingEntry.Item.Id)
                                     .WithErrorHandler(this)
                                     .OnSuccess(async result => await UpdatePlaylist(result))
                                     .Execute();
            }
            else
            {
                var songIds = GetSongIdsForActivePlaylist();
                await SubsonicService.CreatePlaylist(PlaylistName, songIds)
                                     .WithErrorHandler(this)
                                     .OnSuccess(OnSaveFinished)
                                     .Execute();
            }
        }

        public void PlaylistNameChanged(TextBox sender)
        {
            PlaylistName = sender.Text;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            CanEdit = true;
        }

        private static int ExtractId(Client.Common.Models.PlaylistItem item)
        {
            return int.Parse(item.Uri.ExtractParamterFromQuery("id"));
        }

        private async Task UpdatePlaylist(Playlist playlist)
        {
            var songIds = GetSongIdsForActivePlaylist().ToList();
            var songIdsInPlaylist = playlist.Entries.Select(entry => entry.Id).ToList();
            var songIdsToAdd = songIds.Where(songId => !songIdsInPlaylist.Contains(songId));
            var songIndexesToRemove = songIdsInPlaylist.Where(songId => !songIds.Contains(songId))
                                                       .Select(songId => songIdsInPlaylist.IndexOf(songId));
            await SubsonicService.UpdatePlaylist(playlist.Id, songIdsToAdd, songIndexesToRemove)
                                 .WithErrorHandler(this)
                                 .OnSuccess(OnSaveFinished).Execute();
        }

        private void OnSaveFinished(bool result)
        {
            CanEdit = true;
            if (!result)
            {
                DialogNotificationService.Show(new DialogNotificationOptions
                    {
                        Message = "Could not save playlist. Please retry."
                    });
            }
            else
            {
                GoBack();
            }
        }

        private IEnumerable<int> GetSongIdsForActivePlaylist()
        {
            return PlaylistManagementService.Items.Select(ExtractId);
        }
    }
}