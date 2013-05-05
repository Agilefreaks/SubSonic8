using System.Collections.Generic;
using System.Linq;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Playlists
{
    public class ManagePlaylistsViewModel : CollectionViewModelBase<object, PlaylistCollection>, IManagePlaylistsViewModel
    {
        public ManagePlaylistsViewModel()
        {
            UpdateDisplayName = () => DisplayName = "Open remote playlist";
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
    }
}