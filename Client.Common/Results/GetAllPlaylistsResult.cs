using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class GetAllPlaylistsResult : ServiceResultBase<PlaylistCollection>, IGetAllPlaylistsResult
    {
        public override string ViewName
        {
            get { return "getPlaylists.view"; }
        }

        public GetAllPlaylistsResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof (PlaylistCollection), PlaylistEntry.GetXmlAttributeOverrides(),
                                                  new[] {typeof (Playlist)}, null, Namespace.NamespaceName);
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "playlists");
            using (var reader = xElement.CreateReader())
            {
                Result = (PlaylistCollection)xmlSerializer.Deserialize(reader);
            }
        }
    }
}