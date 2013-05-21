namespace Client.Common.Tests.Mocks
{
    using System.Collections.Generic;
    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlayerManagementService;

    public class MockPlayer : IPlayer
    {
        #region Fields

        private readonly List<PlaylistItem> _playCallArguments;

        #endregion

        #region Constructors and Destructors

        public MockPlayer()
        {
            _playCallArguments = new List<PlaylistItem>();
            PlayCallArguments = _playCallArguments;
        }

        #endregion

        #region Public Properties

        public IEnumerable<PlaylistItem> PlayCallArguments { get; private set; }

        public int PlayCount { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Pause()
        {
        }

        public void Play(PlaylistItem item, object options = null)
        {
            PlayCount++;
            _playCallArguments.Add(item);
        }

        public void Resume()
        {
        }

        public void Stop()
        {
        }

        #endregion
    }
}