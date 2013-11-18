namespace Client.Common.Results
{
    using Client.Common.Services.DataStructures.SubsonicService;

    public class DeletePlaylistResult : EmptyResponseResultBase, IDeletePlaylistResult
    {
        #region Constructors and Destructors

        public DeletePlaylistResult(ISubsonicServiceConfiguration configuration, int id)
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
                return "deletePlaylist.view";
            }
        }

        #endregion
    }
}