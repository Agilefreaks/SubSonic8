using System.Threading.Tasks;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Settings;

namespace Client.Tests.Settings
{
    [TestClass]
    public class SettingsViewModelTests
    {
        SettingsViewModel _subject;
        private MockSubsonicService _mockSubsonicService;
        private MockStorageService _mockStorageService;
        private MockToastNotificationService _mockToastNotificationService;
        private MockNavigationService _mockNavigationService;

        [TestInitialize]
        public void Setup()
        {
            _mockSubsonicService = new MockSubsonicService();
            _mockStorageService = new MockStorageService();
            _mockToastNotificationService = new MockToastNotificationService();
            _mockNavigationService = new MockNavigationService();
            _subject = new SettingsViewModel(_mockSubsonicService, _mockToastNotificationService, _mockStorageService, _mockNavigationService);
        }

        [TestMethod]
        public async Task Populate_Always_CalsStorageServiceLoadWithSubsonicConfiguration()
        {
            await _subject.Populate();

            _mockStorageService.LoadCallCount.Should().Be(1);
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
        public async Task SaveSettings_Always_CallsStorageServiceSave()
        {
            await _subject.Populate();

            await _subject.SaveSettings();

            _mockStorageService.SaveCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task SaveSettings_Always_SetsSubsonicServiceToTheCurrentConfiguration()
        {
            var configuration = new Subsonic8Configuration();
            _mockStorageService.LoadFunc = t => configuration;
            await _subject.Populate();

            await _subject.SaveSettings();

            _mockSubsonicService.Configuration.Should().Be(configuration.SubsonicServiceConfiguration);
        }

        [TestMethod]
        public async Task SaveSettings_Always_SetsNotificationServiceToTheCurrentOption()
        {
            var configuration = new Subsonic8Configuration { ToastsUseSound = true };
            _mockStorageService.LoadFunc = t => configuration;
            await _subject.Populate();

            await _subject.SaveSettings();

            _mockToastNotificationService.UseSound.Should().BeTrue();
        }
    }
}