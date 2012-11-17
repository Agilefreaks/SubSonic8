using System;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public abstract class ResultBase : PropertyChangedBase, IResultBase
    {
        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public Exception Error { get; set; }

        protected virtual void OnCompleted()
        {
            OnCompleted(new ResultCompletionEventArgs());
        }

        protected virtual void OnError(Exception error)
        {
            Error = error;
            OnCompleted();
        }

        protected virtual void OnCancelled()
        {
            OnCompleted(new ResultCompletionEventArgs
            {
                WasCancelled = true
            });
        }

        protected virtual void OnCompleted(ResultCompletionEventArgs e)
        {
            Caliburn.Micro.Execute.OnUIThread(() => Completed(this, e));
        }

        public abstract void Execute(ActionExecutionContext context);
    }
}