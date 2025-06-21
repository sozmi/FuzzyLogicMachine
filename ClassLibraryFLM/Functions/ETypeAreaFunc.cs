using System.ComponentModel;

namespace ClassLibraryFLM.Functions
{
    public enum ETypeAreaFunc
    {
        [Description("Пересечение")]
        Intersection,
        [Description("Объединение")]
        Union,
        [Description("Разность")]
        Difference,
        [Description("Симметрическая разность")]
        SymetricDifference,
        [Description("Дополнение")]
        Addition,
        [Description("Количество")]
        Count
    }
}