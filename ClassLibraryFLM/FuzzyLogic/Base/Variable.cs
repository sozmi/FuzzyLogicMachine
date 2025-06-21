using ClassLibraryFLM.Base;
using ClassLibraryFLM.Functions;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    /// <summary>
    /// Лингвистическая переменная.
    /// <remarks>
    /// Cкорость автомобиля
    /// <remarks>
    /// </summary>
    public class Variable(string name, Pair<double, double> universe, List<Term> terms)
    {
        public string Name { get; set; } = name;
        public Pair<double, double> Universe { get; set; } = universe;
        public List<Term> Terms { get; set; } = terms;
        public double? Value { get; set; } = null;

        public Variable(string name) : this(name, new(0, 100), []) { }
        public Variable(string name, Pair<double, double> universe) : this(name, universe, []) { }

        public Term AddTerm(string name, FunctionInfo func)
        {
            Terms.Add(new(name, func));
            return Terms.Last();
        }

        public Term? GetTerm(string value)
        {
            return Terms.Find(x => x.Name == value);
        }

        public void RemoveTerm(string name)
        {
            var index = Terms.FindIndex(v => v.Name == name);
            Terms.RemoveAt(index);
        }

        public override string ToString()
        {
            return "[" + Name + "]";
        }

        public FunctionInfo GetFP()
        {
            List<FunctionInfo> funcs = [];
            foreach (var term in Terms)
                funcs.Add(term.Function);
            if (funcs.Count == 0)
                return FuncFabric.GetFunc(ETypeFunc.None);
            return AreaFuncs.GetFunc(ETypeAreaFunc.Union, funcs);
        }

        public void ClearValues()
        {
            Value = null;
            foreach (var term in Terms)
                term.ClearValue();
        }
    }
}
