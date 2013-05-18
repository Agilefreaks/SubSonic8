using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class PingResult : DiagnosticStepResultBase, IPingResult
    {
        public override string ViewName
        {
            get { return "ping.view"; }
        }

        public override DiagnosticStepEnum Id
        {
            get { return DiagnosticStepEnum.Ping; }
        }

        public PingResult(ISubsonicServiceConfiguration configuration)
            : base(configuration)
        {
        }
    }
}