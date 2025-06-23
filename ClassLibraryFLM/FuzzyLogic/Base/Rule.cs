using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Production;
using ClassLibraryFLM.Parser;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    public class Rule
    {
        public FunctionInfo? OutVarFunction = null;
        public Condition Condition { get; set; }
        public Comment Output { get; set; }

        public Rule(string condition, string target)
        {
            Condition = new Condition(condition);
            SyntaxAnalizer s = new(target);
            var vars = s.GetInputVars();
            if(vars.Count != 0)
            {
                Output = BaseRules.Obj().GetOrCreateFuzzyComment(vars.First());
                return;
            }
            if (condition.Length > 0 && condition.Length>0)
                throw new ArgumentException("Не найдено заключение для правила");
        }
        public Rule(Condition condition, Comment output)
        {
            Condition = condition;
            Output = output;
        }

        public void SetCondition(string v)
        {
            Condition = new Condition(v);
        }

        public void SetOutput(string obj)
        {
            SyntaxAnalizer s = new(obj);
            Output = BaseRules.Obj().GetOrCreateFuzzyComment(s.GetInputVars()[0]);
        }

        public override string ToString()
        {
            return $"ЕСЛИ {Condition} ТО {Output}";
        }
        public void Active()
        {
            double val = Condition.Value ?? throw new ArgumentNullException("Значение для условия не установлено");
            OutVarFunction = AreaFuncs.GetFunc(ETypeAreaFunc.Intersection, [Output.Term.Function, FuncFabric.GetLine(val)]); // min-активизация
        }

        public void Aggregation()
        {
            Condition.Aggregation();
        }

        internal void ClearValues()
        {
            OutVarFunction = null;
            Condition.ClearValues();
        }
    }
}