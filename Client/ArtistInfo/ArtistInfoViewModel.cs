namespace Subsonic8.ArtistInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Framework.ViewModel;
    using global::Common.Interfaces;
    using MugenInjection.Attributes;
    using SubLastFm;
    using SubLastFm.Models;
    using Subsonic8.Framework.Services;

    public class ArtistInfoViewModel : ViewModelBase, IArtistInfoViewModel
    {
        public const string CoverArtPlaceholder = @"/Assets/CoverArtPlaceholder.jpg";

        private ArtistDetails _artistDetails;
        private string _biography;
        private string _artistImage;
        private bool _isBusy;

        public string Parameter { get; set; }

        public ArtistDetails ArtistDetails
        {
            get
            {
                return _artistDetails;
            }
            set
            {
                _artistDetails = value;
                NotifyOfPropertyChange();
            }
        }

        public string Biography
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
        public IHtmlTransformService HtmlTransformService { get; set; }

        public async Task Populate()
        {
            IsBusy = true;
            var getArtistDetailsResult = LastFmService.GetArtistDetails(Parameter).WithErrorHandler(this);
            await getArtistDetailsResult.Execute();
            ArtistDetails = getArtistDetailsResult.Result;
            Biography = HtmlTransformService.ToText(ArtistDetails.Biography.Content);
            ArtistImage = ArtistDetails.Images.Any() ? ArtistDetails.LargestImage().UrlString : CoverArtPlaceholder;
            IsBusy = false;
        }

        public async Task HandleError(Exception error)
        {
            await NotificationService.Show(new DialogNotificationOptions
            {
                Message = "Could not get artist info",
                PossibleActions = new List<PossibleAction> { new PossibleAction("Ok", () => NavigationService.GoBack()) }
            });
        }
    }
}