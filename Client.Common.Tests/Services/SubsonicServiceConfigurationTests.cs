namespace Client.Common.Tests.Services
{
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class SubsonicServiceConfigurationTests
    {
        #region Fields

        private SubsonicServiceConfiguration _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void SetBaseUrl_TheValuesIsAValidSubsonicUrl_SetsTheGivenValue()
        {
            _subject.BaseUrl = "https://localhost/";

            _subject.BaseUrl.Should().Be("https://localhost/");
        }

        [TestMethod]
        public void SetBaseUrl_TheValuesDoesNotHaveEndingSlash_WillAddEndingSlash()
        {
            _subject.BaseUrl = "http://localhost:4040";

            _subject.BaseUrl.Should().Be("http://localhost:4040/");
        }

        [TestMethod]
        public void SetBaseUrl_TheValuesDoesNotHaveAProtocol_WillAddHttpAsTheProtocol()
        {
            _subject.BaseUrl = "localhost:4040/";

            _subject.BaseUrl.Should().Be("http://localhost:4040/");
        }

        //[TestMethod]
        //public void SetBaseUrl_TheValuesHasAMalformedHttpProtocol_WillCorrectTheProtocol()
        //{
        //    var examples = new[]
        //                       {
        //                           "http:/localhost:4040/", "htp:/localhost:4040/", "ttp://localhost:4040/",
        //                           "htt://localhost:4040/", "tp:/localhost:4040/"
        //                       };
        //    foreach (var example in examples)
        //    {
        //        _subject.BaseUrl = example;
        //        _subject.BaseUrl.Should().Be("http://localhost:4040/");
        //    }
        //}

        //[TestMethod]
        //public void SetBaseUrl_TheValuesHasAMalformedHttpsProtocol_WillCorrectTheProtocol()
        //{
        //    var examples = new[]
        //                       {
        //                           "https:/localhost:4040/", "htps:/localhost:4040/", "ttps://localhost:4040/",
        //                           "htts://localhost:4040/", "tps:/localhost:4040/"
        //                       };
        //    foreach (var example in examples)
        //    {
        //        _subject.BaseUrl = example;
        //        _subject.BaseUrl.Should().Be("https://localhost:4040/");
        //    }
        //}

        [TestMethod]
        public void EncodedCredentials_Always_ReturnsTheCredentialsEncodedInBase64()
        {
            _subject.Username = "Aladdin";
            _subject.Password = "open sesame";

            _subject.EncodedCredentials.Should().Be("QWxhZGRpbjpvcGVuIHNlc2FtZQ==");
        }

        [TestMethod]
        public void EncodedPassword_Always_ReturnsThePasswordHexEncoded()
        {
            _subject.Password = "sesame";

            _subject.EncodedPassword.Should().Be("enc:736573616d65");
        }

        [TestInitialize]
        public void Setup()
        {
            _subject = new SubsonicServiceConfiguration();
        }

        #endregion
    }
}