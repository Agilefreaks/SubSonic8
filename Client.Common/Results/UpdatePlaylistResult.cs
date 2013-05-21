namespace Client.Common.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class UpdatePlaylistResult : EmptyResponseResultBase, IUpdatePlaylistResult
    {
        #region Constructors and Destructors

        public UpdatePlaylistResult(
            ISubsonicServiceConfiguration configuration, 
            int id, 
            IEnumerable<int> songIdsToAdd, 
            IEnumerable<int> songIndexesToRemove)
            : base(configuration)
        {
            Id = id;
            SongIdsToAdd = songIdsToAdd;
            SongIndexesToRemove = songIndexesToRemove;
        }

        #endregion

        #region Public Properties

        public int Id { get; private set; }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&playlistId=" + Id
                       + SongIdsToAdd.Aggregate(
                           string.Empty, (result, entry) => result + "&songIdToAdd=" + entry.ToString())
                       + SongIndexesToRemove.Aggregate(
                           string.Empty, (result, entry) => result + "&songIndexToRemove=" + entry.ToString());
            }
        }

        public IEnumerable<int> SongIdsToAdd { get; private set; }

        public IEnumerable<int> SongIndexesToRemove { get; private set; }

        public override string ViewName
        {
            get
            {
                return "updatePlaylist.view";
            }
        }

        #endregion
    }
}