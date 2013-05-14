using System.Linq;
using Client.Common;
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
            get { return !string.IsNullOrWhiteSpace(PlaylistName); }
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
            var oldPlaylist = MenuItems.FirstOrDefault(item => item.Title == PlaylistName);
            var songIds = PlaylistManagementService.Items.Select(ExtractId);
            if (oldPlaylist != null)
            {
                GoBack();
            }
            else
            {
                await SubsonicService.CreatePlaylist(PlaylistName, songIds)
                                     .WithErrorHandler(this)
                                     .OnSuccess(OnSaveFinished)
                                     .Execute();
            }
        }

        private void OnSaveFinished(bool result)
        {
            if (result)
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

        private static int ExtractId(Client.Common.Models.PlaylistItem item)
        {
            return int.Parse(item.Uri.ExtractParamterFromQuery("id"));
        }
    }
}