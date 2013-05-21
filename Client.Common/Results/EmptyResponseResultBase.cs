using System.Xml.Linq;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public abstract class EmptyResponseResultBase : ServiceResultBase<bool>, IEmptyResponseResult
    {
        protected EmptyResponseResultBase(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
        }
    }
}