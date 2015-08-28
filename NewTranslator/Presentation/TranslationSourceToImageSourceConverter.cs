using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using NewTranslator.Core;

namespace NewTranslator.Presentation
{
    public class TranslationSourceToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var translationSource = value as string;
            if (!string.IsNullOrEmpty(translationSource))
            {
                switch (translationSource)
                {
                    case Constants.TranslatorNames.Google:
                        return GetImage("google_translate_12x12.png");
                    case Constants.TranslatorNames.Bing:
                        return GetImage("bing_translate_12x12.png");
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static BitmapImage GetImage(string name)
        {
            return new BitmapImage(new Uri("pack://application:,,,/NewTranslator;component/Images/" + name));
        }
    }
}