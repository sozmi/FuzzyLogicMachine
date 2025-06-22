using ClassLibraryFLM.Functions;

namespace ClassLibraryFLM.FuzzyLogic.Base
{
    /// <summary>
    /// Нечеткое высказывание 
    /// <remarks>
    /// [Скорость автомобиля] - очень [высокая]
    /// <remarks>
    /// </summary>
    public class Comment(Variable variable, ETypeModificator modif, Term term)
    {
        public Comment(Variable variable, Term term) : this(variable, ETypeModificator.None, term) { }
        public ETypeModificator EModificator { get; set; } = modif;
        public Term Term { get; set; } = term;
        public Variable Variable { get; set; } = variable;

        public double Function()
        {
            if (Term.Value == null)
                throw new ArgumentNullException("Значение терма не установлено");
            double v = (double)Term.Value;
            return EModificator switch
            {
                ETypeModificator.Very => FunctionInfo.Con(v),
                ETypeModificator.MoreOrLess => FunctionInfo.Dil(v),
                _ => v,
            };
        }

        public void Fuzzy()
        {
            if (Variable.Value == null)
                throw new NullReferenceException("Значение входной переменной не установлено");
            Term.Fuzzy((double)Variable.Value);
        }

        public override string ToString()
        {
            string modif = EModificator.Description();
            if (modif.Length > 0)
                modif += " ";
            return $"{Variable} есть {modif}{Term}";
        }
    }
}