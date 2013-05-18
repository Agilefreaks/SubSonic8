using Client.Common.Results;

namespace Client.Common.Tests.Mocks
{
    public class MockPingResult : MockServiceResultBase<bool>, IPingResult
    {
        public DiagnosticStepEnum Id { get; set; }
    }
}