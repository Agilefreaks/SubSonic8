namespace Subsonic8.ArtistInfo
{
    using System.Threading.Tasks;
    using global::Common.Interfaces;
    using Subsonic8.Framework.ViewModel;

    public interface IArtistInfoViewModel : IViewModel, IErrorHandler
    {
        Task Populate();

        string Parameter { get; set; }
    }
}