namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using Client.Common.Services;
    using Subsonic8.Framework.ViewModel;
    using Windows.UI.Xaml.Navigation;

    public class MockNavigationService : ICustomFrameAdapter
    {
        #region Constructors and Destructors

        public MockNavigationService()
        {
            NavigateToViewModelCalls = new List<KeyValuePair<Type, object>>();
        }

        #endregion

        #region Public Events

        public event NavigatedEventHandler Navigated;

        public event NavigatingCancelEventHandler Navigating;

        public event NavigationFailedEventHandler NavigationFailed;

        public event NavigationStoppedEventHandler NavigationStopped;

        #endregion

        #region Public Properties

        public bool CanGoBack { get; set; }

        public bool CanGoForward { get; private set; }

        public Type CurrentSourcePageType { get; private set; }

        public int GoBackCallCount { get; set; }

        public List<KeyValuePair<Type, object>> NavigateToViewModelCalls { get; private set; }

        public Type SourcePageType { get; set; }

        #endregion

        #region Public Methods and Operators

        public void DoNavigate()
        {
            NavigateToViewModelCalls.Add(new KeyValuePair<Type, object>(typeof(IViewModel), null));
        }

        public void DoNavigate(Type targetType)
        {
            NavigateToViewModelCalls.Add(new KeyValuePair<Type, object>(targetType, null));
        }

        public void GoBack()
        {
            GoBackCallCount++;
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType)
        {
            NavigateToViewModelCalls.Add(new KeyValuePair<Type, object>(sourcePageType, null));
            return true;
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            NavigateToViewModelCalls.Add(new KeyValuePair<Type, object>(sourcePageType, parameter));
            return true;
        }

        public void NavigateToViewModel<T>(object parameter = null) where T : Screen
        {
            NavigateToViewModelCalls.Add(new KeyValuePair<Type, object>(typeof(T), parameter));
        }

        #endregion
    }
}