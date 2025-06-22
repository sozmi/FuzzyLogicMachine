using ClassLibraryFLM.Functions;
using ClassLibraryFLM.FuzzyLogic.Base;

namespace ClassLibraryFLM.FuzzyLogic.Production
{

    public class OutputInfo
    {
        public OutputInfo(InputParam start) { Best = start; }
        public InputParam Best;
        public List<InputParam> Params = [];
        public List<FunctionInfo> Functions = [];

    }
    public class Output
    {
        public static OutputInfo Calc(List<InputParam> inputs, Variable target, bool isMaxOrder = true)
        {
            OutputInfo output = new(inputs[0]);
            var db = BaseRules.Obj();
            foreach (var p in inputs)
            {
                db.ClearValues();
                Console.WriteLine("Установка значений:");
                foreach (var v in p.VariablesValue)
                    db.SetValue(v.Item1, v.Item2);
                p.Value = db.Evaluate(target, out FunctionInfo f);
                output.Params.Add(p);
                output.Functions.Add(f);
                if ((isMaxOrder && p.Value > output.Best.Value) || (!isMaxOrder && p.Value < output.Best.Value))
                    output.Best = p;

            }
            Console.WriteLine($"Наиболее подходит {output.Best.Name}: {output.Best.Value}");
            Console.WriteLine("Общие итоги:");
            foreach (var v in inputs)
                Console.WriteLine($"{v.Name}: {v.Value}");
            return output;
        }
    }
}
