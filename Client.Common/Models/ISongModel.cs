namespace Client.Common.Models
{
    public interface ISongModel : IMediaModel
    {
        string Artist { get; }

        int Duration { get; }
    }
}