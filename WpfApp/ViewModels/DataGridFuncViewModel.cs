using ClassLibraryFLM.Functions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using WpfApp.Classes;
using WpfApp.Converters.Helpers;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    public delegate void FuncHandler(FuncViewModel? function, IndexsInfo? info = null);
    public delegate void FuncAreaHandler(AreaByIntervals? function, IndexsInfo? info = null);
    public class DataGridFuncViewModel : CollectionViewModel
    {
        public event FuncHandler? AddFunction, RemoveFunction, EditFunction;
        public event FuncAreaHandler? AddAreaFunction, RemoveAreaFunction, EditAreaFunction;
        private Tuple<double, double> interval = new(0, 30);
        public ObservableCollection<FuncViewModel> Functions { get; set; }
        public AreaByIntervals?[] AdditionalFuncs { get; set; }
        public DataGridFuncViewModel(double start_, double end_)
        {
            interval = new(start_, end_);
            Functions = [];
            AdditionalFuncs = new AreaByIntervals?[(int)ETypeAreaFunc.Count];
            Functions.CollectionChanged += DataCollectionChanged;
        }

        public void Init()
        {
            for (ETypeAreaFunc etype = ETypeAreaFunc.Intersection; etype < ETypeAreaFunc.Count; etype++)
                Init(etype);
        }
        private void DataCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (FuncViewModel v in e.NewItems)
                    OnAddFunc(v, e.NewStartingIndex);
                Debug.WriteLine("Добавленных элементов: {0}", e.NewItems.Count);
            }

            if (e.OldItems != null)
            {
                foreach (FuncViewModel v in e.OldItems)
                    OnRemoveFunc(v, e.OldStartingIndex);
                Debug.WriteLine("Удалённых элементов: {0}", e.OldItems.Count);
            }
        }
        private void OnAddFunc(FuncViewModel f, int index)
        {
            f.PropertyChanged += EditFunc;
            f.function.Start = interval.Item1;
            f.function.End = interval.Item2;
            IndexsInfo info = new()
            {
                start = index * 4,
                additional = []
            };
            for (int i = 1; i <= f.function.Additional.Length; i++)
                info.additional.Add(info.start + i);
            AddFunction?.Invoke(f, info);
        }

        public void AddFunc(string name, ETypeFunc eType = ETypeFunc.None)
        {
            var info = FuncFabric.GetFunc(eType);
            FunctionExtraSeriesByIntervals series = new(info, interval.Item1, interval.Item2, name);
            FuncViewModel func = new(series);
            Functions.Add(func);
        }

        private void OnRemoveFunc(FuncViewModel f, int index)
        {
            IndexsInfo info = new()
            {
                start = index * 4,
                additional = []
            };
            for (int i = 1; i <= f.function.Additional.Length; i++)
                info.additional.Add(info.start + i);
            RemoveFunction?.Invoke(null, info);
        }
        public void EditFunc(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is FuncViewModel f)
            {
                int index = Functions.IndexOf(f);
                IndexsInfo info = new()
                {
                    start = index * 4,
                    additional = []
                };
                for (int i = 1; i <= f.function.Additional.Length; i++)
                    info.additional.Add(info.start + i);
                EditFunction?.Invoke(f, info);
            }
        }
        public void RecalcAllFuncByIntervals(double start, double end)
        {
            interval = new(start, end);
            foreach (FuncViewModel f in Functions)
            {
                f.function.Start = interval.Item1;
                f.function.End = interval.Item2;
                f.function.Recalc();
                EditFunc(f, new PropertyChangedEventArgs("intervals"));
            }
        }

        private List<FunctionInfo> GetSelected()
        {
            List<FunctionInfo> lst = [];
            foreach (FuncViewModel f in Functions)
                if (f.Selected)
                    lst.Add(f.function.func);
            return lst;
        }
        private void Init(ETypeAreaFunc eTypeArea)
        {
            FunctionInfo func = new(ETypeFunc.None);
            int index = (int)eTypeArea;
            AdditionalFuncs[index] = new(func, interval.Item1, interval.Item2, EnumHelper.Description(eTypeArea));
            IndexsInfo info = new()
            {
                start = index
            };
            AddAreaFunction?.Invoke(AdditionalFuncs[index], info);
        }

        public void IntersectionVisible()
        {
            EditVisible(ETypeAreaFunc.Intersection);
        }

        public void UnionVisible()
        {
            EditVisible(ETypeAreaFunc.Union);
        }

        private void EditVisible(ETypeAreaFunc eTypeArea)
        {
            var lst = GetSelected();
            if (eTypeArea == ETypeAreaFunc.Addition && lst.Count != 1)
            {
                MessageBox.Show("Для операции дополнения должна быть выбрана только одна функция");
                return;
            }    
            FunctionInfo func = lst.Count == 0 ? new(ETypeFunc.None) : AreaFuncs.GetFunc(eTypeArea, lst);
            int index = (int)eTypeArea;
            AdditionalFuncs[index] = new(func, interval.Item1, interval.Item2, EnumHelper.Description(eTypeArea));
            IndexsInfo info = new()
            {
                start = Functions.Count * 4 + index
            };
            EditAreaFunction?.Invoke(AdditionalFuncs[index], info);
        }

        internal void AdditionVisible()
        {
            EditVisible(ETypeAreaFunc.Addition);
        }

        internal void DifferenceVisible()
        {
            EditVisible(ETypeAreaFunc.Difference);
        }

        internal void SymetricDifferenceVisible()
        {
            EditVisible(ETypeAreaFunc.SymetricDifference);
        }
    }
}