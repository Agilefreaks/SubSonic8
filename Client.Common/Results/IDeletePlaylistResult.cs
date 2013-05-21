namespace Client.Common.Results
{
    public interface IDeletePlaylistResult : IEmptyResponseResult
    {
        #region Public Properties

        int Id { get; }

        #endregion
    }
}