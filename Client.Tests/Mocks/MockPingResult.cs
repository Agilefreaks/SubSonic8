namespace Client.Tests.Mocks
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;

    public class MockPingResult : MockServiceResultBase<bool>, IPingResult
    {
        #region Public Properties

        public Error ApiError { get; set; }

        #endregion
    }
}