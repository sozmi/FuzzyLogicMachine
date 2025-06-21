using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;
using ClassLibraryFLM.FuzzyLogic.Production;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using WpfApp.Classes;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{
    public class LinguisticTermViewModel : BaseViewModel
    {
        public Term item;

        public LinguisticTermViewModel()
        {
            item = new("Новое", FuncFabric.GetFunc(ETypeFunc.None));
            FuncViewModel = new(new SeriesByIntervals(item.Function, 0, 0, item.Name));
            FuncViewModel.PropertyChanged += FuncChanged;
        }
        public LinguisticTermViewModel(Term item, Pair<double, double> universe)
        {
            this.item = item;
            FuncViewModel = new(new SeriesByIntervals(item.Function, universe.Item1, universe.Item2, item.Name));
            FuncViewModel.PropertyChanged += FuncChanged;
        }
        private void FuncChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is FuncViewModel2 fv)
                item.Function = fv.function.func;
            Invoke(nameof(FuncViewModel));
        }
        public string Name { get => item.Name; set => SetProperty(item.Name, value, v => { item.Name = v; }); }

        public FuncViewModel2 FuncViewModel { get; set; }
    }

    public class PairViewModel<A, B>(Pair<A, B> pair) : BaseViewModel
    {
        public Pair<A, B> item = pair;

        public A Item1 { get => item.Item1; set => SetProperty(item.Item1, value, v => { item.Item1 = v; }); }
        public B Item2 { get => item.Item2; set => SetProperty(item.Item2, value, v => { item.Item2 = v; }); }
    }

    public class LinguisticVarViewModel : BaseViewModel
    {
        public Variable item;

        public LinguisticVarViewModel()
        {
            this.item = new Variable("Новая переменная");
            DG_Terms = new();
            DG_Terms.OnEdit += EditTerm;
            DG_Terms.OnAdd += AddTerm;
            Universe = new(new(0,100));
            Universe.PropertyChanged += UniverseChanged;
        }
        public LinguisticVarViewModel(Variable item)
        {
            this.item = item;
            DG_Terms = new();
            foreach (var term in item.Terms)
                DG_Terms.Items.Add(new(term, item.Universe));
            DG_Terms.OnEdit += EditTerm;
            DG_Terms.OnAdd += AddTerm;
            DG_Terms.OnRemove += RemoveTerm;
            Universe = new(item.Universe);
            Universe.PropertyChanged += UniverseChanged;
        }

        public string Name { get => item.Name; set => SetProperty(item.Name, value, v => { item.Name = v; }); }

        public double? Value { get => item.Value; set => SetProperty(item.Value, value, v => { item.Value = v; }); }
        public PairViewModel<double, double> Universe { get; set; }


        private void UniverseChanged(object? sender, PropertyChangedEventArgs e)
        {
            Invoke(nameof(Universe));
        }
        private void EditTerm(ParamsInfo info)
        {
            Invoke(nameof(DG_Terms));
        }
        private void AddTerm(ParamsInfo info)
        {
            if (info.Item is LinguisticTermViewModel vm)
                vm.item = item.AddTerm(vm.Name, vm.FuncViewModel.function.func);
            Invoke(nameof(DG_Terms));
        }

        private void RemoveTerm(ParamsInfo info)
        {
            if (info.Item is LinguisticTermViewModel vm)
                item.RemoveTerm(vm.Name);
            Invoke(nameof(DG_Terms));
        }
        public bool Selected { get => _selected; set => SetProperty(ref _selected, value); }
        private bool _selected;
        public DataGridViewModel<LinguisticTermViewModel> DG_Terms { get; set; }
    }
    public class ParamsInfo
    {
        public int Index = 0;
        public object Item;
    }
    public class DataGridViewModel<TViewModel> : CollectionViewModel where TViewModel : new()
    {
        public delegate void FuncHandler(ParamsInfo info);
        public event FuncHandler? OnAdd, OnRemove, OnEdit;
        public ObservableCollection<TViewModel> Items { get; set; } = [];
        public DataGridViewModel()
        {
            Items = [];
            Items.CollectionChanged += DataCollectionChanged;
        }
        public DataGridViewModel(List<TViewModel> views)
        {
            Items = [.. views];
            foreach (var item in views)
                if (item is BaseViewModel b)
                    b.PropertyChanged += ItemChanged;
            Items.CollectionChanged += DataCollectionChanged;
        }

        private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ParamsInfo info = new()
                    {
                        Index = Items.Count
                    };
                    if (item is TViewModel i)
                        info.Item = i;
                    if (item is BaseViewModel b)
                        b.PropertyChanged += ItemChanged;
                    OnAdd?.Invoke(info);
                }

                Debug.WriteLine("Добавленных элементов: {0}", e.NewItems.Count);
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ParamsInfo info = new()
                    {
                        Index = e.OldStartingIndex,
                    };
                    if (item is TViewModel i)
                        info.Item = i;
                    OnRemove?.Invoke(info);
                }
                Debug.WriteLine("Удалённых элементов: {0}", e.OldItems.Count);
            }
        }

        private void ItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TViewModel f)
            {
                int index = Items.IndexOf(f);
                ParamsInfo info = new()
                {
                    Index = index,
                    Item = f
                };
                OnEdit?.Invoke(info);
            }
        }
    }


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
            ActionEval = new(Eval);
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

        public RelayAction ActionEval { get; private set; }

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

        private void Eval()
        {
            //var db = BaseRules.Obj();
            //db.Evaluate();
        }

        private void FillViewModel()
        {
           ReInitDGRules();
           ReInitDGVariables();
        }
    }
}