using System.Net;
using System.Xml.Linq;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class RenamePlaylistResult : ServiceResultBase<bool>, IRenamePlaylistResult
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public override string ViewName
        {
            get { return "updatePlaylist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&playlistId=" + Id + "&name=" + WebUtility.UrlEncode(Name);
            }
        }

        public RenamePlaylistResult(ISubsonicServiceConfiguration configuration, int id, string name)
            : base(configuration)
        {
            Id = id;
            Name = name;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
        }
    }
}