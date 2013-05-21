namespace Client.Tests.Mocks
{
    using Client.Common.Results;

    public class MockRenamePlaylistResult : MockServiceResultBase<bool>, IRenamePlaylistResult
    {
        #region Public Properties

        public int Id { get; private set; }

        public string Name { get; private set; }

        #endregion
    }
}