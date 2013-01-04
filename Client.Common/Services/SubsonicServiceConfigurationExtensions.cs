namespace Client.Common.Services
{
    public static class SubsonicServiceConfigurationExtensions
    {
         public static string RequestFormat(this ISubsonicServiceConfiguration configuration)
         {
             return configuration.BaseUrl + "rest/{0}?v=1.8.0&c=SubSonic8";
         }
    }
}