namespace Subsonic8.ArtistInfo
{
    using System.Threading.Tasks;
    using Subsonic8.Framework.ViewModel;

    public interface IArtistInfoViewModel : IViewModel
    {
        Task Populate();

        string Parameter { get; set; }
    }
}