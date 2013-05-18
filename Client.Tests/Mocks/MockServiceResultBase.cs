using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using Action = System.Action;

namespace Client.Tests.Mocks
{
    public abstract class MockServiceResultBase<T> : IServiceResultBase<T>
    {
        private Action<T> _extendedOnSuccess;
        private IErrorHandler _errorHandler;
        private Action _onSuccess;

        public int ExecuteCallCount { get; protected set; }

        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; protected set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public Func<T> GetResultFunc { get; set; }

        public Func<Exception> GetErrorFunc { get; set; }

        public T Result { get; protected set; }

        public string ViewName { get; protected set; }

        public string RequestUrl { get; protected set; }

        IExtendedResult IExtendedResult.WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        public IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
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

        public Task Execute(ActionExecutionContext context = null)
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
    }
}