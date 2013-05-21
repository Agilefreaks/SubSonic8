namespace Client.Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetPlaylistResult : ServiceResultBase<Playlist>, IGetPlaylistResult
    {
        #region Constructors and Destructors

        public GetPlaylistResult(ISubsonicServiceConfiguration configuration, int id)
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
                return "getPlaylist.view";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Playlist), new[] { typeof(PlaylistEntry) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "playlist");
            using (var reader = xElement.CreateReader())
            {
                Result = (Playlist)xmlSerializer.Deserialize(reader);
            }
        }

        #endregion
    }
}