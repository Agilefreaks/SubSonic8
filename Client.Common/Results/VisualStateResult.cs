namespace Client.Common.Results
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class VisualStateResult : ResultBase
    {
        #region Fields

        private readonly string _stateName;

        private readonly bool _useTransitions;

        #endregion

        #region Constructors and Destructors

        public VisualStateResult(string stateName, bool useTransitions = true)
        {
            _stateName = stateName;
            _useTransitions = useTransitions;
        }

        #endregion

        #region Public Properties

        public string StateName
        {
            get
            {
                return _stateName;
            }
        }

        public bool UseTransitions
        {
            get
            {
                return _useTransitions;
            }
        }

        #endregion

        #region Methods

        protected override Task ExecuteCore(ActionExecutionContext context = null)
        {
            context = context ?? new ActionExecutionContext();
            var view = (context.View ?? ((CaliburnApplication)Application.Current).RootFrame.Content) as Control;

            VisualStateManager.GoToState(view, StateName, UseTransitions);

            return null;
        }

        #endregion
    }
}