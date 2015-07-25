namespace Client.Common.Helpers
{
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;

    public interface IMediaControlHandler : IHandle<StartPlaybackMessage>
    {
        #region Public Methods and Operators

        void Register();

        #endregion
    }
}