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
using QuanLyMayMoc.ViewModel;
using QuanLyMayMoc.Model;
using System.Data;
using Npgsql;

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


        private async void OnSaveEmployeeClicked(object sender, RoutedEventArgs e)
        {

            var newEmployee = new Employee
            {
                MaNhanVienDuAn = MaNhanVienInput.Text + AppData.ProjectID,
                MaNhanVien = MaNhanVienInput.Text ?? "Không có",
                HoTen = HoTenInput.Text ?? "Không rõ",
                GioiTinh = (GioiTinhInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Khác",
                NgaySinh = NgaySinhInput.SelectedDate?.Date ?? DateTime.MinValue,
                DiaChi = DiaChiInput.Text ?? "Không có",
                SoDienThoai = SoDienThoaiInput.Text ?? "Không có",  
                Email = EmailInput.Text ?? "Không có",
                TrangThai = TrangThaiInput.Text ?? "Không có",
                PhongBan = PhongBanInput.Text ?? "Không có",
                CCCD = CCCDInput.Text ?? "Không có",
                AnhDaiDien = string.IsNullOrWhiteSpace(AnhDaiDienInput.Text) ? "No Image" : AnhDaiDienInput.Text,
                MaDuAn = AppData.ProjectID,
            };
            try
            {
                ViewModel.Employees.Add(newEmployee);
                ViewModel.InsertToEmployees(newEmployee);
                AddEmployeePopup.IsOpen = false;
                await new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Nhân viên đã được lưu thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();

            }

            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi lưu nhân viên: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }

    }
}


