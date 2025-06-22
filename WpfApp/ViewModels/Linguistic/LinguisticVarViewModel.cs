using ClassLibraryFLM.FuzzyLogic.Base;
using System.ComponentModel;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels.Linguistic
{
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
}