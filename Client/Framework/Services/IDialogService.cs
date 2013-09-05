namespace Subsonic8.Framework.Services
{
    using System;
    using Caliburn.Micro;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Media;

    public interface IDialogService
    {
        /// <summary>
        /// Shows a dialog
        /// </summary>
        /// <typeparam name="T">View model's type, inherited from Screen</typeparam>
        /// <param name="placement">Defines where the dialog should be shown</param>
        /// <param name="placementTarget">The control which is used as a placement target. Example: Placement target can be a button the view. Placement-property defines if the dialog is shown above, under, left or right of the button.</param>
        /// <param name="onInitialize">Method which is executed before the dialog is shown</param>
        /// <param name="onClose">Method which is executed after the dialog has been closed</param>
        void ShowDialog<T>(
            PlacementMode placement, 
            UIElement placementTarget, 
            Action<T> onInitialize = null, 
            Action<T, UIElement> onClose = null) where T : Screen;

        /// <summary>
        /// Shows a settings dialog
        /// </summary>
        /// <typeparam name="T">View model's type, inherited from Screen</typeparam>
        /// <param name="onInitialize">Method which is executed before the dialog is shown</param>
        /// <param name="onClosed">Method which is executed after the dialog has been closed</param>
        /// <param name="headerBrush">Setting pane's header color</param>
        /// <param name="backgroundBrush">Setting pane's backgruond color</param>
        void ShowSettings<T>(
            Action<T> onInitialize = null, 
            Action<T, UIElement> onClosed = null, 
            SolidColorBrush headerBrush = null, 
            SolidColorBrush backgroundBrush = null) where T : Screen;
    }
}