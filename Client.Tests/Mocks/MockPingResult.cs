namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using global::Common.Mocks;

    public class MockPingResult : MockServiceResultBase<bool>, IPingResult
    {
        #region Public Properties

        public Error ApiError { get; set; }

        #endregion
    }
}