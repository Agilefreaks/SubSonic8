using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Tests.Mocks
{
    public abstract class MockServiceResultBase<T> : IServiceResultBase<T>
        where T : class
    {
        private Action<T> _onSuccess;
        private IErrorHandler _errorHandler;

        public int ExecuteCallCount { get; protected set; }

        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; protected set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public Func<T> GetResultFunc { get; set; }

        public Func<Exception> GetErrorFunc { get; set; }

        public T Result { get; protected set; }

        public string ViewName { get; protected set; }

        public string RequestUrl { get; protected set; }

        public IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }

        public IServiceResultBase<T> OnSuccess(Action<T> onSuccess)
        {
            _onSuccess = onSuccess;
            return this;
        }

        public Task Execute(ActionExecutionContext context = null)
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<T>();
            Result = GetResultFunc != null ? GetResultFunc() : null;
            Error = GetErrorFunc != null ? GetErrorFunc() : null;
            taskCompletionSource.SetResult(Result);
            if (_errorHandler != null && Error != null)
            {
                _errorHandler.HandleError(Error);
            }

            if (_onSuccess != null)
            {
                _onSuccess(Result);
            }

            return taskCompletionSource.Task;

        }
    }
}