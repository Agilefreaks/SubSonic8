namespace Common.Results
{
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public abstract class ApiCallErrorResponseHandler<TError> : IApiCallErrorResponseHandler
    {
        protected virtual string RootElementName
        {
            get
            {
                return "result";
            }
        }

        public virtual void HandleFailedCall(XContainer xDocument)
        {
            var xElement = GetErrorElement(xDocument);
            var xmlSerializer = new XmlSerializer(typeof(TError));
            using (var xmlReader = xElement.CreateReader())
            {
                var response = (TError)xmlSerializer.Deserialize(xmlReader);
                HandleErrorResponse(response);
            }
        }

        protected abstract void HandleErrorResponse(TError response);

        protected virtual XElement GetErrorElement(XContainer xDocument)
        {
            return xDocument.Element(RootElementName);
        }
    }
}