using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;

namespace ClassLibraryFLM.FuzzyLogic.Production
{
    /// <summary>
    /// База правил
    /// </summary>
    public class BaseRules
    {
        readonly List<Comment> fuzzyComments = [];
        public List<Variable> LinguisticVars { get; private set; } = [];

        public List<Rule> Rules { get; private set; } = [];

        static BaseRules? instance = null;

        public static BaseRules Obj()
        {
            instance ??= new BaseRules();
            return instance;
        }
        private BaseRules() { }

        public Variable AddLinguisticVariable(string name, Pair<double, double> interval)
        {
            if (GetLinguisticVariable(name) != null) throw new NullReferenceException("Переменная уже создана");
            return AddLinguisticVariable(new(name, interval));
        }
        public Variable AddLinguisticVariable(Variable variable_)
        {
            if (GetLinguisticVariable(variable_.Name) != null) throw new NullReferenceException("Переменная уже создана");
            LinguisticVars.Add(variable_);
            return LinguisticVars.Last();
        }

        public void RemoveLinguisticVariable(string name)
        {
            var index = LinguisticVars.FindIndex(v => v.Name == name);
            LinguisticVars.RemoveAt(index);
        }
        public Rule AddRule(Rule rule)
        {
            if (Rules.Find(r => r.ToString() == rule.ToString()) != null)
                throw new Exception("Такое правило уже существует");
            Rules.Add(rule);
            rule = Rules.Last();
            return rule;
        }

        public void RemoveRule(Rule rule)
        {
            var index = Rules.FindIndex(v => v == rule);
            Rules.RemoveAt(index);
        }

        public Variable? GetLinguisticVariable(string name)
        {
            var v = LinguisticVars.Find(x => x.Name == name);
            return v;
        }

        public Comment GetOrCreateFuzzyComment(Comment fuzzy)
        {
            var comment = fuzzyComments.Find(x => x.ToString() == fuzzy.ToString());
            if (comment == null)
            {
                fuzzyComments.Add(fuzzy);
                return fuzzyComments.Last();
            }
            return comment;
        }

        public void SetValue(Variable var, double value)
        {
            int id = LinguisticVars.IndexOf(var);
            if (id != -1)
            {
                LinguisticVars[id].Value = value;
                Console.WriteLine($"{var} = {value}");
                return;
            }
            throw new ArgumentException("Значение не установлено");
        }
        private void PrintInfo()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine(ToString());
            Console.WriteLine("------------------------------------------------------");
        }

        public double Evaluate(Variable target, out FunctionInfo func)
        {
            PrintInfo();
            Fuzzy();

            PrintInfo();
            Aggregation();

            PrintInfo();
            Activation();

            PrintInfo();
            func = Acсumulation(target.Name);

            PrintInfo();
            return Unfuzzy(func, target);
        }


        public static double Unfuzzy(FunctionInfo function, Variable variable)
        {
            Console.WriteLine("");
            Console.WriteLine("Дефаззификация");
            Console.WriteLine("");
            double dx = 0.1;
            double sumxfx = 0, sumfx = 0;
            for (double x = variable.Universe.Item1; x <= variable.Universe.Item2 + dx * 0.5; x += dx)
            {
                x = Math.Round(x, 6);
                double y = function.Calc(x);
                sumxfx += x * y;
                sumfx += y;
            }
            //var model = new PlotModel { Title = variable.Name, Background = OxyColor.FromRgb(255, 255, 255) };
            //model.Series.Add(new FunctionSeries(function.Calc, variable.Universe.Item1, variable.Universe.Item2, 0.1, "Sin(x)"));
            //PngExporter.Export(model, "plot.png", 1280, 720);
            double result = sumxfx / sumfx;
            Console.WriteLine($"{variable} = {result}");
            return result;
        }

        public FunctionInfo Acсumulation(string name)
        {
            // строим ФП для выходных лингвистических переменных
            Console.WriteLine("");
            Console.WriteLine("Аккумуляция");
            Console.WriteLine("");

            List<FunctionInfo> funcs = [];
            foreach (var rule in Rules)
            {
                if (rule.OutVarFunction != null && rule.Output.Variable.Name == name)
                    funcs.Add(rule.OutVarFunction);
            }
            Console.WriteLine($"Объединено {funcs.Count} функций для выходной переменной {name}");
            return AreaFuncs.GetFunc(ETypeAreaFunc.Union, funcs);
        }

        public void Fuzzy()
        {
            // На входе известны значения всех лингвистических переменных
            // Необходимо вычислить значения функций для всех термов в нечетких высказываниях
            Console.WriteLine("");
            Console.WriteLine("Фаззификация");
            Console.WriteLine("");
            foreach (var rule in Rules)
                foreach (var comment in rule.Condition.fuzzyComments)
                    comment.Fuzzy();

            foreach (var v in LinguisticVars)
                foreach (Term term in v.Terms)
                {
                    string value = "?";
                    if (term.Value is double d)
                        value = Math.Round(d, 3).ToString();
                    Console.WriteLine($"{v} - {term}: {value}");
                }

        }

        public void Aggregation()
        {
            //процедура определения степени истинности условий по каждому из правил системы нечеткого вывода
            Console.WriteLine("");
            Console.WriteLine("Агрегация");
            Console.WriteLine("");

            foreach (var rule in Rules)
                rule.Aggregation();
        }

        public void Activation()
        {
            Console.WriteLine("");
            Console.WriteLine("Активизация");
            Console.WriteLine("");
            //процесс нахождения степени истинности каждого из подзаключений правил нечетких продукций
            foreach (var rule in Rules)
                rule.Active();
            Console.WriteLine($"Выполнена min-активизация правил {Rules.Count}");
        }

        public override string ToString()
        {
            string s = "";
            foreach (var lv in LinguisticVars)
            {
                var v = lv.Value != null ? $"{Math.Round((double)lv.Value, 2)}" : "";
                s += $"{lv} = {v}: ";
                foreach (var t in lv.Terms)
                    s += $"{t}";
                s += "\n";
            }
            s += "\n";

            foreach (var rule in Rules)
            {
                s += rule.ToString() + "\n";
            }

            s += "\n";

            foreach (var comment in fuzzyComments)
                s += $"{comment}\n";

            return s.TrimEnd('\n');
        }

        public void Clear()
        {
            LinguisticVars.Clear();
            Rules.Clear();
            fuzzyComments.Clear();
        }

        public void ClearValues()
        {
            foreach (var rule in Rules)
                rule.ClearValues();
            foreach (var lv in LinguisticVars)
                lv.ClearValues();
        }
    }
}
