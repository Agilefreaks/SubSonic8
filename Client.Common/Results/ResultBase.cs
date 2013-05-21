namespace Client.Common.Results
{
    using System;
    using System.Threading.Tasks;
    using Caliburn.Micro;

    public abstract class ResultBase : PropertyChangedBase, ITaskResult
    {
        #region Public Properties

        public Exception Error { get; set; }

        #endregion

        #region Public Methods and Operators

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

        #endregion

        #region Methods

        protected abstract Task ExecuteCore(ActionExecutionContext context = null);

        protected virtual void OnError(Exception error)
        {
            Error = error;
        }

        #endregion
    }
}