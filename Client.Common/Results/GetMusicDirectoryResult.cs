namespace Client.Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetMusicDirectoryResult : ServiceResultBase<MusicDirectory>, IGetMusicDirectoryResult
    {
        #region Fields

        private readonly int _id;

        #endregion

        #region Constructors and Destructors

        public GetMusicDirectoryResult(SubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            _id = id;
        }

        #endregion

        #region Public Properties

        public int Id
        {
            get
            {
                return _id;
            }
        }

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
                return "getMusicDirectory.view";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(MusicDirectory), new[] { typeof(MusicDirectoryChild) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "directory");
            using (var xmlReader = xElement.CreateReader())
            {
                Result = (MusicDirectory)xmlSerializer.Deserialize(xmlReader);
            }
        }

        #endregion
    }
}