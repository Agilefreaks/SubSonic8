namespace Client.Common.Results
{
    using System.Net;
    using System.Xml.Linq;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class RenamePlaylistResult : ServiceResultBase<bool>, IRenamePlaylistResult
    {
        #region Constructors and Destructors

        public RenamePlaylistResult(ISubsonicServiceConfiguration configuration, int id, string name)
            : base(configuration)
        {
            Id = id;
            Name = name;
        }

        #endregion

        #region Public Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        public override string RequestUrl
        {
            get
            {
                return base.RequestUrl + "&playlistId=" + Id + "&name=" + WebUtility.UrlEncode(Name);
            }
        }

        public override string ResourcePath
        {
            get
            {
                return "updatePlaylist.view";
            }
        }

        #endregion

        #region Methods

        public override void HandleResponse(XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
        }

        #endregion
    }
}