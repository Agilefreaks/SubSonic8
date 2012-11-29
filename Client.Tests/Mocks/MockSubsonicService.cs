using System;
using Client.Common.Services;

namespace Client.Tests.Mocks
{
    public class MockSubsonicService : SubsonicService
    {
        public override Uri GetUriForFileWithId(int id)
        {
            return new Uri(string.Format("http://subsonic.org?id={0}", id));
        }
    }
}
