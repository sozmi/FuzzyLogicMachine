﻿using System.Windows.Data;

namespace WpfApp.Converters;

public class ComparisonConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return value.Equals(true) == true ? parameter : Binding.DoNothing;
    }
}
