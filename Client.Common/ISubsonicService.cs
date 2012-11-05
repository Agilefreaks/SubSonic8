using Client.Common.Results;

namespace Client.Common
{
    public interface ISubsonicService
    {
        SubsonicServiceConfiguration Configuration { get; set; }

        IGetIndexResult GetRootIndex();
    }
}