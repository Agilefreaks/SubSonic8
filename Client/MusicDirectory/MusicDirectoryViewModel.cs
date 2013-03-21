using System;
using System.Collections.Generic;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
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
            return SubsonicService.GetMusicDirectory(Parameter.Id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay()
        {
            return Item.Children;
        }

        public void LoadState(string parameter, Dictionary<string, object> statePageState)
        {
            Parameter = (GenericSubsonicModel) statePageState["Parameter"];
        }

        public void SaveState(Dictionary<string, object> statePageState, List<Type> knownTypes)
        {
            knownTypes.Add(typeof(GenericSubsonicModel));
            var genericSubsonicModel = new GenericSubsonicModel(Parameter);
            statePageState.Add("Parameter", genericSubsonicModel);
        }
    }
}