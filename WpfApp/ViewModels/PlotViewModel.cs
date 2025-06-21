using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using WpfApp.Classes;
using WpfApp.ViewModels.Base;

namespace WpfApp.ViewModels
{
    public class PlotViewModel : BaseViewModel
    {
        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; SetProperty(ref plotModel, value); }
        }

        public PlotViewModel()
        {
            var model = new PlotModel
            {
                Title = "График функций принадлежности",
            };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Достоверность",
                Maximum = 1,
                Minimum = 0
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Значения",
            });
            model.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.TopRight,
                LegendFontSize = 12
            });
            PlotModel = model;
        }

        public void AddSerial(FuncViewModel? func, IndexsInfo? info_ = null)
        {
            if (func == null)
                return;
            FunctionExtraSeriesByIntervals series = func.function;
            if (info_ is IndexsInfo inf)
            {
                PlotModel.Series.Insert(inf.start, series);
                for (int i = 0; i < inf.additional.Count; i++)
                {
                    int indexInSeries = inf.additional[i];
                    PlotModel.Series.Insert(indexInSeries, series.Additional[i]);
                    PlotModel.Series[indexInSeries].RenderInLegend = func.AdditionalInfo;
                    PlotModel.Series[indexInSeries].IsVisible = func.AdditionalInfo;
                }
                PlotModel.InvalidatePlot(true);
            }

        }

        public void EditSerial(FuncViewModel? func, IndexsInfo? info = null)
        {
            if (func == null)
                return;
            FunctionExtraSeriesByIntervals series = func.function;
            if (info is IndexsInfo inf)
            {
                PlotModel.Series[inf.start] = series;
                for (int i = 0; i < inf.additional.Count; i++)
                {
                    int indexInSeries = inf.additional[i];
                    PlotModel.Series[indexInSeries] = series.Additional[i];
                    PlotModel.Series[indexInSeries].RenderInLegend = func.AdditionalInfo;
                    PlotModel.Series[indexInSeries].IsVisible = func.AdditionalInfo;
                }
            }
            PlotModel.InvalidatePlot(true);
        }

        public void RemoveSerial(FuncViewModel? _, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf)
            {
                PlotModel.Series.RemoveAt(inf.start);
                for (int i = 0; i < inf.additional.Count; i++)
                {
                    int indexInSeries = inf.additional[i] - (i + 1);
                    PlotModel.Series.RemoveAt(indexInSeries);
                }
                PlotModel.InvalidatePlot(true);
            }

        }

        public void AddArea(AreaByIntervals? func, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf && func is AreaByIntervals series)
            {
                PlotModel.Series.Insert(inf.start, series);
                PlotModel.InvalidatePlot(true);
            }
        }

        public void EditArea(AreaByIntervals? func, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf && func is AreaByIntervals series)
            {
                PlotModel.Series[inf.start] = series;
                PlotModel.InvalidatePlot(true);
            }
        }

        public void RemoveArea(AreaByIntervals? _, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf)
            {
                PlotModel.Series.RemoveAt(inf.start);
                PlotModel.InvalidatePlot(true);
            }
        }
    }

    public class PlotViewModelTest : BaseViewModel
    {
        private PlotModel plotModel;
        public PlotModel PlotModel
        {
            get { return plotModel; }
            set { plotModel = value; SetProperty(ref plotModel, value); }
        }

        public PlotViewModelTest()
        {
            var model = new PlotModel
            {
                Title = "График функций принадлежности",
            };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                Title = "Достоверность",
                Maximum = 1,
                Minimum = 0
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                Title = "Значения",
            });
            model.Legends.Add(new Legend()
            {
                LegendPosition = LegendPosition.TopRight,
                LegendFontSize = 12
            });
            PlotModel = model;
        }

        public void AddSerial(SeriesByIntervals series)
        {
            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);
        }

        public void EditSerial(FuncViewModel? func, IndexsInfo? info = null)
        {
            if (func == null)
                return;
            FunctionExtraSeriesByIntervals series = func.function;
            if (info is IndexsInfo inf)
            {
                PlotModel.Series[inf.start] = series;
                for (int i = 0; i < inf.additional.Count; i++)
                {
                    int indexInSeries = inf.additional[i];
                    PlotModel.Series[indexInSeries] = series.Additional[i];
                    PlotModel.Series[indexInSeries].RenderInLegend = func.AdditionalInfo;
                    PlotModel.Series[indexInSeries].IsVisible = func.AdditionalInfo;
                }
            }
            PlotModel.InvalidatePlot(true);
        }

        public void RemoveSerial(FuncViewModel? _, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf)
            {
                PlotModel.Series.RemoveAt(inf.start);
                for (int i = 0; i < inf.additional.Count; i++)
                {
                    int indexInSeries = inf.additional[i] - (i + 1);
                    PlotModel.Series.RemoveAt(indexInSeries);
                }
                PlotModel.InvalidatePlot(true);
            }

        }

        public void AddArea(AreaByIntervals series)
        {
            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);
        }

        public void EditArea(AreaByIntervals? func, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf && func is AreaByIntervals series)
            {
                PlotModel.Series[inf.start] = series;
                PlotModel.InvalidatePlot(true);
            }
        }

        public void RemoveArea(AreaByIntervals? _, IndexsInfo? info = null)
        {
            if (info is IndexsInfo inf)
            {
                PlotModel.Series.RemoveAt(inf.start);
                PlotModel.InvalidatePlot(true);
            }
        }

        internal void Clear()
        {
            PlotModel.Series.Clear();
            PlotModel.InvalidatePlot(true);
        }
    }
}