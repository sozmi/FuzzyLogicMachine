using System.Reflection;

namespace ClassLibraryFLM.Functions
{
    public class ParamValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public ParamValue(string name, double value)
        {
            Id = Guid.NewGuid();
            Name = name;
            Value = value;
        }

        public ParamValue(Guid id, string name, double value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public static bool operator <=(double left, ParamValue right) { return left <= right.Value; }
        public static bool operator <=( ParamValue left, double right) { return left.Value <= right; }

        public static bool operator >=(double left, ParamValue right) { return left >= right.Value; }
        public static bool operator >=(ParamValue left, double right) { return left.Value <= right; }
        public static bool operator <(double left, ParamValue right) { return left < right.Value; }
        public static bool operator >(double left, ParamValue right) { return left > right.Value; }

        public static bool operator <(ParamValue left, double right) { return left.Value < right; }
        public static bool operator >(ParamValue left, double right) { return left.Value > right; }

        public static double operator + (ParamValue left, double right)
        {
            return left.Value + right;
        }
        public static double operator +(double left, ParamValue right )
        {
            return left + right.Value;
        }
        #region operator-
        public static double operator -(ParamValue left, double right)
        {
            return left.Value - right;
        }
        public static double operator -(double left, ParamValue right)
        {
            return left - right.Value;
        }
        public static double operator -(ParamValue left, ParamValue right)
        {
            return left.Value - right.Value;
        }
        #endregion
    }
    public class FunctionInfo
    {
        public Dictionary<Guid, ParamValue> Params { get; set; }
        public ETypeFunc TypeFunc;

        public List<FunctionRuleInfo> Functions { get; set; } = [];
        public FunctionInfo(ETypeFunc eType)
        {
            TypeFunc = eType;
            Params = [];
        }
        public FunctionInfo(ETypeFunc eType, ParameterInfo[] names, out List<Guid> outVar, params double[] args) : this(eType)
        {
            outVar = [];
            for (int i = 0; i < names.Length; i++)
            {
                string key = names[i].Name;
                ParamValue val = new(key, args[i]);
                outVar.Add(val.Id);
                Params[val.Id] = val;
                //Params[key] = args[i];
            }
        }
        public FunctionInfo(ETypeFunc eType, string[] names, out List<Guid> outVar, params double[] args) : this(eType)
        {
            outVar = [];
            for (int i = 0; i < names.Length; i++)
            {
                string key = names[i];
                ParamValue val = new(key, args[i]);
                outVar.Add(val.Id);
                Params[val.Id] = val;
                //Params[key] = args[i];
            }
        }
        public FunctionInfo(ETypeFunc eType, List<FunctionRuleInfo> funcs, ParameterInfo[] names, out List<Guid> outVar, params double[] args) : this(eType, names, out outVar, args)
        {
            Functions = funcs;
        }

        public FunctionInfo(FunctionInfo a_) 
        {
            Params = a_.Params;
            Functions = a_.Functions;
            TypeFunc = a_.TypeFunc;
        }

        private static double CalcFunction(double x, FunctionInfo f)
        {
            double y = 0.0;
            foreach (var intervalFunc in f.Functions)
                if (intervalFunc.predicate(x))
                    y = intervalFunc.func(x);
            return y;
        }

        public double Calc(double x)
        {
            foreach (var intervalFunc in Functions)
                if (intervalFunc.predicate(x))
                   return intervalFunc.func(x);
            return -1;
        }

        public static FunctionInfo operator *(FunctionInfo a, FunctionInfo b)
        {
            return CalcBinaryOperation(a, b, (ay,by) => ay*by);
        }
        public static FunctionInfo Max(FunctionInfo a_, FunctionInfo b_)
        {
            return CalcBinaryOperation(a_, b_, Math.Max);
        }
        public static FunctionInfo Min(FunctionInfo a_, FunctionInfo b_)
        {
            return CalcBinaryOperation(a_, b_, Math.Min);
        }
        private static FunctionInfo Diff(FunctionInfo a_, FunctionInfo b_)
        {
            return CalcBinaryOperation(a_, b_, (ya, yb) => Math.Max(ya - yb, 0));
        }
        private static FunctionInfo SimDiff(FunctionInfo a_, FunctionInfo b_)
        {
            return CalcBinaryOperation(a_, b_, (ya, yb) => Math.Abs(ya - yb));
        }
        private static FunctionInfo Add(FunctionInfo a)
        {
            return CalcUnary(a, (ya) => 1 - ya);
        }

        public static FunctionInfo Con(FunctionInfo a)
        {
            return CalcUnary(a, Con);
        }
        public static double Con(double ya)
        {
            return Math.Pow(ya, 2);
        }
        public static double Dil(double ya)
        {
            return Math.Pow(ya, 0.5);
        }
        public static FunctionInfo Dil(FunctionInfo a)
        {
            return CalcUnary(a, Dil);
        }

        private static FunctionInfo CalcBinaryOperation(FunctionInfo a_, FunctionInfo b_, Func<double, double, double> func)
        {
            FunctionInfo a = new(a_), b = new(b_);
            FunctionInfo c = new(ETypeFunc.None)
            {
                Params = a.Params.Union(b.Params.Where(k => !a.Params.ContainsKey(k.Key))).ToDictionary(k => k.Key, v => v.Value)
            };

            FunctionRuleInfo rule = new(x => true, x =>
            {
                a.Params = c.Params;
                b.Params = c.Params;
                double ay = CalcFunction(x, a);
                double by = CalcFunction(x, b);

                return func(ay, by);
            });
            c.Functions.Add(rule);
            return c;
        }
        private static FunctionInfo CalcUnary(FunctionInfo a_, Func<double, double> func)
        {
            FunctionInfo c = new(ETypeFunc.None)
            {
                Params = a_.Params
            };

            FunctionRuleInfo rule = new(x => true, x =>
            {
                double ay = CalcFunction(x, a_);
                return func(ay);
            });
            c.Functions.Add(rule);
            return c;
        }

        public static FunctionInfo Max(List<FunctionInfo> functions)
        {
            var max = functions[0];
            for (int i = 1; i < functions.Count; i++)
                max = Max(max, functions[i]);

            return max;
        }

        public static FunctionInfo Min(List<FunctionInfo> funcs)
        {
            var min = funcs[0];
            for (int i = 1; i < funcs.Count; i++)
                min = Min(min, funcs[i]);

            return min;
        }


        public static FunctionInfo Diff(List<FunctionInfo> funcs)
        {
            var min = funcs[0];
            for (int i = 1; i < funcs.Count; i++)
                min = Diff(min, funcs[i]);

            return min;
        }

        public static FunctionInfo SimDiff(List<FunctionInfo> funcs)
        {
            var min = funcs[0];
            for (int i = 1; i < funcs.Count; i++)
                min = SimDiff(min, funcs[i]);

            return min;
        }
        public static FunctionInfo Add(List<FunctionInfo> funcs)
        {
            return Add(funcs[0]);
        }
    }
}