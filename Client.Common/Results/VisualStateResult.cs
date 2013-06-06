namespace Client.Common.Results
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class VisualStateResult : ResultBase
    {
        #region Constructors and Destructors

        public VisualStateResult(string stateName, bool useTransitions = true)
        {
            StateName = stateName;
            UseTransitions = useTransitions;
        }

        #endregion

        #region Public Properties

        public string StateName { get; private set; }

        public bool UseTransitions { get; private set; }

        #endregion

        #region Methods

        protected override Task ExecuteCore(ActionExecutionContext context = null)
        {
            context = context ?? new ActionExecutionContext();
            var view = (context.View ?? ((CaliburnApplication)Application.Current).RootFrame.Content) as Control;

            VisualStateManager.GoToState(view, StateName, UseTransitions);

            return Task.Factory.StartNew(() => { });
        }

        #endregion
    }
}