namespace Subsonic8.Framework.ViewModel
{
    using Client.Common.Results;
    using Client.Common.Services;

    public interface ISongLoader : IErrorHandler
    {
        #region Public Properties

        ISubsonicService SubsonicService { get; set; }

        #endregion
    }
}