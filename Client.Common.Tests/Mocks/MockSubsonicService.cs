using System;
using Client.Common.Services;

namespace Client.Common.Tests.Mocks
{
    public class MockSubsonicService : SubsonicService
    {
        public int GetUriForFileWithIdCallCount { get; set; }

        public int GetUriForVideoWithIdCallCount { get; set; }

        public int GetCoverArtForIdCallCount { get; set; }
     
        public override Uri GetUriForFileWithId(int id)
        {
            GetUriForFileWithIdCallCount++;

            return new Uri(string.Format("http://subsonic.org?id={0}", id));
        }

        public override Uri GetUriForVideoWithId(int id, int timeOffset = 0)
        {
            GetUriForVideoWithIdCallCount++;

            return new Uri(string.Format("http://test.mock/{0}", id));
        }

        public override string GetCoverArtForId(string coverArt, ImageType imageType)
        {
            GetCoverArtForIdCallCount++;

            return coverArt;
        }
    }
}
