using System.Collections.Generic;
using Client.Common.Models;
using Client.Common.Results;
using Subsonic8.Framework;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.MusicDirectory
{
    public class MusicDirectoryViewModel : DetailViewModelBase<Client.Common.Models.Subsonic.MusicDirectory>, IMusicDirectoryViewModel
    {
        protected override IServiceResultBase<Client.Common.Models.Subsonic.MusicDirectory> GetResult(int id)
        {
            return SubsonicService.GetMusicDirectory(Parameter.Id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay()
        {
            return Item.Children;
        }
    }
}