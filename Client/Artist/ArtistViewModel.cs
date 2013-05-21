namespace Subsonic8.Artist
{
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.MenuItem;

    public class ArtistViewModel : DetailViewModelBase<ExpandedArtist>, IArtistViewModel
    {
        #region Constructors and Destructors

        public ArtistViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        #endregion

        #region Methods

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(ExpandedArtist result)
        {
            return result.Albums;
        }

        protected override IServiceResultBase<ExpandedArtist> GetResult(int id)
        {
            return SubsonicService.GetArtist(id);
        }

        #endregion
    }
}