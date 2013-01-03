using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockGetArtistResult : IGetArtistResult
    {
        public int ExecuteCallCount { get; set; }

        public Func<ExpandedArtist> GetResultFunc { get; set; }

        public Exception Error { get; set; }

        public ISubsonicServiceConfiguration Configuration { get; private set; }

        public Func<Task<HttpStreamResult>> Response { get; set; }

        public ExpandedArtist Result { get; private set; }

        public string ViewName { get; private set; }

        public string RequestUrl { get; private set; }

        public Task Execute(ActionExecutionContext context = null)
        {
            ExecuteCallCount++;
            var taskCompletionSource = new TaskCompletionSource<ExpandedArtist>();
            Result = GetResultFunc();
            taskCompletionSource.SetResult(Result);

            return taskCompletionSource.Task;
        }
    }
}