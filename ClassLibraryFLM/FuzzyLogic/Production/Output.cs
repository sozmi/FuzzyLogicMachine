using ClassLibraryFLM.FuzzyLogic.Base;

namespace ClassLibraryFLM.FuzzyLogic.Production
{
    public class Output
    {
        internal static void Calc(List<InputParam> inputs, Variable target)
        {
            var db = BaseRules.Obj();
            InputParam max = inputs[0];
            foreach (var p in inputs)
            {
                db.ClearValues();
                Console.WriteLine("Установка значений:");
                foreach (var v in p.VariablesValue)
                    db.SetValue(v.Item1, v.Item2);
                p.Value = db.Evaluate(target);
                if (p.Value > max.Value)
                    max = p;
            }
            Console.WriteLine($"Наиболее подходит {max.Name}: {max.Value}");
            Console.WriteLine("Общие итоги:");
            foreach (var v in inputs)
                Console.WriteLine($"{v.Name}: {v.Value}");
        }
    }
}
