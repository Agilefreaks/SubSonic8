using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services;

namespace Client.Common.Results
{
    public class GetArtistsResult : ServiceResultBase<ExpandedArtist>, IGetArtistResult
    {
        public int Id { get; private set; }

        public override string ViewName
        {
            get { return "getArtist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetArtistsResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(ExpandedArtist), new[] { typeof(Album) });

            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "artist");
            Result = (ExpandedArtist)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}