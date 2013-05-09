using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class GetPlaylistResult : ServiceResultBase<Playlist>, IGetPlaylistResult
    {
        public int Id { get; private set; }

        public override string ViewName
        {
            get { return "getPlaylist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetPlaylistResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof (Playlist), PlaylistEntry.GetXmlAttributeOverrides(),
                                                  new[] {typeof (PlaylistEntry)}, null, Namespace.NamespaceName);
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "playlist");
            using (var reader = xElement.CreateReader())
            {
                Result = (Playlist)xmlSerializer.Deserialize(reader);
            }
        }
    }
}