namespace Client.Common.Results
{
    public interface IRenamePlaylistResult : IServiceResultBase<bool>
    {
        #region Public Properties

        int Id { get; }

        string Name { get; }

        #endregion
    }
}