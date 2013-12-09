namespace Subsonic8.ArtistInfo
{
    using System.Threading.Tasks;
    using Framework.ViewModel;
    using MugenInjection.Attributes;
    using SubEchoNest;
    using SubEchoNest.Models;
    using SubLastFm;
    using Subsonic8.Framework.Extensions;

    public class ArtistInfoViewModel : ViewModelBase, IArtistInfoViewModel
    {
        private Biographies _biographies;
        private BiographyInfo _biography;
        private string _artistImage;
        private bool _isBusy;

        public string Parameter { get; set; }

        public Biographies Biographies
        {
            get
            {
                return _biographies;
            }
            set
            {
                _biographies = value;
                NotifyOfPropertyChange();
            }
        }

        public BiographyInfo Biography
        {
            get
            {
                return _biography;
            }
            set
            {
                if (value == _biography) return;
                _biography = value;
                NotifyOfPropertyChange();
            }
        }

        public string ArtistImage
        {
            get
            {
                return _artistImage;
            }

            set
            {
                if (value == _artistImage) return;
                _artistImage = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                NotifyOfPropertyChange();
            }
        }

        [Inject]
        public ILastFmService LastFmService { get; set; }

        [Inject]
        public IEchoNestService EchoNestService { get; set; }

        public async Task Populate()
        {
            IsBusy = true;
            var getBiographiesResult = EchoNestService.GetArtistBiographies(Parameter);
            var getArtistDetailsResult = LastFmService.GetArtistDetails(Parameter);
            await Task.WhenAll(getBiographiesResult.Execute(), getArtistDetailsResult.Execute());
            Biography = getBiographiesResult.PreferredBiographyInfo();
            ArtistImage = getArtistDetailsResult.LargestImageUrl();
            IsBusy = false;
        }
    }
}