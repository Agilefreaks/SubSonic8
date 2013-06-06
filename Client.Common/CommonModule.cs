namespace Client.Common
{
    using global::Common.MugenExtensions;

    public class CommonModule : MugenModuleWithAutoDiscoveryBase
    {
        #region Methods

        protected override void PrepareForLoad()
        {
            Convetions.Add(new ServiceConvention(Injector));
        }

        #endregion
    }
}