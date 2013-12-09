namespace Subsonic8
{
    using Subsonic8.Playback;

    public class StringResources
    {
        private static readonly PlaybackViewModelStrings PlaybackViewModelStringsInstance = new PlaybackViewModelStrings();

        public PlaybackViewModelStrings PlaybackViewModelStrings
        {
            get
            {
                return PlaybackViewModelStringsInstance;
            }
        }
    }
}