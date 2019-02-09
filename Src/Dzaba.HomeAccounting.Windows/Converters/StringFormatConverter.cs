using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Dzaba.HomeAccounting.Windows.Converters
{
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatParam = parameter as string;
            var format = new StringBuilder("{0");
            if (!string.IsNullOrWhiteSpace(formatParam))
            {
                format.Append(":" + formatParam +  "}");
            }
            else
            {
                format.Append("}");
            }

            return string.Format(format.ToString(), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
