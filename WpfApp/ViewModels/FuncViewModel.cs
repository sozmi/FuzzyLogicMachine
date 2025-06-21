using ClassLibraryFLM.Functions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfApp.Classes;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    public class FuncViewModel : BaseViewModel
    {
        public FunctionExtraSeriesByIntervals function;
        public FuncViewModel()
        {
            function = new FunctionExtraSeriesByIntervals
            {
                Title = "Default"
            };
        }
        public FuncViewModel(FunctionExtraSeriesByIntervals function)
        {
            this.function = function;
        }

        public string Name { get => function.Title; set => SetProperty(function.Title, value, v => function.Title = v); }

        public ETypeFunc Type { get => function.EType; set => SetProperty(function.EType, value, v => function.EType = v, nameof(Type), nameof(Params)); }

        public ObservableCollection<ParamViewModel> Params { get => Getter(); }

        public bool Selected { get => selected; set => SetProperty(ref selected, value); }
        private bool selected = false;
        public bool AdditionalInfo { get => additionalInfo; set => SetProperty(ref additionalInfo, value); }
        private bool additionalInfo = false;

        private ObservableCollection<ParamViewModel> Getter()
        {
            ObservableCollection<ParamViewModel> nparams = [];
            foreach (var p in function.func.Params)
            {
                nparams.Add(new(p));
                nparams.Last().PropertyChanged += OnParamUpdate;
            }
            return nparams;
        }

        private void OnParamUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is ParamViewModel p)
            {
                function.RecalcNewParam(new(p.Id, p.Name, p.Value));
                Invoke(nameof(Params));
            }
        }
    }

    public class FuncViewModel2 : BaseViewModel
    {
        public SeriesByIntervals function;
        public FuncViewModel2()
        {
            function = new SeriesByIntervals
            {
                Title = "Default"
            };
        }
        public FuncViewModel2(SeriesByIntervals function)
        {
            this.function = function;
        }

        public string Name { get => function.Title; set => SetProperty(function.Title, value, v => function.Title = v); }

        public ETypeFunc Type { get => function.EType; set => SetProperty(function.EType, value, v => function.EType = v, nameof(Type), nameof(Params)); }

        public ObservableCollection<ParamViewModel> Params { get => Getter(); }

        private ObservableCollection<ParamViewModel> Getter()
        {
            ObservableCollection<ParamViewModel> nparams = [];
            foreach (var p in function.func.Params)
            {
                nparams.Add(new(p));
                nparams.Last().PropertyChanged += OnParamUpdate;
            }
            return nparams;
        }

        private void OnParamUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is ParamViewModel p)
            {
                function.RecalcNewParam(new(p.Id, p.Name, p.Value));
                Invoke(nameof(Params));
            }
        }
    }
}