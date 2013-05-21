namespace Subsonic8.MusicDirectory
{
    using System.Collections.Generic;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.ViewModel;

    public class MusicDirectoryViewModel : DetailViewModelBase<MusicDirectory>, IMusicDirectoryViewModel
    {
        #region Public Properties

        [Inject]
        public IStorageService StorageService { get; set; }

        #endregion

        #region Methods

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(MusicDirectory result)
        {
            return result.Children;
        }

        protected override IServiceResultBase<MusicDirectory> GetResult(int id)
        {
            return SubsonicService.GetMusicDirectory(id);
        }

        #endregion
    }
}