namespace Subsonic8.Framework.Extensions
{
    using SubLastFm.Results;

    public static class GetArtistDetailsResultExtensionMethods
    {
        public const string CoverArtPlaceholder = @"/Assets/CoverArtPlaceholderLarge.jpg";

        public static string LargestImageUrl(this IGetArtistDetailsResult artistDetailsResult)
        {
            var artistDetails = artistDetailsResult.Result;
            if (artistDetailsResult.Error != null || artistDetails == null) return CoverArtPlaceholder;
            var largestImage = artistDetails.LargestImage();

            return largestImage != null ? largestImage.UrlString : CoverArtPlaceholder;
        }
    }
}
