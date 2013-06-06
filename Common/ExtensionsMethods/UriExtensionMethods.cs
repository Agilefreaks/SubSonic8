namespace Common.ExtensionsMethods
{
    using System;
    using Windows.Foundation;

    public static class UriExtensionMethods
    {
        #region Public Methods and Operators

        public static string ExtractParamterFromQuery(this Uri uri, string parameterName)
        {
            var urlDecoder = new WwwFormUrlDecoder(uri.Query);

            return urlDecoder.GetFirstValueByName(parameterName);
        }

        #endregion
    }
}