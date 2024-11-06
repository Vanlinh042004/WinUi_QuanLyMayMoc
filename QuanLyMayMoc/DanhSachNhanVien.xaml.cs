using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DanhSachNhanVien : Page
    {
        
        public MainViewModel ViewModel
        {
            get; set;
        }
       

        public DanhSachNhanVien()
        {
            this.InitializeComponent();

            // Initialize the list with sample data
            ViewModel = new MainViewModel();
        }



        private void OnAddEmployeeClicked(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = true; // Mở popup
        }

        // Sự kiện khi nhấn nút "Hủy"
        private void OnCancelEmployeeClicked(object sender, RoutedEventArgs e)
        {
            AddEmployeePopup.IsOpen = false; // Đóng popup
        }

      
        private void OnSaveEmployeeClicked(object sender, RoutedEventArgs e)
        {
            
            var newEmployee = new Employee
            {
                MaNhanVien = MaNhanVienInput.Text ?? "Mặc định",
                HoTen = HoTenInput.Text ?? "Không rõ",
                ChucVu = (ChucVuInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Nhân viên",
                GioiTinh = (GioiTinhInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Khác",
                NgaySinh = NgaySinhInput.SelectedDate?.Date ?? DateTime.MinValue,
                CCCD = CCCDInput.Text ?? "Không có",
                SoDienThoai = SoDienThoaiInput.Text ?? "Không có",
                Email = EmailInput.Text ?? "Không có",
                DiaChi = DiaChiInput.Text ?? "Không có",
                AnhDaiDien = string.IsNullOrWhiteSpace(AnhDaiDienInput.Text) ? "No Image" : AnhDaiDienInput.Text, // Kiểm tra xem có nhập ảnh không
            };

          
            ViewModel.Employees.Add(newEmployee);
           
            AddEmployeePopup.IsOpen = false;
        }
    }
}


