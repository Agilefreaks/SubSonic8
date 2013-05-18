using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class DiagnosticsResult : ExtendedResultBase, IDiagnosticsResult
    {
        private Exception _lastError;

        public IList<IDiagnosticStepResult> DiagnosticSteps { get; private set; }

        public DiagnosticStepEnum? FailedStep { get; private set; }

        public DiagnosticsResult(ISubsonicServiceConfiguration configuration)
        {
            DiagnosticSteps = new List<IDiagnosticStepResult>
                {
                    new PingResult(configuration)
                };
        }

        protected override async Task ExecuteCore(ActionExecutionContext context = null)
        {
            foreach (var step in DiagnosticSteps)
            {
                await step.WithErrorHandler(this).Execute(context);
                if (_lastError == null && step.Result) continue;
                FailedStep = step.Id;
                break;
            }
        }

        public void HandleError(Exception error)
        {
            _lastError = error;
        }
    }
}