using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public abstract class DiagnosticStepResultBase : EmptyResponseResultBase, IDiagnosticStepResult
    {
        public abstract DiagnosticStepEnum Id { get; }

        protected DiagnosticStepResultBase(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }
    }
}