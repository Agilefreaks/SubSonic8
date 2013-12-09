namespace Client.Common.Results
{
    using System.Xml.Linq;
    using Client.Common.Models.Subsonic;
    using global::Common.Exceptions;
    using global::Common.Results;

    public class SubsonicApiCallErrorResponseHandler : ApiCallErrorResponseHandler<SubsonicResponse>
    {
        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        protected override string RootElementName
        {
            get { return "subsonic-response"; }
        }

        protected override void HandleErrorResponse(SubsonicResponse response)
        {
            if (response.Status == SubsonicResponseEnum.Failed.ToString().ToLowerInvariant())
            {
                throw new ApiException(response.Error);
            }
        }

        protected override XElement GetErrorElement(XContainer xDocument)
        {
            return xDocument.Element(Namespace + RootElementName);
        }
    }
}
