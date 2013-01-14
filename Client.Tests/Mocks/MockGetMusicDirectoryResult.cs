using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockGetMusicDirectoryResult : IGetMusicDirectoryResult
    {
        public int ExecuteCallCount { get; set; }

        public Func<Common.Models.Subsonic.MusicDirectory> GetResultFunc { get; set; }

        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public Common.Models.Subsonic.MusicDirectory Result { get; private set; }

        public string ViewName { get; private set; }

        public string RequestUrl { get; private set; }
        
        public Task Execute(ActionExecutionContext context = null)
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<Common.Models.Subsonic.MusicDirectory>();
            Result = GetResultFunc();
            taskCompletionSource.SetResult(Result);

            return taskCompletionSource.Task;

        }

        public IServiceResultBase<Common.Models.Subsonic.MusicDirectory> WithErrorHandler(IErrorHandler errorHandler)
        {
            return this;
        }

        public IServiceResultBase<Common.Models.Subsonic.MusicDirectory> OnSuccess(Action<Common.Models.Subsonic.MusicDirectory> onSuccess)
        {
            return this;
        }
    }
}