namespace Client.Tests.Settings
{
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework;
    using Subsonic8.Settings;
    using Windows.Security.Credentials;

    [TestClass]
    public class SettingsViewModelTests
    {
        #region Fields

        private MockNavigationService _mockNavigationService;

        private MockSettingsHelper _mockSettingsHelper;

        private MockStorageService _mockStorageService;

        private MockSubsonicService _mockSubsonicService;

        private MockToastNotificationService _mockToastNotificationService;

        private SettingsViewModel _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public async Task CanSaveChanges_PasswordEmpty_ReturnsFalse()
        {
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Password = string.Empty;

            _subject.CanSaveChanges.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanSaveChanges_UsernameAndPasswordNotEmpty_ReturnsTrue()
        {
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = "a";
            _subject.Configuration.SubsonicServiceConfiguration.Password = "a";

            _subject.CanSaveChanges.Should().BeTrue();
        }

        [TestMethod]
        public async Task CanSaveChanges_UsernameEmpty_ReturnsFalse()
        {
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = string.Empty;

            _subject.CanSaveChanges.Should().BeFalse();
        }

        [TestMethod]
        public async Task Populate_Always_CallsSettingsHelperGetCredentialsFromVault()
        {
            _mockStorageService.LoadFunc = t => null;

            await _subject.Populate();

            _mockSettingsHelper.MethodCalls.Count(c => c.Item1 == "GetCredentialsFromVault").Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_Always_CalsStorageServiceLoadWithSubsonicConfiguration()
        {
            await _subject.Populate();

            _mockStorageService.LoadCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_SettingsHelperReturnsPassworCredentials_CallsSettingsHelperGetCredentialsFromVault()
        {
            _mockStorageService.LoadFunc = t => null;
            _mockSettingsHelper.OnGetCredentialsFromVault = () => new PasswordCredential("r", "u", "p");

            await _subject.Populate();

            _subject.Configuration.SubsonicServiceConfiguration.Username.Should().Be("u");
            _subject.Configuration.SubsonicServiceConfiguration.Password.Should().Be("p");
        }

        [TestMethod]
        public async Task Populate_StorageServiceReturnsANotNullObject_SetsTheObtainedObjectAsTheCurrentConfiguration()
        {
            var configuration = new Subsonic8Configuration();
            _mockStorageService.LoadFunc = t => configuration;

            await _subject.Populate();

            _subject.Configuration.Should().Be(configuration);
        }

        [TestMethod]
        public async Task Populate_StorageServiceReturnsNull_SetsANewConfigurationAsTheCurrentConfiguration()
        {
            _mockStorageService.LoadFunc = t => null;

            await _subject.Populate();

            _subject.Configuration.Should().NotBeNull();
        }

        [TestMethod]
        public async Task Populate_StorageServiceReturnsNull_TheNewConfigurationHasToastsUseSoundFalse()
        {
            _mockStorageService.LoadFunc = t => null;

            await _subject.Populate();

            _subject.Configuration.ToastsUseSound.Should().BeFalse();
        }

        [TestMethod]
        public async Task SaveChanges_Always_CallsStorageServiceSave()
        {
            await _subject.Populate();

            _subject.SaveChanges();

            _mockStorageService.SaveCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task SaveSettings_Always_CallsSettingsHelperUpdateCredentialsInVaultWithCorrectCredentials()
        {
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = "test";
            _subject.Configuration.SubsonicServiceConfiguration.Password = "test_p";

            await _subject.SaveSettings();

            var methodCalls = _mockSettingsHelper.MethodCalls.Where(c => c.Item1 == "UpdateCredentialsInVault").ToList();
            methodCalls.Count.Should().Be(1);
            var passwordCredential = (PasswordCredential)methodCalls[0].Item2[0];
            passwordCredential.UserName.Should().Be("test");
            passwordCredential.Password.Should().Be("test_p");
            passwordCredential.Resource.Should().Be(SettingsHelper.PasswordVaultResourceName);
        }

        [TestMethod]
        public async Task SaveSettings_Always_CallsStorageServiceSave()
        {
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = "test";
            _subject.Configuration.SubsonicServiceConfiguration.Password = "test";

            await _subject.SaveSettings();

            _mockStorageService.SaveCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task SaveSettings_Always_SetsNotificationServiceToTheCurrentOption()
        {
            var configuration = new Subsonic8Configuration { ToastsUseSound = true };
            _mockStorageService.LoadFunc = t => configuration;
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = "test";
            _subject.Configuration.SubsonicServiceConfiguration.Password = "test";

            await _subject.SaveSettings();

            _mockToastNotificationService.UseSound.Should().BeTrue();
        }

        [TestMethod]
        public async Task SaveSettings_Always_SetsSubsonicServiceToTheCurrentConfiguration()
        {
            var configuration = new Subsonic8Configuration();
            _mockStorageService.LoadFunc = t => configuration;
            await _subject.Populate();
            _subject.Configuration.SubsonicServiceConfiguration.Username = "test";
            _subject.Configuration.SubsonicServiceConfiguration.Password = "test";

            await _subject.SaveSettings();

            _mockSubsonicService.Configuration.Should().Be(configuration.SubsonicServiceConfiguration);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockSubsonicService = new MockSubsonicService();
            _mockStorageService = new MockStorageService();
            _mockToastNotificationService = new MockToastNotificationService();
            _mockNavigationService = new MockNavigationService();
            _mockSettingsHelper = new MockSettingsHelper();
            _subject = new SettingsViewModel(
                _mockSubsonicService, _mockToastNotificationService, _mockStorageService, _mockNavigationService)
                           {
                               SettingsHelper
                                   =
                                   _mockSettingsHelper
                           };
        }

        #endregion
    }
}