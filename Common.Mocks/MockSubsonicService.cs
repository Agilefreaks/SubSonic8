namespace Common.Mocks
{
    using System;
    using Client.Common.Services;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class MockSubsonicService : SubsonicService
    {
        #region Public Properties

        public int GetCoverArtForIdCallCount { get; set; }

        public int GetUriForFileWithIdCallCount { get; set; }

        public int GetUriForVideoWithIdCallCount { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string GetCoverArtForId(string coverArt, ImageType imageType)
        {
            GetCoverArtForIdCallCount++;

            return coverArt;
        }

        public override Uri GetUriForFileWithId(int id)
        {
            GetUriForFileWithIdCallCount++;

            return new Uri(string.Format("http://subsonic.org?id={0}", id));
        }

        public override Uri GetUriForVideoWithId(int id, int timeOffset = 0, int maxBitrate = 0)
        {
            GetUriForVideoWithIdCallCount++;

            return new Uri(string.Format("http://test.mock/{0}", id));
        }

        #endregion
    }
}