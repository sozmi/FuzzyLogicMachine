using ClassLibraryFLM.FuzzyLogic.Base;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{
    public class RuleViewModel : BaseViewModel
    {
        public Rule item;
        public RuleViewModel(Rule rule)
        {
            item = rule;
        }

        public RuleViewModel()
        {
            item = new("","");
        }
        public string Condition { get => item.Condition==null ? " " : item.Condition.ToString(); set => SetProperty(item.Condition == null ? " " : item.Condition.Analizer.Input, value, item.SetCondition); }
        public string Target { get => item.Output == null ? " " :item.Output.ToString(); set => SetProperty(item.Output == null ? " " : item.Output.ToString(), value, item.SetOutput); }
    }
}