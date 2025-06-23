using System.ComponentModel;

namespace ClassLibraryFLM.Functions
{
    public enum ETypeFunc
    {
        [Description("Не указан")]
        None,
        [Description("Линейная")]
        Line,
        [Description("Линейная убывающая")]
        LineDecr,
        [Description("Треугольная")]
        Triangle,
        [Description("Трапецевидная")]
        Trapetial,
        [Description("Линейная S-образная")]
        LineS,
        [Description("Линейная Z-образная")]
        LineZ,
        [Description("S-образная")]
        S,
        [Description("Z-образная")]
        Z,
        [Description("P-образная")]
        P
    }
}