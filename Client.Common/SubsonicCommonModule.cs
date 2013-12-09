namespace Client.Common
{
    using global::Common.MugenExtensions;

    public class SubsonicCommonModule : MugenModuleWithAutoDiscoveryBase
    {
        #region Methods

        protected override void PrepareForLoad()
        {
            Conventions.Add(new ServiceConvention(Injector));
        }

        #endregion
    }
}