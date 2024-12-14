using System;
using System.Globalization;
using Microsoft.UI.Xaml.Data;

namespace QuanLyMayMoc
{
    public class CurrencyFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && double.TryParse(value.ToString(), out double number))
            {
                return number.ToString("N0", new CultureInfo("vi-VN"));
            }
            return value; // Trả về giá trị gốc nếu không chuyển đổi được.
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

