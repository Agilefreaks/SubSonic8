namespace Subsonic8.Framework.Services
{
    public class PlaybackNotificationOptions
    {
        private string _title;
        private string _subtitle;

        public string Title
        {
            get { return string.IsNullOrEmpty(_title) ? "Unknown" : _title; }
            set { _title = value; }
        }

        public string Subtitle
        {
            get { return string.IsNullOrEmpty(_subtitle) ? "Unknown" : _subtitle; }
            set { _subtitle = value; }
        }

        public string ImageUrl { get; set; }
    }
}
