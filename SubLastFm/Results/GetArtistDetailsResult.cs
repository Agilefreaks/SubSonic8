namespace SubLastFm.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using SubLastFm.Models;

    public class GetArtistDetailsResult : LastFmResultBase<ArtistDetails>, IGetArtistDetailsResult
    {
        public GetArtistDetailsResult(IConfiguration configuration, string artistName)
            : base(configuration)
        {
            ArtistName = artistName;
        }

        public string ArtistName { get; private set; }

        public override string MethodName
        {
            get
            {
                return "artist.getInfo";
            }
        }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + string.Format("&artist={0}", ArtistName);
            }
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var element = xDocument.Element("lfm").Element("artist");
            var extraTypes = new[] { typeof(Band), typeof(BandMember), typeof(Image), typeof(TagList), typeof(Tag) };
            var xmlSerializer = new XmlSerializer(typeof(ArtistDetails), extraTypes);
            using (var xmlReader = element.CreateReader())
            {
                Result = (ArtistDetails)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}