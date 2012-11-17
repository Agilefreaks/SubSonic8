using System;
using Caliburn.Micro;
using Client.Common;
using Subsonic8.Messages;

namespace Subsonic8.Shell
{
    public class ShellViewModel : Screen, IShellViewModel
    {
        private Uri _source;

        public Uri Source
        {
            get
            {
                return _source;
            }

            set
            {
                if (Equals(value, _source)) return;
                _source = value;
                NotifyOfPropertyChange();
            }
        }

        public ISubsonicService SubsonicService { get; set; }

        public ShellViewModel(IEventAggregator eventAggregator, ISubsonicService subsonicService)
        {
            SubsonicService = subsonicService;
            eventAggregator.Subscribe(this);
        }

        public void Handle(PlayFile message)
        {
            Source = SubsonicService.GetUriForFileWithId(message.Id);
        }
    }
}