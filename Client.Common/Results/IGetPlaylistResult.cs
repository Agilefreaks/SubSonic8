using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetPlaylistResult : IServiceResultBase<Playlist>
    {
        int Id { get; }
    }
}