using System;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockSubsonicService : SubsonicService
    {
        private bool _hasValidSubsonicUrl;

        public int GetUriForFileWithIdCallCount { get; set; }

        public int GetUriForVideoWithIdCallCount { get; set; }

        public int GetCoverArtForIdCallCount { get; set; }

        public override bool HasValidSubsonicUrl
        {
            get { return _hasValidSubsonicUrl; }
        }

        public override Uri GetUriForFileWithId(int id)
        {
            GetUriForFileWithIdCallCount++;

            return new Uri(string.Format("http://subsonic.org?id={0}", id));
        }

        public override Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitrate = 0)
        {
            GetUriForVideoWithIdCallCount++;

            return new Uri("http://test.mock");
        }

        public override string GetCoverArtForId(string coverArt, ImageType imageType)
        {
            GetCoverArtForIdCallCount++;

            return string.Empty;
        }

        public void SetHasValidSubsonicUrl(bool value)
        {
            _hasValidSubsonicUrl = value;
        }

        public MockSubsonicService()
        {
            GetSong = id => new MockGetSongResult(id);
        }
    }
}