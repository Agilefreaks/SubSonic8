using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetMusicDirectoryResult : IServiceResultBase
    {
        MusicDirectory Result { get; set; }
    }
}