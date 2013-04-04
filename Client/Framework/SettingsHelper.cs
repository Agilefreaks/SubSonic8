using System.Threading.Tasks;
using Client.Common.Services;
using MugenInjection.Attributes;
using Subsonic8.Framework.Interfaces;
using Subsonic8.Framework.Services;
using Subsonic8.Settings;

namespace Subsonic8.Framework
{
    public class SettingsHelper : ISettingsHelper
    {
        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        [Inject]
        public IStorageService StorageService { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

         public async Task LoadSettings()
         {
             var subsonic8Configuration = await GetSubsonic8Configuration();

             SubsonicService.Configuration = subsonic8Configuration.SubsonicServiceConfiguration;

             ToastNotificationService.UseSound = subsonic8Configuration.ToastsUseSound;
         }

         private async Task<Subsonic8Configuration> GetSubsonic8Configuration()
         {
             var subsonic8Configuration = await StorageService.Load<Subsonic8Configuration>() ?? new Subsonic8Configuration();

             return subsonic8Configuration;
         }
    }
}