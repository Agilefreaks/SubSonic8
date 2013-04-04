using System;
using System.Collections.Generic;
using Client.Common.Models;
using Client.Common.Results;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.ViewModel;

namespace Subsonic8.MusicDirectory
{
    public class MusicDirectoryViewModel : DetailViewModelBase<Client.Common.Models.Subsonic.MusicDirectory>, IMusicDirectoryViewModel
    {
        [Inject]
        public IStorageService StorageService { get; set; }

        protected override IServiceResultBase<Client.Common.Models.Subsonic.MusicDirectory> GetResult(int id)
        {
            return SubsonicService.GetMusicDirectory(id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay()
        {
            return Item.Children;
        }

        public void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
        }

        public void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
        }
    }
}