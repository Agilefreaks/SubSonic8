namespace Client.Tests.Mocks
{
    using Caliburn.Micro;

    public class MockSharingService : ISharingService
    {
        public int ShowShareUICallCount { get; set; }

        public void ShowShareUI()
        {
            ShowShareUICallCount++;
        }
    }
}