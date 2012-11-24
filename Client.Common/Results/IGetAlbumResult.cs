using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetAlbumResult : IServiceResultBase
    {
        Album Result { get; set; }
    }
}
