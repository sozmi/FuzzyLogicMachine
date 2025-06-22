using System.ComponentModel;
using System.Globalization;

namespace WpfApp.Converters.Helpers
{
    public static class EnumHelper
    {
        public static string Description(this Enum value)
        {
            var attributes = value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return (attributes.First() as DescriptionAttribute).Description;

            TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            return ti.ToTitleCase(ti.ToLower(value.ToString().Replace("_", " ")));
        }

        public static IEnumerable<Tuple<object, object>> GetAllValuesAndDescriptions(Type t)
        {
            if (!t.IsEnum)
                throw new ArgumentException($"{nameof(t)} must be an enum type");

            return Enum.GetValues(t).Cast<Enum>().Select((e) => new Tuple<object, object>(e, e.Description())).ToList();
        }

        public static object GetValue(Type t, string descr)
        {
            return Enum.GetValues(t).Cast<Enum>().Where(e => e.Description() == descr).FirstOrDefault() ?? throw new ArgumentException("Не найдено среди enum");
        }
    }
}