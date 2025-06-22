using ClassLibraryFLM.FuzzyLogic.Production;
using WpfApp.Classes;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{

    public class ParamsInfo
    {
        public int Index = 0;
        public object Item;
    }



    public class LinguisticViewModel : BaseViewModel
    {
        public PlotViewModelTest PlotVM { get; set; }
        public DataGridViewModel<LinguisticVarViewModel> DGVariables { get => _DGVariables; set => SetProperty(ref _DGVariables, value); }
        private DataGridViewModel<LinguisticVarViewModel> _DGVariables;
        public DataGridViewModel<RuleViewModel> DGRules { get => _DGRules; set => SetProperty(ref _DGRules, value); }
        private DataGridViewModel<RuleViewModel> _DGRules;
        public LinguisticViewModel()
        {
            ReInitDGVariables();

            ReInitDGRules();

            PlotVM = new();

            ActionTestAccumulation = new(TestCaseAccumulation);
            ActionTestActive = new(TestCaseActivation);
            ActionTestAggregation = new(TestCaseAggregation);
            ActionTestFuzzification = new(TestCaseFazzification);
            ActionTestSyntaxAnalizer = new(TestCaseSyntaxAnalizer);
            ActionTestUnFuzzification = new(TestCaseUnFuzzification);

            ActionFillWater = new(FillWater);
            ActionFillCar = new(FillCar);
            ActionFillExam = new(FillExam);
            ActionFillWork = new(FillWork);
        }



        private void ReInitDGRules()
        {
            var db = BaseRules.Obj();
            List<RuleViewModel> vars = [];
            foreach (var v in db.Rules)
                vars.Add(new(v));

            DGRules = new(vars);
            DGRules.OnAdd += AddRule;
            DGRules.OnRemove += RemoveRule;
        }

        private void ReInitDGVariables()
        {
            var db = BaseRules.Obj();
            List<LinguisticVarViewModel> vars = [];
            foreach (var v in db.LinguisticVars)
                vars.Add(new(v));
            
            DGVariables = new(vars);
            DGVariables.OnAdd += AddLingvisticVar;
            DGVariables.OnEdit += EditLinguisticVar;
            DGVariables.OnRemove += RemoveLingvisticVar;
        }
        private void AddRule(ParamsInfo info)
        {
            if (info.Item is RuleViewModel varVM)
            {
                var db = BaseRules.Obj();
                varVM.item = db.AddRule(varVM.item);
            }
        }
        private void RemoveRule(ParamsInfo info)
        {
            if (info.Item is RuleViewModel varVM)
            {
                var db = BaseRules.Obj();
                db.RemoveRule(varVM.item);
            }
        }

        private void AddLingvisticVar(ParamsInfo info)
        {
            if (info.Item is LinguisticVarViewModel varVM)
            {
                var db = BaseRules.Obj();
                varVM.item = db.AddLinguisticVariable(varVM.item);
            }
        }
        private void RemoveLingvisticVar(ParamsInfo info)
        {
            if (info.Item is LinguisticVarViewModel varVM)
            {
                var db = BaseRules.Obj();
                db.RemoveLinguisticVariable(varVM.item.Name);
            }
        }
        private void EditLinguisticVar(ParamsInfo info)
        {
            if (info.Item is LinguisticVarViewModel varVM)
            {
                if (!varVM.Selected || varVM.Universe == null)
                {
                    PlotVM.Clear();
                    return;
                }

                AreaByIntervals area = new(varVM.item.GetFP(), varVM.Universe.Item1, varVM.Universe.Item2, varVM.Name);
                PlotVM.Clear();
                PlotVM.AddArea(area);
                foreach (var term in varVM.item.Terms)
                {
                    SeriesByIntervals series = new(term.Function, varVM.Universe.Item1, varVM.Universe.Item2, term.Name);
                    PlotVM.AddSerial(series);
                }
            }
        }

        public RelayAction ActionTestFuzzification { get; set; }
        public RelayAction ActionTestSyntaxAnalizer { get; set; }
        public RelayAction ActionTestAccumulation { get; set; }
        public RelayAction ActionTestActive { get; set; }
        public RelayAction ActionTestAggregation { get; set; }
        public RelayAction ActionTestUnFuzzification { get; set; }

        
        public RelayAction ActionFillCar {  get; set; }
        public RelayAction ActionFillExam {  get; set; }
        public RelayAction ActionFillWater { get; set; }
        public RelayAction ActionFillWork{ get; set; }

        private void TestCaseAccumulation()
        {
            TestFuzzy.TestAccumulation();
            FillViewModel();
        }

        private void TestCaseSyntaxAnalizer()
        {
            TestFuzzy.TestSyntaxAnalyze();
            FillViewModel();
        }
        private void TestCaseActivation()
        {
            TestFuzzy.TestActivation();
            FillViewModel();
        }

        private void TestCaseFazzification()
        {
            TestFuzzy.TestFazzification();
            FillViewModel();
        }

        private void TestCaseAggregation()
        {
            TestFuzzy.TestAggregation();
            FillViewModel();
        }
        private void TestCaseUnFuzzification()
        {
            TestFuzzy.TestUnFazzification();
            FillViewModel();
        }

        private void FillWater()
        {
            BaseRules.Obj().Clear();
            BaseRulesManager.TestWater();
            FillViewModel();
        }
        private void FillWork()
        {
            BaseRules.Obj().Clear();
            BaseRulesManager.TestWork();
            FillViewModel();
        }
        private void FillCar()
        {
            BaseRules.Obj().Clear();
            BaseRulesManager.TestCar();
            FillViewModel();
        }
        private void FillExam()
        {
            BaseRules.Obj().Clear();
            BaseRulesManager.TestExam();
            FillViewModel();
        }
        private void FillViewModel()
        {
           ReInitDGRules();
           ReInitDGVariables();
        }
    }
}