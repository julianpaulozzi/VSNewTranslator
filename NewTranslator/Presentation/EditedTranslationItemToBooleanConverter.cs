using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using NewTranslator.Adornment;
using NewTranslator.Core;

namespace NewTranslator.Presentation
{
    public class EditedTranslationItemToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var translationItem = value as TranslationItem;
            if (translationItem == null || string.IsNullOrEmpty(translationItem.Header))
                return false;

            return translationItem.Header.Equals(Constants.TranslationCache.UserEditedItemHeader, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class EditedTranslationItemToVisibilityConverter : IValueConverter
    {
        public EditedTranslationItemToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var translationItem = value as TranslationItem;
            if (translationItem == null || string.IsNullOrEmpty(translationItem.Header))
                return FalseValue;

            var isUserEditedItemHeader = translationItem.Header.Equals(Constants.TranslationCache.UserEditedItemHeader, StringComparison.OrdinalIgnoreCase);
            return isUserEditedItemHeader ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}