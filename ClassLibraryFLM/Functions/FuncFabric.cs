using System.Reflection;

namespace ClassLibraryFLM.Functions
{
    public static class FuncFabric
    {
        public static FunctionInfo GetLine(double a, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.Line, names, out vars, a);
            Guid ka = vars[0];
            info.Functions =
            [
                new FunctionRuleInfo(x => true, x => info.Params[ka].Value),
            ];
            return info;
        }
        public static FunctionInfo GetLineDecr(double a, double b, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.LineDecr, names, out vars, a, b);
            Guid ka = vars[0];
            Guid kb = vars[1];
            info.Functions =
            [
                new FunctionRuleInfo(x => x<=info.Params[ka], x => 1),
                new FunctionRuleInfo(x => info.Params[ka]<x && x <= info.Params[kb], x => (info.Params[kb] - x)/(info.Params[kb]-info.Params[ka] )),
                new FunctionRuleInfo(x => x>=info.Params[kb].Value, x => 0),
            ];
            return info;
        }
        public static FunctionInfo GetTrapetial(double a, double b, double c, double d, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.Trapetial, names, out vars, a, b, c, d);
            Guid ka = vars[0], kb = vars[1], kc = vars[2], kd = vars[3];
            info.Functions =
            [
                new FunctionRuleInfo(x => x <= info.Params[ka], x => 0),
                new FunctionRuleInfo(x => info.Params[ka] < x && x <= info.Params[kb], x => (x - info.Params[ka]) / (info.Params[kb] - info.Params[ka])),
                new FunctionRuleInfo(x => info.Params[kb] < x && x <= info.Params[kc], x => 1),
                new FunctionRuleInfo(x => info.Params[kc] < x && x <= info.Params[kd], x => (info.Params[kd] - x) / (info.Params[kd] - info.Params[kc])),
                new FunctionRuleInfo(x => info.Params[kd] < x, x => 0)
            ];
            return info;
        }
        public static FunctionInfo GetLineZ(double a, double b, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.LineZ, names, out vars, a, b);
            Guid ka = vars[0], kb = vars[1];
            info.Functions =
            [
                new FunctionRuleInfo(x => x <= info.Params[ka], x => 1),
                new FunctionRuleInfo(x => info.Params[ka] < x && x < info.Params[kb], x => (info.Params[kb] - x) / (info.Params[kb] - info.Params[ka])),
                new FunctionRuleInfo(x => info.Params[kb] <= x, x => 0)
            ];
            return info;
        }

        public static FunctionInfo GetZ(double a, double b, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.Z, names, out vars, a, b);
            Guid ka = vars[0], kb = vars[1];
            info.Functions =
            [
                new FunctionRuleInfo(x => x <= info.Params[ka], x => 1),
                new FunctionRuleInfo(x => info.Params[ka] < x && x < info.Params[kb], x => 0.5 + 0.5 * Math.Cos((x-info.Params[ka])/(info.Params[kb]-info.Params[ka])*Math.PI)),
                new FunctionRuleInfo(x => info.Params[kb] <= x, x => 0)
            ];
            return info;
        }
        public static FunctionInfo GetLineS(double a, double b, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.LineS, names, out vars, a, b);
            Guid ka = vars[0], kb = vars[1];
            info.Functions =
            [
                new FunctionRuleInfo(x => x <= info.Params[ka], x => 0),
                new FunctionRuleInfo(x => info.Params[ka] < x && x < info.Params[kb], x => (x - info.Params[ka]) / (info.Params[kb] - info.Params[ka])),
                new FunctionRuleInfo(x => info.Params[kb] <= x, x => 1)
            ];
            return info;
        }

        public static FunctionInfo GetS(double a, double b, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.S, names, out vars, a, b);
            Guid ka = vars[0], kb = vars[1];
            info.Functions =
            [
                new FunctionRuleInfo(x => x < info.Params[ka], x => 0),
                new FunctionRuleInfo(x => info.Params[ka] <= x && x <= info.Params[kb], x => 0.5 + 0.5 * Math.Cos((x - info.Params[kb]) / (info.Params[kb] - info.Params[ka])*Math.PI)),
                new FunctionRuleInfo(x => info.Params[kb] < x, x => 1)
            ];
            return info;
        }

        private static string[] GetNamesParam(ParameterInfo[] parameters)
        {
            string[] names = new string[parameters.Length - 1];
            for (int i = 0; i < names.Length; i++)
            {
                string? name = parameters[i].Name;
                if (name != null)
                    names[i] = name;
            }

            return names;
        }

        public static FunctionInfo GetTriangle(double a, double b, double c, params string[] names)
        {
            if (names.Length == 0)
            {
                var parameters = MethodBase.GetCurrentMethod()?.GetParameters() ?? throw new Exception("Магия");
                names = GetNamesParam(parameters);
            }
            List<Guid> vars = [];
            FunctionInfo info = new(ETypeFunc.Triangle, names, out vars, a, b, c);
            Guid ka = vars[0], kb = vars[1], kc = vars[2];
            info.Functions =
            [
                new FunctionRuleInfo(x => x <= info.Params[ka], x => 0),
                new FunctionRuleInfo(x => info.Params[ka] < x && x <= info.Params[kb], x => (x - info.Params[ka]) / (info.Params[kb] - info.Params[ka])),
                new FunctionRuleInfo(x => info.Params[kb] < x && x <= info.Params[kc], x => (info.Params[kc] - x) / (info.Params[kc] - info.Params[kb])),
                new FunctionRuleInfo(x => info.Params[kc] < x, x => 0)
            ];
            return info;
        }

        public static FunctionInfo GetP(double a, double b, double c, double d)
        {
            var s = GetS(a, b, nameof(a), nameof(b));
            var z = GetZ(c, d, nameof(c), nameof(d));

            FunctionInfo info = s * z;
            info.TypeFunc = ETypeFunc.P;
            return info;

        }

        public static FunctionInfo GetFunc(ETypeFunc eType)
        {
            return eType switch
            {
                ETypeFunc.Triangle => GetTriangle(2, 5, 10),
                ETypeFunc.Trapetial => GetTrapetial(2, 5, 10, 15),
                ETypeFunc.LineS => GetLineS(5, 10),
                ETypeFunc.LineZ => GetLineZ(5, 10),
                ETypeFunc.S => GetS(5, 10),
                ETypeFunc.Z => GetZ(5, 10),
                ETypeFunc.P => GetP(2, 5, 10, 15),
                ETypeFunc.Line => GetLine(0.5),
                ETypeFunc.LineDecr => GetLineDecr(3, 8),
                _ => new(ETypeFunc.None),
            };
        }
    }

    public static class AreaFuncs
    {
        public static FunctionInfo GetFunc(ETypeAreaFunc eTypeAreaFunc, List<FunctionInfo> funcs)
        {
            return eTypeAreaFunc switch
            {
                ETypeAreaFunc.Intersection => FunctionInfo.Min(funcs),
                ETypeAreaFunc.Union => FunctionInfo.Max(funcs),
                ETypeAreaFunc.Difference => FunctionInfo.Diff(funcs),
                ETypeAreaFunc.SymetricDifference => FunctionInfo.SimDiff(funcs),
                ETypeAreaFunc.Addition => FunctionInfo.Add(funcs),
                _ => new(ETypeFunc.None)
            };
        }
    }
}
