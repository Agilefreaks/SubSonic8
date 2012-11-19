﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class GetRootResultTests
    {
        private GetRootResult _subject;
        private ISubsonicServiceConfiguration _subsonicServiceConfiguration;

        [TestInitialize]
        public void Setup()
        {
            _subsonicServiceConfiguration = new SubsonicServiceConfiguration();
            _subject = new GetRootResult(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void CtorShouldSetConfiguration()
        {
            _subject.Configuration.Should().BeSameAs(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void ViewNameShoulBeCorrect()
        {
            _subject.ViewName.Should().Be("getIndexes.view");
        }

        [TestMethod]
        public async Task ExecuteShouldHandleHttpRequestException()
        {
            _subject.Response = () =>
                                    {
                                        var tcr = new TaskCompletionSource<Stream>();
                                        tcr.SetException(new HttpRequestException());
                                        return tcr.Task;
                                    };
            
            await Task.Run(() => _subject.Execute(new ActionExecutionContext()));

            _subject.Error.Should().BeOfType<HttpRequestException>();
        }

        [TestMethod]
        public void HandleResponseShouldDoTheBuggy()
        {
            // TODO: pending
        }
    }
}