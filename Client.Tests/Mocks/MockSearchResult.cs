using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockSearchResult : SearchResult
    {
        public bool ExecuteCalled { get; set; }

        public MockSearchResult(ISubsonicServiceConfiguration configuration, string query)
            : base(configuration, query)
        {
            ExecuteCalled = false;
        }

        public override async Task Execute(ActionExecutionContext context = null)
        {
            await Task.Run(() => ExecuteCalled = true);
        }
    }
}