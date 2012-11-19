using System.Threading.Tasks;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public interface ITaskResult
    {
        Task Execute(ActionExecutionContext context = null);
    }
}