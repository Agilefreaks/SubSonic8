using System.Collections.Generic;
using System.Linq;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class UpdatePlaylistResult : EmptyResponseResultBase, IUpdatePlaylistResult
    {
        public int Id { get; private set; }

        public IEnumerable<int> SongIdsToAdd { get; private set; }

        public IEnumerable<int> SongIndexesToRemove { get; private set; }

        public override string ViewName
        {
            get { return "updatePlaylist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&playlistId=" + Id +
                       SongIdsToAdd.Aggregate(string.Empty,
                                              (result, entry) => result + "&songIdToAdd=" + entry.ToString()) +
                       SongIndexesToRemove.Aggregate(string.Empty,
                                                     (result, entry) =>
                                                     result + "&songIndexToRemove=" + entry.ToString());
            }
        }

        public UpdatePlaylistResult(ISubsonicServiceConfiguration configuration, int id, IEnumerable<int> songIdsToAdd, IEnumerable<int> songIndexesToRemove)
            : base(configuration)
        {
            Id = id;
            SongIdsToAdd = songIdsToAdd;
            SongIndexesToRemove = songIndexesToRemove;
        }
    }
}