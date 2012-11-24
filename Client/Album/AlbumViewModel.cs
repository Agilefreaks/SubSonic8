using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models;
using Client.Common.Results;
using Client.Common.ViewModels;
using Subsonic8.Framework;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

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
