using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetMusicDirectoryResult : IResult
    {
        MusicDirectory Result { get; set; }
    }
}