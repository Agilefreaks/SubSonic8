using System.Collections.Generic;
using Client.Common.Models;
using Client.Common.Results;
using Subsonic8.Framework;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.Album
{
    public class AlbumViewModel : DetailViewModelBase<Client.Common.Models.Subsonic.Album>, IAlbumViewModel
    {
        protected override IServiceResultBase<Client.Common.Models.Subsonic.Album> GetResult(int id)
        {
            return SubsonicService.GetAlbum(Parameter.Id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay()
        {
            return Item.Songs;
        }
    }
}
