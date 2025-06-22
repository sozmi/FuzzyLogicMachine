using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfApp.Converters.Helpers;

namespace WpfApp.Converters
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<Tuple<object, object>>))]
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public BindingBase SourceEnum { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetAllValuesAndDescriptions(value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetValue(targetType, value.ToString());
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class EnumToItemsSource(Type type) : MarkupExtension
    {
        private readonly Type _type = type;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = e, DisplayName = EnumHelper.Description((Enum)e) });
        }
    }
}