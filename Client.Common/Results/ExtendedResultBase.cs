namespace Client.Common.Results
{
    using System;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Action = System.Action;

    public abstract class ExtendedResultBase : ResultBase, IExtendedResult
    {
        #region Properties

        protected IErrorHandler ErrorHandler { get; set; }

        protected Action OnSuccessAction { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task Execute(ActionExecutionContext context = null)
        {
            Exception catchedException = null;

            await new VisualStateResult("Loading").Execute();

            try
            {
                await ExecuteCore();
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

            await new VisualStateResult("LoadingComplete").Execute();
        }

        public IExtendedResult OnSuccess(Action onSuccess)
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

        public IExtendedResult WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;

            return this;
        }

        #endregion

        #region Methods

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

        #endregion
    }
}