namespace Client.Tests.Mocks
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.Services;
    using global::Common.Interfaces;
    using Subsonic8.ArtistInfo;
    using Subsonic8.ErrorDialog;
    using Subsonic8.Framework.Services;
    using Action = System.Action;

    public class MockArtistInfoViewModel : IArtistInfoViewModel
    {
        public string DisplayName { get; set; }

        public bool IsActive { get; private set; }

        public bool IsNotifying { get; set; }

        public IErrorHandler ErrorHandler { get; private set; }

        public ISubsonicService SubsonicService { get; set; }

        public bool CanGoBack { get; private set; }

        public IEventAggregator EventAggregator { get; set; }

        public ICustomFrameAdapter NavigationService { get; set; }

        public IDialogNotificationService NotificationService { get; set; }

        public Action UpdateDisplayName { get; set; }

        public IErrorDialogViewModel ErrorDialogViewModel { get; set; }

        public string Parameter { get; set; }

        public int PopulateCallCount { get; set; }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public void Activate()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ActivationEventArgs> Activated;

        public void Deactivate(bool close)
        {
            throw new NotImplementedException();
        }

        public void TryClose()
        {
            throw new NotImplementedException();
        }

        public void CanClose(Action<bool> callback)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyOfPropertyChange(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public Task HandleError(Exception error)
        {
            throw new NotImplementedException();
        }

        public async Task Populate()
        {
            await Task.Run(() =>
            {
                PopulateCallCount++;
            });
        }
    }
}