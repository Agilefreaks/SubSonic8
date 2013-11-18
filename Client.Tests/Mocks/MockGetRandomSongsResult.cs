namespace Client.Tests.Mocks
{
    using System.Collections.Generic;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using global::Common.Mocks;

    public class MockGetRandomSongsResult : MockServiceResultBase<IList<Song>>, IGetRandomSongsResult
    {
        public MockGetRandomSongsResult(int numberOfSongs)
        {
            NumberOfSongs = numberOfSongs;
        }

        public int NumberOfSongs { get; private set; }
    }
}