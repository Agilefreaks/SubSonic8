namespace Client.Tests.Mocks
{
    using System.Collections.Generic;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using global::Common.Mocks;

    public class MockGetRootResult : MockServiceResultBase<IList<MusicFolder>>, IGetRootResult
    {
        #region Constructors and Destructors

        public MockGetRootResult()
        {
            ExecuteCallCount = 0;
            Result = new List<MusicFolder>();
        }

        #endregion
    }
}