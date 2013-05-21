using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using Action = System.Action;

namespace Client.Common.Tests.Mocks
{
    public abstract class MockServiceResultBase<T> : IServiceResultBase<T>
    {
        private Action<T> _extendedOnSuccess;
        private Action _onSuccess;

        public IErrorHandler ErrorHandler { get; private set; }

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
            ErrorHandler = errorHandler;
            return this;
        }

        public IServiceResultBase<T> WithErrorHandler(IErrorHandler errorHandler)
        {
            ErrorHandler = errorHandler;
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
    }
}