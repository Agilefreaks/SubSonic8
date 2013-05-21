using Client.Common.Models.Subsonic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockPingResult : MockServiceResultBase<bool>, IPingResult
    {
        public Error ApiError { get; set; }
    }
}