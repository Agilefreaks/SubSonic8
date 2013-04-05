using System;
using Caliburn.Micro;
using Subsonic8.Framework.Services;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;

namespace Subsonic8.Framework.Extensions
{
    public static class SettingsPaneExtensionMethods
    {
        /// <summary>
        /// Adds an item to the Settings Pane
        /// </summary>
        /// <typeparam name="T">View model's type, inherited from Screen</typeparam>
        /// <param name="args">The settings pane commands requested event args</param>
        /// <param name="onInitialize">Method which is executed before the dialog is shown</param>
        /// <param name="onClosed">Method which is executed after the dialog has been closed</param>
        public static void AddSetting<T>(this SettingsPaneCommandsRequestedEventArgs args, Action<T> onInitialize = null, Action<T, UIElement> onClosed = null) where T : Screen
        {
            var header = IoC.Get<T>().DisplayName;

            var cmd = new SettingsCommand(header, header, command => DialogService.ShowSettings(onInitialize, onClosed));

            args.Request.ApplicationCommands.Add(cmd);
        }
    }
}