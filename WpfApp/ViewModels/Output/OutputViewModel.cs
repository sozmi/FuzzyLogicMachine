using ClassLibraryFLM.Base;
using ClassLibraryFLM.FuzzyLogic.Base;
using ClassLibraryFLM.FuzzyLogic.Production;
using ClassLibraryFLM.FuzzyLogic.Test;
using System.Runtime.Serialization;
using System.Windows;
using WpfApp.Classes;
using WpfApp.ViewModels.Base;
using WpfApp.ViewModels.Linguistic;
using WpfApp.Views.Linguistic;

namespace WpfApp.ViewModels.Output
{
    public class PairVarValueViewModel : BaseViewModel
    {
        public Pair<Variable, double> item;

        public PairVarValueViewModel(Pair<Variable, double> pair)
        {
            this.item = pair;
        }
        public PairVarValueViewModel()
        {
           item = new(new("Не указан"), 0);
        }
        public Variable Variable { get => item.Item1; set => SetProperty(item.Item1, value, v => { item.Item1 = v; }); }
        public double Value { get => item.Item2; set => SetProperty(item.Item2, value, v => { item.Item2 = v; }); }
    }
    public class InputParamsViewModel : BaseViewModel
    {
        public InputParam item;

        public InputParamsViewModel()
        {
            item = new("", []);
            DG_VarsValue = new();
            DG_VarsValue.OnAdd += OnAddValue;
            DG_VarsValue.OnRemove += OnRemoveValue;
        }
        public void OnAddValue(ParamsInfo info)
        {
            if (info.Item is PairVarValueViewModel vm)
            {
                item.VariablesValue.Add(vm.item);
                vm.item = item.VariablesValue.Last();
            }    
        }
        public void OnRemoveValue(ParamsInfo info)
        {
            if (info.Item is PairVarValueViewModel vm)
            {
                item.VariablesValue.Remove(vm.item);
            }  
        }
        public InputParamsViewModel(InputParam inputParam)
        {
            item = inputParam;
            DG_VarsValue = new();

            foreach (var v in inputParam.VariablesValue)
                DG_VarsValue.Items.Add(new(v));
            DG_VarsValue.OnAdd += OnAddValue;
            DG_VarsValue.OnRemove += OnRemoveValue;
        }
        public DataGridViewModel<PairVarValueViewModel> DG_VarsValue { get; set; }
        public string Name { get => item.Name; set => SetProperty(item.Name, value, v => { item.Name = v; }); }
        public List<Variable> Vars { get => BaseRules.Obj().LinguisticVars; }
    }
    public class OutputViewModel : BaseViewModel
    {
        public Variable OutVar { get => _outVar; set => SetProperty(ref _outVar, value); }
        private Variable _outVar;
        public InputParam BestParam { get => _bestParam; set => SetProperty(ref _bestParam, value); }
        private InputParam _bestParam;
        public bool IsMax { get => _maxmin; set => SetProperty(ref _maxmin, value); }
        private bool _maxmin = false;
        public List<Variable> Vars { get => BaseRules.Obj().LinguisticVars; }
        public PlotViewModelTest PlotVM { get; set; }
        public DataGridViewModel<InputParamsViewModel> DGParams { get => _DGParams; set => SetProperty(ref _DGParams, value); }
        private DataGridViewModel<InputParamsViewModel> _DGParams;


        public OutputViewModel()
        {
            ReInitDG();

            PlotVM = new();
            ActionEval = new(Eval);

            ActionTestCar = new(TestCar);
            ActionTestWater = new(TestWater);
            ActionTestExam = new(TestExam);
            ActionTestWork = new(TestWork);
        }
        public RelayAction ActionEval { get; private set; }
        public RelayAction ActionTestCar { get; private set; }
        public RelayAction ActionTestWater { get; private set; }
        public RelayAction ActionTestExam { get; private set; }
        public RelayAction ActionTestWork { get; private set; }

        private void Eval()
        {
            List<InputParam> inputParams = [];
            foreach (var param in DGParams.Items)
                inputParams.Add(param.item);
            var o =  ClassLibraryFLM.FuzzyLogic.Production.Output.Calc(inputParams, OutVar, IsMax);
            PlotVM.Clear();
            for(int i = 0; i<o.Params.Count; i++)
            {
                var p = o.Params[i];
                var f = o.Functions[i];
                AreaByIntervals area = new(f, OutVar.Universe.Item1, OutVar.Universe.Item2, $"{p.Name}: {p.Value}", 0.001);
                PlotVM.AddArea(area);
            }
            BestParam = o.Best;
        }
        private void TestCar()=> FillByTest(OutputManager.TestCar);
        private void TestWater() => FillByTest(OutputManager.TestWater);
        private void TestExam() => FillByTest(OutputManager.TestExam);
        private void TestWork() => FillByTest(OutputManager.TestWork);

        void FillByTest(Func<Pair<Variable, List<InputParam>>> func)
        {
            var p = func();
            OutVar = p.Item1;
            DGParams.Items.Clear();
            foreach (var inputParam in p.Item2)
                DGParams.Items.Add(new(inputParam));
        }

        private void ReInitDG()
        {
            DGParams = new();
        }

    }
}
