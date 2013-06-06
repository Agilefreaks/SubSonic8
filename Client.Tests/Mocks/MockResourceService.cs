namespace Client.Tests.Mocks
{
    using System;
    using Subsonic8.Framework.Services;

    public class MockResourceService : IResourceService
    {
        #region Public Properties

        public Func<string, string> GetStringResourceFunc { get; set; }

        #endregion

        #region Public Methods and Operators

        public string GetStringResource(string resourceName)
        {
            return GetStringResourceFunc == null ? resourceName : GetStringResourceFunc(resourceName);
        }

        #endregion
    }
}