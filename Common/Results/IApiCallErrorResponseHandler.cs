namespace Common.Results
{
    using System.Xml.Linq;

    public interface IApiCallErrorResponseHandler
    {
        void HandleFailedCall(XContainer xDocument);
    }
}