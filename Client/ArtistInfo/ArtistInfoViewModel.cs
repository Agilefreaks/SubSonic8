namespace Subsonic8.ArtistInfo
{
    using System.Threading.Tasks;
    using Framework.ViewModel;
    using MugenInjection.Attributes;
    using SubLastFm;

    public class ArtistInfoViewModel : ViewModelBase, IArtistInfoViewModel
    {
        public string Parameter { get; set; }

        [Inject]
        public ILastFmService LastFmService { get; set; }

        protected override async void OnActivate()
        {
            base.OnActivate();
            await Populate();
        }

        private async Task Populate()
        {
            var getArtistDetailsResult = LastFmService.GetArtistDetails(Parameter).WithErrorHandler(ErrorDialogViewModel);
            await getArtistDetailsResult.Execute();
        }
    }
}