namespace Client.Tests.Mocks
{
    using System;
    using Subsonic8.Framework.Services;

    public class MockResourceService : IResourceService
    {
        public Func<string, string> GetStringResourceFunc { get; set; }

        public string GetStringResource(string resourceName)
        {
            return GetStringResourceFunc == null ? resourceName : GetStringResourceFunc(resourceName);
        }
    }
}