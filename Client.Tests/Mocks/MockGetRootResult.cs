using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockGetRootResult : MockServiceResultBase<IList<IndexItem>>, IGetRootResult
    {
        public MockGetRootResult()
        {
            ExecuteCallCount = 0;
            Result = new List<IndexItem>();
        }
    }
}