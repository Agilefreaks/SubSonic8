using System.Collections.Generic;
using Caliburn.Micro;

namespace Client.Common
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        IEnumerable<Models.Subsonic.Index> Result { get; set; }

        void Execute(ActionExecutionContext context);
    }
}