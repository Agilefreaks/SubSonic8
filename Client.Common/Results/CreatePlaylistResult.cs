namespace Client.Common.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class CreatePlaylistResult : EmptyResponseResultBase, ICreatePlaylistResult
    {
        #region Constructors and Destructors

        public CreatePlaylistResult(ISubsonicServiceConfiguration configuration, string name, IEnumerable<int> songIds)
            : base(configuration)
        {
            Name = name;
            SongIds = songIds;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&name=" + WebUtility.UrlEncode(Name)
                       + SongIds.Aggregate(string.Empty, (result, entry) => result + "&songId=" + entry.ToString());
            }
        }

        public IEnumerable<int> SongIds { get; private set; }

        public override string ResourcePath
        {
            get
            {
                return "createPlaylist.view";
            }
        }

        #endregion
    }
}