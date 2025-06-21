using ClassLibraryFLM.Base;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    public class InputParam(string name, List<Pair<Variable, double>> vars)
    {
        public string Name { get; set; } = name;
        public List<Pair<Variable, double>> VariablesValue { get; set; } = vars;
        public double Value { get; set; } = 0.0;
    }
}
