using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HuongDanSuDung : Page
    {
        public ObservableCollection<string> TinhNangChinh { get; set; }
        public ObservableCollection<string> ThaoTac { get; set; }
        public ObservableCollection<string> Meo { get; set; }
        public HuongDanSuDung()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Hướng dẫn sử dụng phần mềm");
            };
            TinhNangChinh = new ObservableCollection<string>
            {
                "•   Nhập thông tin khách hàng và dịch vụ đã sử dụng.",
                "•   Gợi ý khách hàng đã sử dụng dịch vụ trước đó.",
                "•   Tổng hợp máy móc và dịch vụ theo tháng, năm.",
                "•   Chia sẻ và lưu dự án với nhiều tùy chọn.",
                "•   Lọc dịch vụ theo ngày/tháng để theo dõi chi tiết.",
                "•   Xóa hoặc lưu dự án một cách dễ dàng.",
                "•   Thêm, sửa, xóa thông tin khách hàng và dịch vụ.",
                "•   Thêm, sửa, xóa thông tin Lõi và Linh Kiện.",

            };
            ThaoTac = new ObservableCollection<string>
            {
                "•   Chọn **Tạo Dự Án Mới** để bắt đầu nhập thông tin khách hàng và dịch vụ.",
                "•   Chọn **Mở Dự Án** để mở và tiếp tục làm việc với dữ liệu cũ.",
                "•   Chọn **Lưu Dự Án** để lưu thông tin khách hàng và dịch vụ.",
                "•   Chọn **Lưu Dự Với Tên Khác** để lưu thông tin khách hàng và dịch vụ với Tên dự án khác.",
                "•   Chọn **Xóa Dự Án** để xóa thông tin khách hàng và dịch vụ.",
                "•   Chọn **Thoát** để thoát khỏi phần mềm",
                "•   Thao tác Thêm, Sửa, Xóa bằng cách sử dụng các nút bấm."

            };
            Meo = new ObservableCollection<string>
            {
                "•   Lưu dự án thường xuyên để tránh mất dữ liệu khi xảy ra sự cố."
            };
        }
    }
}
