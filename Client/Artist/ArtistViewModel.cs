using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Artist
{
    public class ArtistViewModel : DetailViewModelBase<ExpandedArtist>, IArtistViewModel
    {
        public ArtistViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override IServiceResultBase<ExpandedArtist> GetResult(int id)
        {
            return SubsonicService.GetArtist(id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay(ExpandedArtist result)
        {
            return result.Albums;
        }
    }
}