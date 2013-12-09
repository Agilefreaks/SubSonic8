namespace Subsonic8.Framework.Extensions
{
    using SubEchoNest.Models;
    using SubEchoNest.Results;
    using Subsonic8.ArtistInfo;

    public static class GetBiographiesResultExtensionMethods
    {
        public static BiographyInfo PreferredBiographyInfo(this IGetBiographiesResult biographiesResult)
        {
            var biographies = biographiesResult.Result;
            string text;
            string url;
            if (biographiesResult.Error == null)
            {
                Biography biography;
                if (biographies != null && ((biography = biographies.PreferredBiography) != null))
                {
                    text = biography.Text;
                    url = biography.Url;
                }
                else
                {
                    text = ArtistInfoViewModelStrings.NoBiographyFound;
                    url = null;
                }
            }
            else
            {
                text = ArtistInfoViewModelStrings.CouldNotFetchData;
                url = null;
            }

            return new BiographyInfo { Text = text, Url = url };
        }
    }
}
