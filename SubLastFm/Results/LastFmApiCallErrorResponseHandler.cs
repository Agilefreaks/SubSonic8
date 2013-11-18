namespace SubLastFm.Results
{
    using Common.Exceptions;
    using Common.Results;
    using SubLastFm.Models;

    public class LastFmApiCallErrorResponseHandler : ApiCallErrorResponseHandler<LastFmResponse>
    {
        protected override string RootElementName
        {
            // ReSharper disable StringLiteralTypo
            get { return "lfm"; }
            // ReSharper restore StringLiteralTypo
        }

        protected override void HandleErrorResponse(LastFmResponse response)
        {
            if (response.Status == LastFmResponseStatusEnum.Failed)
            {
                throw new ApiException(response.Error);
            }
        }
    }
}
