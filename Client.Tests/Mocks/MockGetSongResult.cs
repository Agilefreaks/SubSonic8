using Client.Common.Models.Subsonic;
using Client.Common.Results;

namespace Client.Tests.Mocks
{
    public class MockGetSongResult : MockServiceResultBase<Song>, IGetSongResult
    {
        public MockGetSongResult(int id = 0)
        {
            GetResultFunc = () => new Song { Id = id };
        }
    }
}
