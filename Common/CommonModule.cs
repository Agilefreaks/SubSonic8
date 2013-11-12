namespace Common
{
    using Common.Interfaces;
    using Common.MugenExtensions;
    using Common.Services;

    public class CommonModule : MugenModuleWithAutoDiscoveryBase
    {
        protected override void PrepareForLoad()
        {
            Singletons.Add<IHtmlTransformService, HtmlTransformService>();
        }
    }
}
