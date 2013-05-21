namespace Subsonic8.Framework.Services
{
    public class PlaybackNotificationOptions
    {
        #region Fields

        private string _subtitle;

        private string _title;

        #endregion

        #region Public Properties

        public string ImageUrl { get; set; }

        public string Subtitle
        {
            get
            {
                return string.IsNullOrEmpty(_subtitle) ? "Unknown" : _subtitle;
            }

            set
            {
                _subtitle = value;
            }
        }

        public string Title
        {
            get
            {
                return string.IsNullOrEmpty(_title) ? "Unknown" : _title;
            }

            set
            {
                _title = value;
            }
        }

        #endregion
    }
}