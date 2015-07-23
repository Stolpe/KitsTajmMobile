using System;
using System.Globalization;
using Xamarin.Forms;

namespace KitsTajmMobile.Converters
{
    public class RowNumberToGridNumberConverter : IValueConverter
    {
        public int Offset { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var row = (int)value;

            return row + this.Offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
