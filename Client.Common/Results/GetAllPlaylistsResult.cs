namespace Client.Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetAllPlaylistsResult : ServiceResultBase<PlaylistCollection>, IGetAllPlaylistsResult
    {
        #region Constructors and Destructors

        public GetAllPlaylistsResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Public Properties

        public override string ViewName
        {
            get
            {
                return "getPlaylists.view";
            }
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(PlaylistCollection), new[] { typeof(Playlist) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "playlists");
            using (var reader = xElement.CreateReader())
            {
                Result = (PlaylistCollection)xmlSerializer.Deserialize(reader);
            }
        }

        #endregion
    }
}