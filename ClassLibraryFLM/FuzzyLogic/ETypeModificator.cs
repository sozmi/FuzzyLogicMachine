using System.ComponentModel;

namespace ClassLibraryFLM.FuzzyLogic
{
    public enum ETypeModificator
    {
        [Description("")]
        None,
        [Description("ОЧЕНЬ")]
        Very,
        [Description("БОЛЕЕ ИЛИ МЕНЕЕ")]
        MoreOrLess,
    }

    public static class EnumExtensions
    {
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString()) ?? throw new NullReferenceException("Что-то пошло не так");
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) ?? throw new NullReferenceException("Что-то не так");
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}