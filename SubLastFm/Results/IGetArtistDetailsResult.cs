namespace SubLastFm.Results
{
    using SubLastFm.Models;

    public interface IGetArtistDetailsResult : ILastFmResultBase<ArtistDetails>
    {
        string ArtistName { get; }
    }
}