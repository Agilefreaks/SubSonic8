namespace SubLastFm
{
    using Common.MugenExtensions;

    public class SubLastFmModule : MugenModuleWithAutoDiscoveryBase
    {
        protected override void PrepareForLoad()
        {
            Conventions.AddRange(new MugenConvetion[] { new ServiceConvention(Injector) });
        }
    }
}
