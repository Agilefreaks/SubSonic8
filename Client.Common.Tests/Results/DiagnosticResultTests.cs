using System;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using Client.Common.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class DiagnosticResultTests
    {
        DiagnosticsResult _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new DiagnosticsResult(new SubsonicServiceConfiguration());
        }

        [TestMethod]
        public void Ctor_Always_ShouldInitiaizeDiagnosticSteps()
        {
            _subject.DiagnosticSteps.Should().NotBeNull();
        }

        [TestMethod]
        public void DiagnosticSteps_Always_ShouldIncludeAPingResult()
        {
            _subject.DiagnosticSteps.Any(s => s.GetType() == typeof(PingResult)).Should().BeTrue();
        }

        [TestMethod]
        public void Di_Act_Assert()
        {
            
        }

        [TestMethod]
        public async Task Execute_Always_ShouldSetSelfAsErrorHandlerForEachDiagnosticStep()
        {
            _subject.DiagnosticSteps.Clear();
            var result = new MockPingResult();
            _subject.DiagnosticSteps.Add(result);

            await _subject.Execute();

            result.ErrorHandler.Should().Be(_subject);
        }

        [TestMethod]
        public async Task Execute_ResultsReturnTrueAndNoError_ShouldCallExecuteForEachResultInDiagnositcSteps()
        {
            _subject.DiagnosticSteps.Clear();
            var result1 = new MockPingResult { GetResultFunc = () => true };
            var result2 = new MockPingResult { GetResultFunc = () => true };
            _subject.DiagnosticSteps.Add(result1);
            _subject.DiagnosticSteps.Add(result2);

            await _subject.Execute();

            result1.ExecuteCallCount.Should().Be(1);
            result2.ExecuteCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Execute_OneDiagnosticStepFails_DoesNotRunNextSteps()
        {
            _subject.DiagnosticSteps.Clear();
            var result1 = new MockPingResult { GetErrorFunc = () => new Exception() };
            var result2 = new MockPingResult();
            _subject.DiagnosticSteps.Add(result1);
            _subject.DiagnosticSteps.Add(result2);

            await _subject.Execute();

            result1.ExecuteCallCount.Should().Be(1);
            result2.ExecuteCallCount.Should().Be(0);
        }

        [TestMethod]
        public async Task Execute_OneDiagnosticStepReturnsFalse_DoesNotRunNextSteps()
        {
            _subject.DiagnosticSteps.Clear();
            var result1 = new MockPingResult { GetResultFunc = () => false };
            var result2 = new MockPingResult();
            _subject.DiagnosticSteps.Add(result1);
            _subject.DiagnosticSteps.Add(result2);

            await _subject.Execute();

            result1.ExecuteCallCount.Should().Be(1);
            result2.ExecuteCallCount.Should().Be(0);
        }

        [TestMethod]
        public async Task Execute_OneDiagnosticStepFails_SetsFailedStepToTheFailingResultId()
        {
            _subject.DiagnosticSteps.Clear();
            var result = new MockPingResult { GetErrorFunc = () => new Exception(), Id = DiagnosticStepEnum.Ping };
            _subject.DiagnosticSteps.Add(result);

            await _subject.Execute();

            _subject.FailedStep.Should().Be(DiagnosticStepEnum.Ping);
        }
    }
}