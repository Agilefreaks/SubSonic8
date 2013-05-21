namespace Client.Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetSongResult : ServiceResultBase<Song>, IGetSongResult
    {
        #region Constructors and Destructors

        public GetSongResult(ISubsonicServiceConfiguration configuration, int id)
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

        public override string ViewName
        {
            get
            {
                return "getSong.view";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Song));
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "song");
            using (var xmlReader = xElement.CreateReader())
            {
                Result = (Song)xmlSerializer.Deserialize(xmlReader);
            }
        }

        #endregion
    }
}