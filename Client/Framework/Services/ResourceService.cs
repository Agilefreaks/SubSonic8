namespace Subsonic8.Framework.Services
{
    using Windows.ApplicationModel.Resources.Core;

    public class ResourceService : IResourceService
    {
        public string GetStringResource(string resourceName)
        {
            var resMap = ResourceManager.Current.MainResourceMap;
            return resMap.GetValue(resourceName).ValueAsString;
        }
    }
}