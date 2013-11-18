namespace Client.Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetAlbumResult : ServiceResultBase<Album>, IGetAlbumResult
    {
        #region Constructors and Destructors

        public GetAlbumResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }

        #endregion

        #region Public Properties

        public int Id { get; private set; }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public override string ResourcePath
        {
            get
            {
                return "getAlbum.view";
            }
        }

        #endregion

        #region Methods

        public override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Album));
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "album");
            using (var xmlReader = xElement.CreateReader())
            {
                Result = (Album)xmlSerializer.Deserialize(xmlReader);
            }
        }

        #endregion
    }
}