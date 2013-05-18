namespace Client.Common.Results
{
    public interface IDiagnosticStepResult : IEmptyResponseResult
    {
        DiagnosticStepEnum Id { get; }
    }
}