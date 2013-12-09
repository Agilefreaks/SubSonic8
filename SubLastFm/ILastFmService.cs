using SubLastFm.Results;

namespace SubLastFm
{
    public interface ILastFmService
    {
        IGetArtistDetailsResult GetArtistDetails(string artistName);
    }
}