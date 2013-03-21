using Client.Common.MugenExtensions;

namespace Client.Common
{
    public class CommonModule : MugenModuleWithAutoDiscoveryBase
    {
        protected override void PrepareForLoad()
        {
            Convetions.Add(new ServiceConvention(Injector));
        }
    }
}
