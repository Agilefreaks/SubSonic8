using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.PlayerFramework;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace HLSPlugin
{
    public class HLSPlugin : IHLSPluign
    {
        public MediaPlayer MediaPlayer { get; set; }

        public void Load()
        {
            MediaPlayer.MediaLoading += MediaPlayerMediaLoading;
        }

        public void Unload()
        {
            MediaPlayer.MediaLoading -= MediaPlayerMediaLoading;
        }

        public void Update(IMediaSource mediaSource)
        {
        }

        private async void MediaPlayerMediaLoading(object sender, MediaPlayerDeferrableEventArgs e)
        {
            var source = MediaPlayer.Source;
            if (!source.AbsoluteUri.Contains(".m3u8")) return;

            var playlistContent = await GetPlaylistContent(source);

            var baseUri = source.AbsoluteUri.Substring(0, source.AbsoluteUri.IndexOf("/rest/", StringComparison.Ordinal));

            var playlistPlugin = MediaPlayer.GetPlaylistPlugin();
            playlistPlugin.Playlist = GetPlaylistItems(playlistContent, baseUri);
            playlistPlugin.GoToNextPlaylistItem();
            MediaPlayer.Play();
        }

        private static ObservableCollection<PlaylistItem> GetPlaylistItems(string playlistContent, string baseUri)
        {
            var regex = new Regex("/stream/.*", RegexOptions.Multiline);
            var playlistItems = new ObservableCollection<PlaylistItem>();
            foreach (var match in regex.Matches(playlistContent).Cast<Match>().Select<Match, Group>(m => m.Groups[0]))
            {
                playlistItems.Add(new PlaylistItem { AutoPlay = true, Source = new Uri(baseUri + match.Value) });
            }

            return playlistItems;
        }

        private static async Task<string> GetPlaylistContent(Uri source)
        {
            const string destination = "playlist.m3u8";
            var folder = ApplicationData.Current.LocalFolder;
            var destinationFile = await folder.CreateFileAsync(destination, CreationCollisionOption.GenerateUniqueName);
            var backgroundDownloader = new BackgroundDownloader();
            var download = backgroundDownloader.CreateDownload(source, destinationFile);
            await download.StartAsync().AsTask();

            return await FileIO.ReadTextAsync(destinationFile);
        }
    }
}
