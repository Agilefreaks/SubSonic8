namespace Common.Mocks
{
    using System;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Common.Results;
    using Action = System.Action;

    public abstract class MockRemoteXmlResultBase<T> : IRemoteXmlResultBase<T>
    {
        #region Fields

        private Action<T> _extendedOnSuccess;

        private Action _onSuccess;

        #endregion

        #region Public Properties

        public Exception Error { get; set; }

        public IErrorHandler ErrorHandler { get; private set; }

        public int ExecuteCallCount { get; protected set; }

        public Func<Exception> GetErrorFunc { get; set; }

        public Func<T> GetResultFunc { get; set; }

        public string RequestUrl { get; protected set; }

        public Func<Task<HttpStreamResult>> GetResourceFunc { get; set; }

        public T Result { get; protected set; }

        public string ResourcePath { get; protected set; }

        #endregion

        #region Public Methods and Operators

        public Task Execute()
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<T>();
            Result = GetResultFunc != null ? GetResultFunc() : default(T);
            Error = GetErrorFunc != null ? GetErrorFunc() : null;
            taskCompletionSource.SetResult(Result);
            if (Error != null)
            {
                if (ErrorHandler != null)
                {
                    ErrorHandler.HandleError(Error);
                }
            }
            else
            {
                if (_extendedOnSuccess != null)
                {
                    _extendedOnSuccess(Result);
                }

                if (_onSuccess != null)
                {
                    _onSuccess();
                }
            }

            return taskCompletionSource.Task;
        }

        public IRemoteXmlResultBase<T> OnSuccess(Action<T> onSuccess)
        {
            _extendedOnSuccess = onSuccess;
            return this;
        }

        public IExtendedResult OnSuccess(Action onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        public IRemoteXmlResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;
            return this;
        }

        #endregion

        #region Explicit Interface Methods

        IExtendedResult IExtendedResult.WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;
            return this;
        }

        #endregion
    }
}