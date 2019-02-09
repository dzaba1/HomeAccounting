using System;
using System.Globalization;
using System.Windows.Data;

namespace Dzaba.HomeAccounting.Windows.Converters
{
    public class ToLocalCurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value:c}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
