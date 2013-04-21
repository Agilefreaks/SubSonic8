using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Services
{
    [TestClass]
    public class SubsonicServiceConfigurationTests
    {
        SubsonicServiceConfiguration _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new SubsonicServiceConfiguration();
        }

        [TestMethod]
        public void EncodedCredentials_Always_ReturnsTheCrendentialsEncodedInBase64()
        {
            _subject.Username = "Aladdin";
            _subject.Password = "open sesame";

            _subject.EncodedCredentials.Should().Be("QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        }

        [TestMethod]
        public void BaseUrl_IfDoesNotHaveEndingSlash_WillAddEndingSlash()
        {
            _subject.BaseUrl = "http://localhost:4040";

            _subject.BaseUrl.Should().Be("http://localhost:4040/");
        }

        [TestMethod]
        public void EncodedPassword_Always_ReturnsThePasswordHexEncoded()
        {
            _subject.Password = "sesame";

            _subject.EncodedPassword.Should().Be("enc:736573616d65");
        }
    }
}