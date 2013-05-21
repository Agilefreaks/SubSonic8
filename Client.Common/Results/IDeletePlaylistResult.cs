namespace Client.Common.Results
{
    public interface IDeletePlaylistResult : IEmptyResponseResult
    {
        int Id { get; }
    }
}