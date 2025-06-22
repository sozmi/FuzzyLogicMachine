using ClassLibraryFLM.Functions;
using OxyPlot;
using OxyPlot.Series;

namespace WpfApp.Classes
{
    public class FunctionExtraSeriesByIntervals : LineSeries
    {
        public ETypeFunc EType { get => eType; set => SetEType(value); }
        private ETypeFunc eType;
        public void SetEType(ETypeFunc eType_)
        {
            if (eType_ == eType)
                return;
            func = FuncFabric.GetFunc(eType_);
            eType = func.TypeFunc;
            Recalc();
        }
        public FunctionInfo func;
        public void RecalcNewParam(ParamValue key)
        {
            func.Params[key.Id] = key;
            Recalc();
        }
        public AreaSeries[] Additional = [new AreaSeries(), new AreaSeries(), new AreaSeries()];
        public double Start { get; set; }
        public double End { get; set; }
        readonly double dx;
        public FunctionExtraSeriesByIntervals(double a = 0, double b = 20, double dx_ = 0.1)
        {
            Start = a;
            End = b;
            dx = dx_;
            func = new(ETypeFunc.None);
            Additional[0].Title = "Носитель";
            Additional[1].Title = "Альфа";
            Additional[2].Title = "Ядро";
        }
        public FunctionExtraSeriesByIntervals(FunctionInfo functions, double a, double b, string title, double v3 = 0.01) : this(a, b, v3)
        {
            Title = title;
            eType = functions.TypeFunc;
            func = functions;
            Recalc();
        }

        public void Recalc()
        {
            Points.Clear();
            Additional[0].Points.Clear();
            Additional[1].Points.Clear();
            Additional[2].Points.Clear();
            for (double x = Start; x <= End + dx * 0.5; x += dx)
            {
                x = Math.Round(x, 6);
                double y = func.Calc(x);
                Points.Add(new DataPoint(x, y));
                if (y > 0.0)
                    Additional[0].Points.Add(new DataPoint(x, y)); //носитель
                if (y >= 0.5)
                    Additional[1].Points.Add(new DataPoint(x, 0.5)); // alpha - сечение
                if (y == 1.0)
                    Additional[2].Points.Add(new DataPoint(x, y)); //ядро
            }

            if (Additional[2].Points.Count == 1)
            {
                var point = Additional[2].Points[0];
                List<DataPoint> lst = [new DataPoint(point.X - dx / 100, point.Y), new DataPoint(point.X, point.Y), new DataPoint(point.X + dx / 100, point.Y)];
                Additional[2].Points.Clear();
                Additional[2].Points.AddRange(lst);
            }
        }
    }

    public class SeriesByIntervals : LineSeries
    {
        public ETypeFunc EType { get => eType; set => SetEType(value); }
        private ETypeFunc eType;
        public void SetEType(ETypeFunc eType_)
        {
            if (eType_ == eType)
                return;
            func = FuncFabric.GetFunc(eType_);
            eType = func.TypeFunc;
            Recalc();
        }
        public FunctionInfo func;
        public void RecalcNewParam(ParamValue key)
        {
            func.Params[key.Id] = key;
            Recalc();
        }
        public double Start { get; set; }
        public double End { get; set; }
        readonly double dx;
        public SeriesByIntervals(double a = 0, double b = 20, double dx_ = 0.1)
        {
            Start = a;
            End = b;
            dx = dx_;
            func = new(ETypeFunc.None);
        }
        public SeriesByIntervals(FunctionInfo functions, double a, double b, string title, double v3 = 0.01) : this(a, b, v3)
        {
            Title = title;
            eType = functions.TypeFunc;
            func = functions;
            Recalc();
        }

        public void Recalc()
        {
            Points.Clear();
            for (double x = Start; x <= End + dx * 0.5; x += dx)
            {
                x = Math.Round(x, 6);
                double y = func.Calc(x);
                Points.Add(new DataPoint(x, y));
            }
        }
    }

    public class AreaByIntervals : AreaSeries
    {
        public FunctionInfo functionInfo;
        public void RecalcNewParam(ParamValue key)
        {
            functionInfo.Params[key.Id] = key;
            Recalc();
        }
        public double Start { get; set; }
        public double End { get; set; }
        readonly double dx;
        public AreaByIntervals(FunctionInfo functions, double a, double b, string title, double dx_ = 0.001)
        {
            Start = a;
            End = b;
            dx = dx_;
            Title = title;
            functionInfo = functions;
            Recalc();
        }

        public void Recalc()
        {
            Points.Clear();
            for (double x = Start; x <= End + dx * 0.5; x += dx)
            {
                x = Math.Round(x, 6);
                double y = functionInfo.Calc(x);
                Points.Add(new DataPoint(x, y));
            }
        }
    }
}