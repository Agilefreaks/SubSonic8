using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Subsonic8.Framework.ViewModel;
using Windows.UI.Xaml.Navigation;

namespace Client.Tests.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public Dictionary<Type, object> NavigateToViewModelCalls { get; private set; }

        public Type SourcePageType { get; set; }

        public Type CurrentSourcePageType { get; private set; }

        public bool CanGoForward { get; private set; }

        public bool CanGoBack { get; private set; }

        public event NavigatedEventHandler Navigated;

        public event NavigatingCancelEventHandler Navigating;

        public event NavigationFailedEventHandler NavigationFailed;

        public event NavigationStoppedEventHandler NavigationStopped;

        public MockNavigationService()
        {
            NavigateToViewModelCalls = new Dictionary<Type, object>();
        }

        public void NavigateToViewModel<TViewModel>(object parameter)
        {
            NavigateToViewModelCalls.Add(typeof(TViewModel), parameter);
        }

        public bool Navigate(Type sourcePageType)
        {
            NavigateToViewModelCalls.Add(sourcePageType, null);
            return true;
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            NavigateToViewModelCalls.Add(sourcePageType, parameter);
            return true;
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void DoNavigate()
        {
            NavigateToViewModelCalls.Add(typeof(IViewModel), null);
        }
    }
}