using System;
using Caliburn.Micro;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Client.Common
{
    public class CustomFrameAdapter : FrameAdapter
    {
        private readonly bool _treatViewAsLoaded;

        public CustomFrameAdapter(Frame frame, bool treatViewAsLoaded = false) : base(frame, treatViewAsLoaded)
        {
            _treatViewAsLoaded = treatViewAsLoaded;
        }

        protected override void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content == null)
            {
                return;
            }

            ViewLocator.InitializeComponent(e.Content);

            var viewModel = ViewModelLocator.LocateForViewType(e.Content.GetType());

            if (viewModel == null)
                return;

            var view = e.Content as Page;

            if (view == null)
            {
                throw new ArgumentException("View '" + e.Content.GetType().FullName + "' should inherit from Page or one of its descendents.");
            }

            if (_treatViewAsLoaded)
            {
                view.SetValue(View.IsLoadedProperty, true);
            }

            TryInjectParameters(viewModel, e.Parameter);

            ViewModelBinder.Bind(viewModel, view, null);

            var activator = viewModel as IActivate;

            if (activator != null)
            {
                activator.Activate();
            }

            var viewAware = viewModel as ViewAware;

            if (viewAware != null)
            {
                EventHandler<object> onLayoutUpdate = null;

                onLayoutUpdate = delegate
                {
                    // viewAware.OnViewReady(view);
                    view.LayoutUpdated -= onLayoutUpdate;
                };

                view.LayoutUpdated += onLayoutUpdate;
            }

            GC.Collect(); // Why?
        }

    }
}