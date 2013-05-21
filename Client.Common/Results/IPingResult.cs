namespace Client.Common.Results
{
    using Client.Common.Models.Subsonic;

    public interface IPingResult : IEmptyResponseResult
    {
        #region Public Properties

        Error ApiError { get; }

        #endregion
    }
}