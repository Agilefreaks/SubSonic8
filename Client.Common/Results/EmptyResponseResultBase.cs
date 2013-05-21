namespace Client.Common.Results
{
    using System.Xml.Linq;
    using Client.Common.Services.DataStructures.SubsonicService;

    public abstract class EmptyResponseResultBase : ServiceResultBase<bool>, IEmptyResponseResult
    {
        #region Constructors and Destructors

        protected EmptyResponseResultBase(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion

        #region Methods

        protected override void HandleResponse(XDocument xDocument)
        {
            var xElement = xDocument.Element(Namespace + "subsonic-response");
            Result = !xElement.HasElements;
        }

        #endregion
    }
}