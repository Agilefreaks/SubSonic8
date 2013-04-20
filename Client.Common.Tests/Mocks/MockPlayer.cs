using System.Collections.Generic;
using Client.Common.Models;
using Client.Common.Services.DataStructures.PlayerManagementService;

namespace Client.Common.Tests.Mocks
{
    public class MockPlayer : IPlayer
    {
        private readonly List<PlaylistItem> _playCallArguments;

        public int PlayCount { get; set; }
        
        public IEnumerable<PlaylistItem> PlayCallArguments { get; private set; }

        public MockPlayer()
        {
            _playCallArguments = new List<PlaylistItem>();
            PlayCallArguments = _playCallArguments;
        }

        public void Play(PlaylistItem item)
        {
            PlayCount++;
            _playCallArguments.Add(item);
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Stop()
        {
        }
    }
}