namespace SubEchoNest.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using SubEchoNest;
    using SubEchoNest.Models;

    public class GetBiographiesResult : EchoNestResultBase<Biographies>, IGetBiographiesResult
    {
        public GetBiographiesResult(IConfiguration configuration, string artistName)
            : base(configuration)
        {
            ArtistName = artistName;
        }

        public string ArtistName { get; private set; }

        public override string MethodName
        {
            get
            {
                return "artist/biographies";
            }
        }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + string.Format("&name={0}&license=cc-by-sa", ArtistName);
            }
        }

        public override void HandleResponse(XDocument xDocument)
        {
            var element = xDocument.Element("response").Element("biographies");
            var extraTypes = new[] { typeof(Biography) };
            var xmlSerializer = new XmlSerializer(typeof(Biographies), extraTypes);
            using (var xmlReader = element.CreateReader())
            {
                Result = (Biographies)xmlSerializer.Deserialize(xmlReader);
            }
        }
    }
}