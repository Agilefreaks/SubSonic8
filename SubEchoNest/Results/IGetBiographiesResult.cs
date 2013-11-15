namespace SubEchoNest.Results
{
    using SubEchoNest.Models;

    public interface IGetBiographiesResult : IEchoNestResultBase<Biographies>
    {
        string ArtistName { get; }
    }
}