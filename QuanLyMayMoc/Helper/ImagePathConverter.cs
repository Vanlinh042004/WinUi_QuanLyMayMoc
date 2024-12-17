using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string imageFileName = value as string;
            if (!string.IsNullOrEmpty(imageFileName))
            {
                // Trả về đường dẫn đầy đủ đến ảnh trong thư mục LocalFolder
                return $"ms-appx:///Assets/{imageFileName}";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value; // Không cần chuyển ngược
        }
    }

}
