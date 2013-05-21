namespace Client.Common.Results
{
    using Client.Common.Models.Subsonic;

    public interface IGetPlaylistResult : IServiceResultBase<Playlist>
    {
        #region Public Properties

        int Id { get; }

        #endregion
    }
}