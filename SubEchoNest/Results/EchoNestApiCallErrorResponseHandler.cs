namespace SubEchoNest.Results
{
    using Common.Exceptions;
    using Common.Results;
    using SubEchoNest.Models;

    public class EchoNestApiCallErrorResponseHandler : ApiCallErrorResponseHandler<EchoNestResponse>
    {
        protected override void HandleErrorResponse(EchoNestResponse response)
        {
            if (response.Status.RequestStatus != EchoNestStatusEnum.Success)
            {
                throw new ApiException(response.Status);
            }
        }
    }
}
