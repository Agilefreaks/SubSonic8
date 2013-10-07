namespace Client.Tests.Mocks
{
    using System;
    using System.Threading.Tasks;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using global::Common.Interfaces;
    using global::Common.Results;
    using Action = System.Action;

    public abstract class MockServiceResultBase<T> : IServiceResultBase<T>
    {
        #region Fields

        private IErrorHandler _errorHandler;

        private Action<T> _extendedOnSuccess;

        private Action _onSuccess;

        #endregion

        #region Public Properties

        public ISubsonicServiceConfiguration Configuration { get; protected set; }

        public Exception Error { get; set; }

        public int ExecuteCallCount { get; protected set; }

        public Func<Exception> GetErrorFunc { get; set; }

        public Func<T> GetResultFunc { get; set; }

        public string RequestUrl { get; protected set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public T Result { get; protected set; }

        public string ViewName { get; protected set; }

        #endregion

        #region Public Methods and Operators

        public Task Execute()
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<T>();
            Result = GetResultFunc != null ? GetResultFunc() : default(T);
            Error = GetErrorFunc != null ? GetErrorFunc() : null;
            taskCompletionSource.SetResult(Result);
            if (_errorHandler != null && Error != null)
            {
                _errorHandler.HandleError(Error);
            }

            if (_extendedOnSuccess != null)
            {
                _extendedOnSuccess(Result);
            }

            if (_onSuccess != null)
            {
                _onSuccess();
            }

            return taskCompletionSource.Task;
        }

        public IServiceResultBase<T> OnSuccess(Action<T> onSuccess)
        {
            _extendedOnSuccess = onSuccess;
            return this;
        }

        public IExtendedResult OnSuccess(Action onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        public IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        #endregion

        #region Explicit Interface Methods

        IExtendedResult IExtendedResult.WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        #endregion
    }
}