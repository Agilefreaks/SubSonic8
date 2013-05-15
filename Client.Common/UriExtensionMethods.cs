using Windows.Foundation;

namespace Client.Common
{
    public static class UriExtensionMethods
    {
        public static string ExtractParamterFromQuery(this System.Uri uri, string parameterName)
        {
            var urlDecoder = new WwwFormUrlDecoder(uri.Query);

            return urlDecoder.GetFirstValueByName(parameterName);
        }
    }
}