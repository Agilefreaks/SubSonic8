namespace Subsonic8.Framework
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Client.Common;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Interfaces;
    using Subsonic8.Framework.Services;
    using Subsonic8.Settings;
    using Windows.Security.Credentials;

    public class SettingsHelper : ISettingsHelper
    {
        #region Constants

        public const string PasswordVaultResourceName = "Subsonic8";

        #endregion

        #region Public Properties

        [Inject]
        public IStorageService StorageService { get; set; }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        [Inject]
        public IToastNotificationService ToastNotificationService { get; set; }

        #endregion

        #region Public Methods and Operators

        public PasswordCredential GetCredentialsFromVault()
        {
            var vault = new PasswordVault();
            return GetCredentialsFromVault(vault);
        }

        public async Task LoadSettings()
        {
            var subsonic8Configuration = await GetSubsonic8Configuration();

            SubsonicService.Configuration = subsonic8Configuration.SubsonicServiceConfiguration;

            ToastNotificationService.UseSound = subsonic8Configuration.ToastsUseSound;
        }

        public void UpdateCredentialsInVault(PasswordCredential passwordCredential)
        {
            var vault = new PasswordVault();
            var credentials = GetCredentialsFromVault(vault);
            if (credentials != null)
            {
                vault.Remove(credentials);
            }

            vault.Add(passwordCredential);
        }

        #endregion

        #region Methods

        private PasswordCredential GetCredentialsFromVault(PasswordVault vault)
        {
            PasswordCredential credentials;
            try
            {
                credentials = vault.FindAllByResource(PasswordVaultResourceName).FirstOrDefault();
                credentials = vault.Retrieve(PasswordVaultResourceName, credentials.UserName);
            }
            catch (Exception exception)
            {
                credentials = null;
                this.Log(exception);
            }

            return credentials;
        }

        private async Task<Tuple<string, string>> GetLegacyCredentials()
        {
            Tuple<string, string> credentials = null;
            var data = await StorageService.GetData<Subsonic8Configuration>();
            if (data != null)
            {
                var xDocument = XDocument.Parse(data);
                var configurationElement =
                    xDocument.Element("Subsonic8Configuration").Element("SubsonicServiceConfiguration");
                var usernameElement = configurationElement.Element("Username");
                var passwordElement = configurationElement.Element("Password");
                if (usernameElement != null && passwordElement != null)
                {
                    credentials = new Tuple<string, string>(usernameElement.Value, passwordElement.Value);
                }
            }

            return credentials;
        }

        private async Task<Subsonic8Configuration> GetSubsonic8Configuration()
        {
            var subsonic8Configuration = await StorageService.Load<Subsonic8Configuration>()
                                         ?? new Subsonic8Configuration();
            await SetPasswordFromVault(subsonic8Configuration);

            return subsonic8Configuration;
        }

        private async Task SetPasswordFromVault(Subsonic8Configuration appConfiguration)
        {
            var configuration = appConfiguration.SubsonicServiceConfiguration;
            var vault = new PasswordVault();
            var credentials = GetCredentialsFromVault(vault);
            if (credentials == null)
            {
                if (!string.IsNullOrWhiteSpace(configuration.BaseUrl))
                {
                    var legacyCredentials = await GetLegacyCredentials();
                    if (legacyCredentials != null)
                    {
                        var passwordCredential = new PasswordCredential(
                            PasswordVaultResourceName, legacyCredentials.Item1, legacyCredentials.Item2);
                        vault.Add(passwordCredential);
                        configuration.Username = legacyCredentials.Item1;
                        configuration.Password = legacyCredentials.Item2;
                        await StorageService.Save(appConfiguration);
                    }
                }
            }
            else
            {
                configuration.Username = credentials.UserName;
                configuration.Password = credentials.Password;
            }
        }

        #endregion
    }
}