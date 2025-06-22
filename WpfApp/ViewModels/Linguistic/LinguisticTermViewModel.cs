using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;
using System.ComponentModel;
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
}