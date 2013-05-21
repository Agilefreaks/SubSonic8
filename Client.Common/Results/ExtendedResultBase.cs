using System;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public abstract class ExtendedResultBase : ResultBase, IExtendedResult
    {
        protected IErrorHandler ErrorHandler;
        protected System.Action OnSuccessAction;

        public override async Task Execute(ActionExecutionContext context = null)
        {
            Exception catchedException = null;
            try
            {
                await new VisualStateResult("Loading").Execute();

                await ExecuteCore();

                await new VisualStateResult("LoadingComplete").Execute();
            }
            catch (Exception exception)
            {
                catchedException = exception;
            }
            finally
            {
                if (catchedException != null)
                {
                    OnError(catchedException);
                }
                else
                {
                    ExecuteOnSuccessAction();
                }
            }
        }

        public IExtendedResult WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;

            return this;
        }

        public IExtendedResult OnSuccess(System.Action onSuccess)
        {
            if (OnSuccessAction != null)
            {
                var oldOnSuccess = OnSuccessAction;
                OnSuccessAction = () =>
                {
                    oldOnSuccess();
                    onSuccess();
                };
            }
            else
            {
                OnSuccessAction = onSuccess;
            }

            return this;
        }

        protected virtual void ExecuteOnSuccessAction()
        {
            if (OnSuccessAction != null)
            {
                OnSuccessAction();
            }
        }

        protected override void OnError(Exception error)
        {
            base.OnError(error);
            HandleError();
        }

        private void HandleError()
        {
            if (ErrorHandler != null)
            {
                ErrorHandler.HandleError(Error);
            }
        }
    }
}