using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NewTranslator.Presentation
{
    public class StringEmptyToVisibilityConverter : IValueConverter
    {
        public StringEmptyToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }

        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseValue;

            var eval = value as string;
            return string.IsNullOrEmpty(eval) ? FalseValue : TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}