using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class CreatePlaylistResult : ServiceResultBase<bool>, ICreatePlaylistResult
    {
        public string Name { get; private set; }

        public IEnumerable<int> SongIds { get; private set; }

        public override string ViewName
        {
            get { return "createPlaylist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&name=" + WebUtility.UrlEncode(Name) +
                       SongIds.Aggregate(string.Empty, (result, entry) => result + "&songId=" + entry.ToString());
            }
        }

        public CreatePlaylistResult(ISubsonicServiceConfiguration configuration, string name, IEnumerable<int> songIds)
            : base(configuration)
        {
            Name = name;
            SongIds = songIds;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
        }
    }
}