using Microsoft.UI.Xaml.Data;
using System;

namespace QuanLyMayMoc.Helper
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateReference)
            {
                return dateReference.ToString("dd/MM/yyyy"); // Định dạng ngày bạn muốn
            }
            return string.Empty; // Nếu không có ngày, trả về chuỗi rỗng
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString && DateTime.TryParse(dateString, out DateTime result))
            {
                return result;
            }
            return null;
        }
    }
}
