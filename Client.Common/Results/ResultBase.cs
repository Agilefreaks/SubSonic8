using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public abstract class ResultBase : PropertyChangedBase, ITaskResult
    {
        public Exception Error { get; set; }

        public virtual async Task Execute(ActionExecutionContext context = null)
        {
            try
            {
                await ExecuteCore(context);
            }
            catch (Exception exception)
            {
                OnError(exception);
            }
        }

        protected abstract Task ExecuteCore(ActionExecutionContext context = null);

        protected virtual void OnError(Exception error)
        {
            Error = error;
        }
    }
}