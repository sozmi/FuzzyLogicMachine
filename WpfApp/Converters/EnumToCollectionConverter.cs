using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using WpfApp.Converters.Helpers;

namespace WpfApp.Converters
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<Tuple<object, object>>))]
    public class EnumToCollectionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EnumHelper.GetAllValuesAndDescriptions(value.GetType());
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}