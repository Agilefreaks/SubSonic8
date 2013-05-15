namespace Client.Common.Results
{
    public interface IDeletePlaylistResult : IServiceResultBase<bool>
    {
        int Id { get; }
    }
}