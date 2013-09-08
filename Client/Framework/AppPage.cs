namespace Subsonic8.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Caliburn.Micro;
    using Client.Common.Services;
    using Microsoft.Practices.ServiceLocation;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework.ViewModel;
    using Windows.ApplicationModel;
    using Windows.Foundation.Metadata;
    using Windows.System;
    using Windows.UI.Core;
    using Windows.UI.ViewManagement;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Typical implementation of Page that provides several important conveniences:
    /// <list type="bullet">
    /// <item>
    /// <description>Application view state to visual state mapping</description>
    /// </item>
    /// <item>
    /// <description>GoBack, GoForward, and GoHome event handlers</description>
    /// </item>
    /// <item>
    /// <description>Mouse and keyboard shortcuts for navigation</description>
    /// </item>
    /// <item>
    /// <description>State management for navigation and process lifetime management</description>
    /// </item>
    /// <item>
    /// <description>A default view model</description>
    /// </item>
    /// </list>
    /// </summary>
    [WebHostHidden]
    public class AppPage : Page
    {
        #region Fields

        private List<Control> _layoutAwareControls;

        private string _pageKey;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppPage"/> class.
        /// </summary>
        public AppPage()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            // When this page is part of the visual tree make two changes:
            // 1) Map application view state to visual state for the page
            // 2) Handle keyboard and mouse navigation requests
            Loaded += (sender, e) =>
                {
                    StartLayoutUpdates(sender, e);

                    // Keyboard and mouse navigation only apply when occupying the entire window
                    if (ActualHeight.Equals(Window.Current.Bounds.Height)
                        && ActualWidth.Equals(Window.Current.Bounds.Width))
                    {
                        // Listen to the window directly so focus isn't required
                        Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                            CoreDispatcher_AcceleratorKeyActivated;
                        Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
                    }
                };

            // Undo the same changes when the page is no longer visible
            Unloaded += (sender, e) =>
                {
                    StopLayoutUpdates(sender, e);
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                        CoreDispatcher_AcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed -= CoreWindow_PointerPressed;
                };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Updates all controls that are listening for visual state changes with the correct
        /// visual state.
        /// </summary>
        /// <remarks>
        /// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
        /// signal that a different value may be returned even though the view state has not
        /// changed.
        /// </remarks>
        public void InvalidateVisualState()
        {
            if (_layoutAwareControls != null)
            {
                var visualState = DetermineVisualState(ApplicationView.Value);
                foreach (var layoutAwareControl in _layoutAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                    var visualStateAwareDataContext = layoutAwareControl.DataContext as IVisualStateAware;
                    if (visualStateAwareDataContext != null)
                    {
                        visualStateAwareDataContext.OnVisualStateChanged(visualState);
                    }
                }
            }
        }

        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded"/>
        /// event of a <see cref="Control"/> within the page, to indicate that the sender should
        /// start receiving visual state management changes that correspond to application view
        /// state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.  A corresponding
        /// <see cref="FrameworkElement.Unloaded"/> event handler connected to
        /// <see cref="StopLayoutUpdates"/> is strongly encouraged.  Instances of
        /// <see cref="AppPage"/> automatically invoke these handlers in their Loaded and
        /// Unloaded events.</remarks>
        /// <seealso cref="DetermineVisualState"/>
        /// <seealso cref="InvalidateVisualState"/>
        public void StartLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            if (_layoutAwareControls == null)
            {
                // Start listening to view state changes when there are controls interested in updates
                Window.Current.SizeChanged += WindowSizeChanged;
                _layoutAwareControls = new List<Control>();
            }

            _layoutAwareControls.Add(control);

            // Set the initial visual state of the control
            VisualStateManager.GoToState(control, DetermineVisualState(ApplicationView.Value), false);
        }

        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded"/>
        /// event of a <see cref="Control"/>, to indicate that the sender should start receiving
        /// visual state management changes that correspond to application view state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="e">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.</remarks>
        /// <seealso cref="StartLayoutUpdates"/>
        public void StopLayoutUpdates(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            if (control == null || _layoutAwareControls == null)
            {
                return;
            }

            _layoutAwareControls.Remove(control);
            if (_layoutAwareControls.Count == 0)
            {
                // Stop listening to view state changes when no controls are interested in updates
                _layoutAwareControls = null;
                Window.Current.SizeChanged -= WindowSizeChanged;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Translates <see cref="ApplicationViewState"/> values into strings for visual state
        /// management within the page.  The default implementation uses the names of enum values.
        /// Subclasses may override this method to control the mapping scheme used.
        /// </summary>
        /// <param name="viewState">View state for which a visual state is desired.</param>
        /// <returns>Visual state name used to drive the
        /// <see cref="VisualStateManager"/></returns>
        /// <seealso cref="InvalidateVisualState"/>
        protected virtual string DetermineVisualState(ApplicationViewState viewState)
        {
            return viewState.ToString();
        }

        /// <summary>
        /// Invoked as an event handler to navigate backward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoBack(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to return to the previous page
            if (Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        /// <summary>
        /// Invoked as an event handler to navigate forward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoForward(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to move to the next page
            if (Frame != null && Frame.CanGoForward)
            {
                Frame.GoForward();
            }
        }

        /// <summary>
        /// Invoked as an event handler to navigate backward in the page's associated
        /// <see cref="Frame"/> until it reaches the top of the navigation stack.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="e">Event data describing the conditions that led to the event.</param>
        protected virtual void GoHome(object sender, RoutedEventArgs e)
        {
            // Use the navigation frame to return to the topmost page
            if (Frame == null)
            {
                return;
            }

            while (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session. This will be null the first time a page is visited.</param>
        /// <param name="e">The navigation event args.</param>
        protected virtual void LoadState(
            object navigationParameter, Dictionary<string, object> pageState, NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                var svc = ServiceLocator.Current.GetInstance<INavigationService>() as CustomFrameAdapter;
                if (svc != null)
                {
                    svc.DoNavigated(this, e);
                }
            }

            if (DataContext is IHaveState && pageState != null && pageState.Any())
            {
                var haveState = DataContext as IHaveState;
                haveState.LoadState(Convert.ToString(navigationParameter), pageState);
            }
        }

        /// <summary>
        /// Invoked when this page will no longer be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property provides the group to be displayed.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var frameState = SuspensionManager.SessionStateForFrame(Frame);
            var pageState = new Dictionary<string, object>();
            SaveState(pageState);
            frameState[_pageKey] = pageState;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached. The Parameter
        /// property provides the group to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                // Returning to a cached page through navigation shouldn't trigger state loading
                if (_pageKey != null)
                {
                    return;
                }

                var frameState = SuspensionManager.SessionStateForFrame(Frame);
                _pageKey = "Page-" + Frame.BackStackDepth;
                if (e.NavigationMode == NavigationMode.New)
                {
                    // Clear existing state for forward navigation when adding a new page to the
                    // navigation stack
                    var nextPageKey = _pageKey;
                    var nextPageIndex = Frame.BackStackDepth;
                    while (frameState.Remove(nextPageKey))
                    {
                        nextPageIndex++;
                        nextPageKey = "Page-" + nextPageIndex;
                    }

                    // Pass the navigation parameter to the new page
                    LoadState(e.Parameter, null, e);
                }
                else
                {
                    // Pass the navigation parameter and preserved page state to the page, using
                    // the same strategy for loading suspended state and recreating pages discarded
                    // from cache
                    LoadState(e.Parameter, (Dictionary<string, object>)frameState[_pageKey], e);
                }

                if (true)
                {
                    ApplyTemplate();
                }
            }
            catch (Exception exception)
            {
                var errorDialogViewModel = IoC.Get<IErrorDialogViewModel>();
                errorDialogViewModel.HandleError(exception);
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected virtual void SaveState(Dictionary<string, object> pageState)
        {
            if (DataContext is IHaveState)
            {
                var haveState = DataContext as IHaveState;
                haveState.SaveState(pageState, SuspensionManager.KnownTypes);
            }
        }

        /// <summary>
        /// Invoked on every keystroke, including system keys such as Alt key combinations, when
        /// this page is active and occupies the entire window.  Used to detect keyboard navigation
        /// between pages even when the page itself doesn't have focus.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            var virtualKey = args.VirtualKey;

            // Only investigate further when Left, Right, or the dedicated Previous or Next keys
            // are pressed
            if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                 || args.EventType == CoreAcceleratorKeyEventType.KeyDown)
                && (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right || (int)virtualKey == 166
                    || (int)virtualKey == 167))
            {
                const CoreVirtualKeyStates DownState = CoreVirtualKeyStates.Down;
                var coreWindow = Window.Current.CoreWindow;
                var menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & DownState) == DownState;
                var controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & DownState) == DownState;
                var shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & DownState) == DownState;
                var noModifiers = !menuKey && !controlKey && !shiftKey;
                var onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) || (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // When the previous key or Alt+Left are pressed navigate back
                    args.Handled = true;
                    GoBack(this, new RoutedEventArgs());
                }
                else if (((int)virtualKey == 167 && noModifiers) || (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    args.Handled = true;
                    GoForward(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction when this
        /// page is active and occupies the entire window.  Used to detect browser-style next and
        /// previous mouse button clicks to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed || properties.IsMiddleButtonPressed)
            {
                return;
            }

            // If back or foward are pressed (but not both) navigate appropriately
            var backPressed = properties.IsXButton1Pressed;
            var forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;
                if (backPressed)
                {
                    GoBack(this, new RoutedEventArgs());
                }

                if (forwardPressed)
                {
                    GoForward(this, new RoutedEventArgs());
                }
            }
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            InvalidateVisualState();
        }

        #endregion
    }
}