namespace Common.Results
{
    using System;
    using System.Threading.Tasks;
    using Common.Interfaces;

    public abstract class ExtendedResultBase : ResultBase, IExtendedResult
    {
        #region Properties

        protected IErrorHandler ErrorHandler { get; set; }

        protected Action OnSuccessAction { get; set; }

        #endregion

        #region Public Methods and Operators

        public override async Task Execute()
        {
            Exception caughtException = null;

            await new VisualStateResult("Loading").Execute();

            try
            {
                await ExecuteCore();
            }
            catch (Exception exception)
            {
                caughtException = exception;
            }
            finally
            {
                if (caughtException != null)
                {
                    OnError(caughtException);
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