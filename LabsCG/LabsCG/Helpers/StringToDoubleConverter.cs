namespace LabsCG.Helpers
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as double? ?? 0d;

            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            if (string.IsNullOrEmpty(stringValue))
                return 0d;

            return !double.TryParse(stringValue, out var amount) ? 0d : amount;
        }
    }
}
