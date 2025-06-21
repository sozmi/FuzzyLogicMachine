using ClassLibraryFLM.Parser;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    /// <summary>
    /// Условие в нечетком правиле
    /// </summary>
    public class Condition
    {
        public List<Comment> fuzzyComments;
        public SyntaxAnalizer Analizer { get; set; }
        public double? Value { get; set; } = null;

        public Condition(string input_)
        {
            Analizer = new(input_);
            fuzzyComments = Analizer.GetInputVars();
        }
        public override string ToString()
        {
            return Analizer.Input;
        }
        public void Aggregation()
        {
            if (Value != null)
                return;
            Value = Analizer.Eval();
            var v = Math.Round((double)Value, 2);
            Console.WriteLine($"{this}: {v}");
        }
        internal void ClearValues()
        {
            Value = null;
        }
    }
}