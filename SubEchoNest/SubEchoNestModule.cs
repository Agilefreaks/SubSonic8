namespace SubEchoNest
{
    using Common.MugenExtensions;

    public class SubEchoNestModule : MugenModuleWithAutoDiscoveryBase
    {
        protected override void PrepareForLoad()
        {
            Conventions.AddRange(new MugenConvetion[] { new ServiceConvention(Injector) });
        }
    }
}
