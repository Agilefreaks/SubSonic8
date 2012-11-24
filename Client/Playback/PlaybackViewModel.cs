using Caliburn.Micro;
using Client.Common;
using Client.Common.Models;
using Subsonic8.Messages;

namespace Subsonic8.Playback
{
    public class PlaybackViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private ISubsonicModel _parameter;

        public ISubsonicModel Parameter
        {
            get
            {
                return _parameter;
            }

            set
            {
                if (Equals(value, _parameter)) return;
                _parameter = value;
                NotifyOfPropertyChange();
                _eventAggregator.Publish(new PlayFile { Id = value.Id });
            }
        }

        public PlaybackViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
    }
}