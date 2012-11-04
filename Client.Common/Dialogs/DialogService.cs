using System;
using Callisto.Controls;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Caliburn.Micro
{
    public static class SettingsExtensions
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

    /// <summary>
    /// Helper methods to show Caliburn.Micro's view models using Callisto's dialogs 
    /// </summary>
    public static class DialogService
    {
        /// <summary>
        /// Shows a settings dialog
        /// </summary>
        /// <typeparam name="T">View model's type, inherited from Screen</typeparam>
        /// <param name="onInitialize">Method which is executed before the dialog is shown</param>
        /// <param name="onClosed">Method which is executed after the dialog has been closed</param>
        /// <param name="headerBrush">Setting pane's header color</param>
        /// <param name="backgroundBrush">Setting pane's backgruond color</param>
        public static void ShowSettings<T>(Action<T> onInitialize = null, Action<T, UIElement> onClosed = null, SolidColorBrush headerBrush = null, SolidColorBrush backgroundBrush = null) where T : Screen
        {
            var viewModelAndView = CreateViewModelAndView(onInitialize);
            var vm = viewModelAndView.Item1;
            var view = viewModelAndView.Item2;

            var f = new SettingsFlyout
            {
                HeaderText = vm.DisplayName,
                Content = view,
            };

            if (headerBrush != null)
                f.HeaderBrush = headerBrush;
            if (backgroundBrush != null)
                f.Background = backgroundBrush;

            f.IsOpen = true;

            if (onClosed != null)
                f.Closed += (sender, o) => onClosed(vm, view);
        }

        /// <summary>
        /// Shows a dialog
        /// </summary>
        /// <typeparam name="T">View model's type, inherited from Screen</typeparam>
        /// <param name="placement">Defines where the dialog should be shown</param>
        /// <param name="placementTarget">The control which is used as a placement target. Example: Placement target can be a button the view. Placement-property defines if the dialog is shown above, under, left or right of the button.</param>
        /// <param name="onInitialize">Method which is executed before the dialog is shown</param>
        /// <param name="onClose">Method which is executed after the dialog has been closed</param>
        public static void ShowDialog<T>(PlacementMode placement, UIElement placementTarget, Action<T> onInitialize = null, Action<T, UIElement> onClose = null) where T : Screen
        {
            var viewModelAndView = CreateViewModelAndView(onInitialize);
            var vm = viewModelAndView.Item1;
            var view = viewModelAndView.Item2;

            var f = new Flyout
                        {
                            Content = view,
                            Placement = placement,
                            PlacementTarget = placementTarget,
                            IsOpen = true,
                        };

            if (onClose != null)
                f.Closed += (sender, o) => onClose(vm, view);
        }

        private static Tuple<T, UIElement> CreateViewModelAndView<T>(Action<T> onInitialize = null) where T : Screen
        {
            var vm = IoC.Get<T>();
            var view = ViewLocator.LocateForModel(vm, null, null);

            ViewModelBinder.Bind(vm, view, null);

            if (onInitialize != null)
                onInitialize(vm);

            ScreenExtensions.TryActivate(vm);

            return new Tuple<T, UIElement>(vm, view);
        }

    }
}
