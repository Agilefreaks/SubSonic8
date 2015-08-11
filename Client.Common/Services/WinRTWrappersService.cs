﻿namespace Client.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.ApplicationModel.Search;
    using Windows.Foundation;
    using Windows.Storage;
    using Windows.Storage.Pickers;
    using Windows.UI.ApplicationSettings;
    using Helpers;
    using MetroLog;

    public class WinRTWrappersService : IWinRTWrappersService
    {
        #region Static Fields

        private static readonly ILogger Log = LogManagerFactory.DefaultLogManager.GetLogger<WinRTWrappersService>();

        #endregion

        #region Public Methods and Operators

        public async Task<IStorageFile> GetNewStorageFile()
        {
            var fileSavePicker = new FileSavePicker();
            fileSavePicker.FileTypeChoices.Add("Subsonic8 playlist", new List<string> { ".spls" });
            fileSavePicker.DefaultFileExtension = ".spls";
            fileSavePicker.SuggestedFileName = "playlist.spls";
            fileSavePicker.SettingsIdentifier = "PlaylistPicker";

            return await fileSavePicker.PickSaveFileAsync();
        }

        public async Task<T> LoadFromFile<T>(IStorageFile storageFile) where T : new()
        {
            var randomAccessStream = await storageFile.OpenReadAsync();
            var xmlSerializer = new XmlSerializer(typeof(T));
            var result = new T();
            try
            {
                result = (T)xmlSerializer.Deserialize(randomAccessStream.AsStreamForRead());
            }
            catch (Exception exception)
            {
                Log.Error("Exception during deserializing object from file", exception);
            }
            finally
            {
                randomAccessStream.Dispose();
            }

            return result;
        }

        public async Task<IStorageFile> OpenStorageFile()
        {
            var fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add(".spls");
            fileOpenPicker.ViewMode = PickerViewMode.List;
            return await fileOpenPicker.PickSingleFileAsync();
        }

        public void RegisterMediaControlHandler(IMediaControlHandler mediaControlHandler)
        {
            mediaControlHandler.Register();
        }

        public void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler)
        {
            SearchPane.GetForCurrentView().QuerySubmitted += handler;
        }

        public void RegisterSettingsRequestedHandler(
            TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += handler;
        }

        public async Task SaveToFile<T>(IStorageFile storageFile, T @object)
        {
            var randomAccessStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
            var outputStream = randomAccessStream.GetOutputStreamAt(0);

            var xmlSerializer = new XmlSerializer(typeof(T));
            xmlSerializer.Serialize(outputStream.AsStreamForWrite(), @object);
            outputStream.Dispose();

            await randomAccessStream.FlushAsync();
            randomAccessStream.Dispose();
        }

        public void RegisterShareRequestHandler(Action<DataRequest> requestHandler)
        {
            if (requestHandler != null)
            {
                var dataTransferManager = DataTransferManager.GetForCurrentView();
                dataTransferManager.DataRequested += (sender, eventArgs) => requestHandler(eventArgs.Request);
            }
        }

        public void ShowShareUI()
        {
            DataTransferManager.ShowShareUI();
        }

        #endregion
    }
}