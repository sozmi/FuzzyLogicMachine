namespace ClassLibraryFLM.Functions
{
    public class FunctionRuleInfo(Predicate<double> predicate, Func<double, double> func)
    {
        /// <summary>
        /// Функция
        /// </summary>
        public Func<double, double> func = func;

        /// <summary>
        /// Условие при котором вычисляем функцию
        /// </summary>
        public Predicate<double> predicate = predicate;
    }
}