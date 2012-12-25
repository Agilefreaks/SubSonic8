using System;
using System.IO;
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

        public Func<Task<Stream>> Response { get; set; }

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
    }
}