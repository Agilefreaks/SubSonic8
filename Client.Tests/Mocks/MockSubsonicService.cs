using System;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockSubsonicService : SubsonicService
    {
        public int GetUriForFileWithIdCallCount { get; set; }

        public override Uri GetUriForFileWithId(int id)
        {
            GetUriForFileWithIdCallCount++;

            return new Uri(string.Format("http://subsonic.org?id={0}", id));
        }
    }
}
