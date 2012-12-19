using System;
using Client.Common.Models;

namespace Client.Tests.Mocks
{
    public class MockSubsonicModel : ISubsonicModel
    {
        public int Id { get; set; }

        public SubsonicModelTypeEnum Type { get; private set; }

        public string CoverArt { get; set; }

        public Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>("testName", "testDescription");
        }
    }
}