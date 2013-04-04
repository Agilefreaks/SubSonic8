using Caliburn.Micro;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Client.Common.Services
{
    public class CustomFrameAdapter : FrameAdapter
    {
        public CustomFrameAdapter(Frame frame, bool treatViewAsLoaded = false)
            : base(frame, treatViewAsLoaded)
        {
        }

        public void DoNavigated(object sender, NavigationEventArgs eventArgs)
        {
            base.OnNavigated(sender, eventArgs);
        }
    }
}