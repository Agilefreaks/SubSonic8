namespace Client.Tests.Mocks
{
    using System;
    using Client.Common.Models;

    public class MockSubsonicModel : ISubsonicModel
    {
        #region Public Properties

        public string CoverArt { get; set; }

        public int Id { get; set; }

        public string Name { get; private set; }

        public SubsonicModelTypeEnum Type { get; private set; }

        #endregion

        #region Public Methods and Operators

        public Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>("testName", "testDescription");
        }

        #endregion
    }
}