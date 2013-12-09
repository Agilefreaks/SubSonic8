namespace Common.Results
{
    using System;
    using System.Threading.Tasks;

    public abstract class ResultBase : ITaskResult
    {
        #region Public Properties

        public Exception Error { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual async Task Execute()
        {
            try
            {
                await ExecuteCore();
            }
            catch (Exception exception)
            {
                OnError(exception);
            }
        }

        #endregion

        #region Methods

        protected abstract Task ExecuteCore();

        protected virtual void OnError(Exception error)
        {
            Error = error;
        }

        #endregion
    }
}