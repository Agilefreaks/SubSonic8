namespace Client.Tests.Mocks
{
    using System.Collections.Generic;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;

    public class MockGetRandomSongsResult : MockServiceResultBase<IList<Song>>, IGetRandomSongsResult
    {
        public MockGetRandomSongsResult(int numberOfSongs)
        {
            NumberOfSongs = numberOfSongs;
        }

        public int NumberOfSongs { get; private set; }
    }
}