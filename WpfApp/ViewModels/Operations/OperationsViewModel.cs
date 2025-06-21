using WpfApp.ViewModels.Base;
using WpfApp.Converters.Helpers;
using ClassLibraryFLM.Functions;

namespace WpfApp.ViewModels.Operations
{
    public class OperationsViewModel : BaseViewModel
    {
        double start_ = 0, end_ = 30;
        public double Start { get => start_; set => RecalcStart(value); }
        public double End { get => end_; set => RecalcEnd(value); }
        public PlotViewModel PlotVM { get; set; }
        public DataGridFuncViewModel DataGridFuncViewModel { get; set; }
        public OperationsViewModel()
        {
            DataGridFuncViewModel = new(start_, end_);
            PlotVM = new();
            DataGridFuncViewModel.AddFunction += PlotVM.AddSerial;
            DataGridFuncViewModel.EditFunction += PlotVM.EditSerial;
            DataGridFuncViewModel.RemoveFunction += PlotVM.RemoveSerial;
            DataGridFuncViewModel.AddAreaFunction += PlotVM.AddArea;
            DataGridFuncViewModel.EditAreaFunction += PlotVM.EditArea;
            DataGridFuncViewModel.RemoveAreaFunction += PlotVM.RemoveArea;

            ActionAddDefaultFunc = new(AddDefaultFunc);
            ActionIntersection = new(Intersection);
            ActionUnion = new(Union);
            ActionDifference = new(Difference);
            ActionSymetricDifference = new(SymetricDifference);
            ActionAddition = new(Addition);

            DataGridFuncViewModel.Init();
        }


        private void RecalcStart(double value)
        {
            start_ = value;
            DataGridFuncViewModel.RecalcAllFuncByIntervals(start_, end_);
        }

        private void RecalcEnd(double value)
        {
            end_ = value;
            DataGridFuncViewModel.RecalcAllFuncByIntervals(start_, end_);
        }
        public RelayAction ActionAddDefaultFunc { get; set; }
        public RelayAction ActionIntersection { get; set; }
        public RelayAction ActionUnion { get; set; }
        public RelayAction ActionDifference { get; set; }
        public RelayAction ActionSymetricDifference { get; set; }
        public RelayAction ActionAddition { get; set; }
        private void AddDefaultFunc()
        {
            DataGridFuncViewModel.Functions.Clear();
            foreach (var enVal in EnumHelper.GetAllValuesAndDescriptions(typeof(ETypeFunc)))
                DataGridFuncViewModel.AddFunc((string)enVal.Item2, (ETypeFunc)enVal.Item1);
        }

        private void Intersection()
        {
            DataGridFuncViewModel.IntersectionVisible();
        }

        private void Union()
        {
            DataGridFuncViewModel.UnionVisible();
        }
        private void Difference()
        {
            DataGridFuncViewModel.DifferenceVisible();
        }
        private void SymetricDifference()
        {
            DataGridFuncViewModel.SymetricDifferenceVisible();
        }
        private void Addition()
        {
            DataGridFuncViewModel.AdditionVisible();
        }
    }
}