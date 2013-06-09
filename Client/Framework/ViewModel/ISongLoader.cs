namespace Subsonic8.Framework.ViewModel
{
    using Client.Common.Results;
    using Client.Common.Services;

    public interface ISongLoader
    {
        #region Public Properties

        IErrorHandler ErrorHandler { get; }

        ISubsonicService SubsonicService { get; set; }

        #endregion
    }
}