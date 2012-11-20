using System;
using System.Diagnostics;
using Caliburn.Micro;
using Windows.UI.Xaml.Navigation;

namespace Client.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public void NavigateToViewModel<TViewModel>(object item)
        {
            // Debugger.Break();
        }

        public bool Navigate(Type sourcePageType)
        {
            throw new NotImplementedException();
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public Type SourcePageType { get; set; }

        public Type CurrentSourcePageType { get; private set; }

        public bool CanGoForward { get; private set; }

        public bool CanGoBack { get; private set; }

        public event NavigatedEventHandler Navigated;

        public event NavigatingCancelEventHandler Navigating;

        public event NavigationFailedEventHandler NavigationFailed;

        public event NavigationStoppedEventHandler NavigationStopped;
    }
}