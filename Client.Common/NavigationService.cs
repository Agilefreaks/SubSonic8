using System;
using Caliburn.Micro;
using Windows.UI.Xaml.Controls;

namespace Client.Common
{
    public class NavigationService : FrameAdapter
    {
        protected readonly bool _visibleTreatViewAsLoaded;
        public NavigationService(Frame frame, bool treatViewAsLoaded = false) 
            : base(frame, treatViewAsLoaded)
        {
            _visibleTreatViewAsLoaded = treatViewAsLoaded;
        }

        protected override void OnNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Content == null)
                return;

            ViewLocator.InitializeComponent(e.Content);

            var viewModel = ViewModelLocator.LocateForViewType(e.Content.GetType());

            if (viewModel == null)
                return;

            var view = e.Content as Page;

            if (view == null)
            {
                throw new ArgumentException("View '" + e.Content.GetType().FullName + "' should inherit from Page or one of its descendents.");
            }

            if (_visibleTreatViewAsLoaded)
            {
                view.SetValue(View.IsLoadedProperty, true);
            }

            TryInjectParameter(viewModel, e.Parameter);

            ViewModelBinder.Bind(viewModel, view, null);

            var activator = viewModel as IActivate;

            if (activator != null)
            {
                activator.Activate();
            }

            GC.Collect(); // Why?
        }
    }
}