using System.Threading.Tasks;
using Client.Common.Services;
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

        [TestInitialize]
        public void Setup()
        {
            _mockSubsonicService = new MockSubsonicService();
            _mockStorageService = new MockStorageService();
            _subject = new SettingsViewModel(_mockSubsonicService, _mockStorageService);
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
            var configuration = new SubsonicServiceConfiguration();
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
        public async Task ModifyingTheConfiguration_Always_CallsStorageServiceSaveWithTheCurrentConfigurationOnceEvery400Miliseconds()
        {
            _mockStorageService.LoadFunc = t => new SubsonicServiceConfiguration();
            await _subject.Populate();

            _subject.Configuration.Username = "test1";
            _subject.Configuration.Username = "test2";
            _subject.Configuration.Username = "test3";

            await Task.Delay(500);
            _mockStorageService.SaveCallCount.Should().Be(1);
        }
    }
}