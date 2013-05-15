namespace Client.Common.Results
{
    public interface IRenamePlaylistResult : IServiceResultBase<bool>
    {
        int Id { get; }

        string Name { get; }
    }
}