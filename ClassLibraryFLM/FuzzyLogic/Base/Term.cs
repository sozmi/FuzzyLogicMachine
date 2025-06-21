using ClassLibraryFLM.Functions;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    /// <summary>
    /// Нечеткое значение лингвистической переменной.
    /// <remarks>
    /// Значения(высокая, низкая, средняя) для [Скорости автомобиля]
    /// </remarks> 
    /// </summary>
    public class Term(string name, FunctionInfo func)
    {
        public double? Value = null;
        public string Name { get; set; } = name;
        public FunctionInfo Function { get; set; } = func;
        public void ClearValue()
        {
            Value = null;
        }
        public void Fuzzy(double value_)
        {
            if (Value != null)
                return;
            Value = Function.Calc(value_);
        }
        public override string ToString()
        {
            return "[" + Name + "]";
        }
    }
}
