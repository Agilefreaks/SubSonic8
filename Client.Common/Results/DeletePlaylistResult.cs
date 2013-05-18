using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class DeletePlaylistResult : EmptyResponseResultBase, IDeletePlaylistResult
    {
        public int Id { get; private set; }

        public override string ViewName
        {
            get { return "deletePlaylist.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public DeletePlaylistResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            Id = id;
        }
    }
}