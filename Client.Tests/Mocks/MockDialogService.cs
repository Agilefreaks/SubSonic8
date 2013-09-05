namespace Client.Tests.Mocks
{
    using System;
    using Caliburn.Micro;
    using Subsonic8.Framework.Services;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Media;

    public class MockDialogService : IDialogService
    {
        public int ShowSettingsCallCount { get; set; }

        public void ShowDialog<T>(
            PlacementMode placement, UIElement placementTarget, Action<T> onInitialize = null, Action<T, UIElement> onClose = null) where T : Screen
        {
        }

        public void ShowSettings<T>(
            Action<T> onInitialize = null,
            Action<T, UIElement> onClosed = null,
            SolidColorBrush headerBrush = null,
            SolidColorBrush backgroundBrush = null) where T : Screen
        {
            ShowSettingsCallCount++;
        }
    }
}