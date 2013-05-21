namespace Client.Common.Tests.Results
{
    using System.IO;
    using System.Xml.Linq;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetPlaylistResultTests
    {
        #region Constants

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"> "
            + "<playlist id=\"0\" name=\"Brave New World\" owner=\"admin\" public=\"true\" songCount=\"111\" duration=\"57730\" created=\"2012-10-29T10:27:01\"> "
            + "<entry id=\"15075\" parent=\"15076\" title=\"Intro Chapter 1_1\" album=\"Brave New World - Disk 1\" artist=\"Aldous Huxley\" isDir=\"false\" coverArt=\"15076\" "
            + "created=\"2009-03-23T13:19:51\" duration=\"552\" bitRate=\"64\" track=\"1\" year=\"2003\" genre=\"Audio Book\" size=\"2267008\" suffix=\"mp3\" contentType=\"audio/mpeg\""
            + " isVideo=\"false\" path=\"Books-Comics-Tutorials/Beletristica/Audio/Aldous Huxley - Brave New World/Disk 1/01 - Aldous Huxley - Intro Chapter 1_1.mp3\" albumId=\"865\""
            + " artistId=\"323\" type=\"audiobook\" />" + "</playlist>" + "</subsonic-response>";

        #endregion

        #region Fields

        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void HandleResponse_Always_CanDeserializeAPlaylistEntryProperly()
        {
            var result = new GetPlaylistResultWrapper(new SubsonicServiceConfiguration(), 1);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().NotBeNull();
            result.Result.Entries[0].Name.Should().Be("Intro Chapter 1_1");
        }

        #endregion

        internal class GetPlaylistResultWrapper : GetPlaylistResult
        {
            #region Constructors and Destructors

            public GetPlaylistResultWrapper(ISubsonicServiceConfiguration configuration, int id)
                : base(configuration, id)
            {
            }

            #endregion

            #region Public Methods and Operators

            public void CallHandleResponse(XDocument xDocument)
            {
                HandleResponse(xDocument);
            }

            #endregion
        }
    }
}