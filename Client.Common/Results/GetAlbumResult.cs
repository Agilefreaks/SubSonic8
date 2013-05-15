using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class GetAlbumResult : ServiceResultBase<Album>, IGetAlbumResult
    {
        public int Id { get; private set; }

        public override string ViewName { get { return "getAlbum.view"; } }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetAlbumResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Album));
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "album");
            using (var xmlReader = xElement.CreateReader())
            {
                Result = (Album)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}
