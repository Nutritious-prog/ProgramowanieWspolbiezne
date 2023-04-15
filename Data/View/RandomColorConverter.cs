using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace View
{
    public class RandomColorConverter : IValueConverter
    {

        private static readonly Random random = new Random();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            byte[] bytes = new byte[3]; // tablica 3 losowych bajtów (3 składowych dla koloru)
            random.NextBytes(bytes);
            return new SolidColorBrush(Color.FromRgb(bytes[0], bytes[1], bytes[2]));

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
