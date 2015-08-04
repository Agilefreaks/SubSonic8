﻿namespace Subsonic8.Playback
{
    using System;
    using Windows.UI.Xaml;
    using Caliburn.Micro;
    using MugenInjection.Attributes;
    using Subsonic8.AudioPlayback;

    public class ProgressIndicatorViewModel : Screen, IProgressIndicatorViewModel
    {
        private IAudioPlayerViewModel _audioPlayerViewModel;
        private double _itemDurationInSeconds;
        private double _playbackProgressInSeconds;
        private double _progressStepFrequency;
        private DispatcherTimer _progressTimer;

        #region Fields

        #endregion

        #region Public Properties

        [Inject]
        public IAudioPlayerViewModel AudioPlayerViewModel
        {
            get { return _audioPlayerViewModel; }
            set
            {
                _audioPlayerViewModel = value;
                HookAudioPlayer();
            }
        }

        public double ItemDurationInSeconds
        {
            get { return _itemDurationInSeconds; }
            set
            {
                if (value.Equals(_itemDurationInSeconds)) return;
                _itemDurationInSeconds = value;
                NotifyOfPropertyChange();
            }
        }

        public double PlaybackProgressInSeconds
        {
            get { return _playbackProgressInSeconds; }
            set
            {
                if (value.Equals(_playbackProgressInSeconds)) return;
                _playbackProgressInSeconds = value;
                NotifyOfPropertyChange();
            }
        }

        public double ProgressStepFrequency
        {
            get { return _progressStepFrequency; }
            set
            {
                if (value.Equals(_progressStepFrequency)) return;
                _progressStepFrequency = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Methods

        private void OnProgressTimerTick(object sender, object e)
        {
            UpdateCurrentItemProgress();
        }

        protected void HookAudioPlayer()
        {
            AudioPlayerViewModel.PlaybackPaused += AudioPlayerViewModelOnPlaybackPaused;
            AudioPlayerViewModel.PlaybackStarted += AudioPlayerViewModelOnPlaybackStarted;
            AudioPlayerViewModel.PlaybackStoped += AudioPlayerViewModelOnPlaybackStoped;
        }

        private void AudioPlayerViewModelOnPlaybackStarted(object sender, EventArgs eventArgs)
        {
            ItemDurationInSeconds = AudioPlayerViewModel.GetDuration().TotalSeconds;
            UpdateCurrentItemProgress();
            StartProgressTimer();
        }

        private void AudioPlayerViewModelOnPlaybackStoped(object sender, EventArgs eventArgs)
        {
            StopProgressTimer();
            ItemDurationInSeconds = 0;
            PlaybackProgressInSeconds = 0;
        }

        private void AudioPlayerViewModelOnPlaybackPaused(object sender, EventArgs eventArgs)
        {
            _progressTimer.Stop();
        }

        private void StartProgressTimer()
        {
            _progressTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            _progressTimer.Tick += OnProgressTimerTick;
            _progressTimer.Start();
        }

        private void StopProgressTimer()
        {
            if (_progressTimer != null)
            {
                _progressTimer.Stop();
            }
        }

        private void UpdateCurrentItemProgress()
        {
            PlaybackProgressInSeconds = AudioPlayerViewModel.GetCurrentPosition().TotalSeconds;
        }

        #endregion
    }
}