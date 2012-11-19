using System.Threading.Tasks;
using Caliburn.Micro;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Client.Common.Results
{
    public class VisualStateResult : ResultBase
    {
        private readonly string _stateName;
        private readonly bool _useTransitions;

        public VisualStateResult(string stateName, bool useTransitions = true)
        {
            _stateName = stateName;
            _useTransitions = useTransitions;
        }

        public string StateName
        {
            get { return _stateName; }
        }

        public bool UseTransitions
        {
            get { return _useTransitions; }
        }

        protected override Task ExecuteCore(ActionExecutionContext context = null)
        {
            context = context ?? new ActionExecutionContext();
            var view = (context.View ?? ((CaliburnApplication)Application.Current).RootFrame.Content) as Control;

            VisualStateManager.GoToState(view, StateName, UseTransitions);

            return null;
        }
    }
}