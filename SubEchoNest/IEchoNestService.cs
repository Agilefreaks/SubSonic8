namespace SubEchoNest
{
    using SubEchoNest.Results;

    public interface IEchoNestService
    {
        IGetBiographiesResult GetArtistBiographies(string artistName);
    }
}