namespace Subsonic8.Album
{
    using System.Collections.Generic;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Subsonic8.Framework.ViewModel;

    public class AlbumViewModel : DetailViewModelBase<Album>, IAlbumViewModel
    {
        #region Methods

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(Album result)
        {
            return result.Songs;
        }

        protected override IServiceResultBase<Album> GetResult(int id)
        {
            return SubsonicService.GetAlbum(id);
        }

        #endregion
    }
}