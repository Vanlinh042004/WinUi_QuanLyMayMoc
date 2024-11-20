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
            // Create a new employee object
            var newEmployee = new Employee
            {
                MaNhanVien = MaNhanVienInput.Text ?? "Không có",// Default to -1 if input is null or invalid
                HoTen = HoTenInput.Text ?? "Không rõ",
                GioiTinh = (GioiTinhInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Khác",
                NgaySinh = NgaySinhInput.SelectedDate?.Date ?? DateTime.MinValue,
                CCCD = CCCDInput.Text ?? "Không có",
                SoDienThoai = SoDienThoaiInput.Text ?? "Không có",
                Email = EmailInput.Text ?? "Không có",
                DiaChi = DiaChiInput.Text ?? "Không có",
                TrangThai = TrangThaiInput.Text ?? "Không có",
                PhongBan = PhongBanInput.Text ?? "Không có",
                AnhDaiDien = string.IsNullOrWhiteSpace(AnhDaiDienInput.Text) ? "No Image" : AnhDaiDienInput.Text, // Default if no image provided
                MaDuAn = AppData.ProjectID,
            };

            // Add the new employee to the ViewModel
            ViewModel.Employees.Add(newEmployee);

            // Connection string for the database
            string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=postgres";

            // SQL query to insert a new employee into the database
            string insertEmployeeQuery = @"
        INSERT INTO nhanvientamthoi 
        (manvduan,manv, hoten, gioitinh, ngaysinh, diachi, sdt, email,phongban, cccd,trangthai,ngaykyhopdong,maduan  ) 
        VALUES 
        (@manvduan,@manv, @hoten, @gioitinh, @ngaysinh, @diachi, @sdt, @email,@phongban, @cccd, @trangthai,@ngaykyhopdong,@maduan)";

            try
            {
                // Open a connection to the database
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Prepare the SQL command with parameters
                    using (var command = new NpgsqlCommand(insertEmployeeQuery, connection))
                    {
                        command.Parameters.AddWithValue("@manvduan", newEmployee.MaNhanVien + AppData.ProjectID);
                        command.Parameters.AddWithValue("@manv", newEmployee.MaNhanVien);
                        command.Parameters.AddWithValue("@hoten", newEmployee.HoTen);
                        command.Parameters.AddWithValue("@gioitinh", newEmployee.GioiTinh);
                        command.Parameters.AddWithValue("@ngaysinh", newEmployee.NgaySinh);
                        command.Parameters.AddWithValue("@diachi", newEmployee.DiaChi);
                        command.Parameters.AddWithValue("@sdt", newEmployee.SoDienThoai);
                        command.Parameters.AddWithValue("@email", newEmployee.Email);
                        command.Parameters.AddWithValue("@phongban", newEmployee.PhongBan);
                        command.Parameters.AddWithValue("@cccd", newEmployee.CCCD);
                        command.Parameters.AddWithValue("@trangthai", newEmployee.TrangThai);
                        command.Parameters.AddWithValue("@ngaykyhopdong", newEmployee.NgayKyHD);
                        // command.Parameters.AddWithValue("@anhdaidien", newEmployee.AnhDaiDien);
                        command.Parameters.AddWithValue("@maduan", newEmployee.MaDuAn);

                        // Execute the SQL command
                        await command.ExecuteNonQueryAsync();
                    }
                    //}

                    // Close the popup after successful save
                    AddEmployeePopup.IsOpen = false;

                    // Optionally, show a confirmation message
                    ShowSuccessMessage("Nhân viên được lưu  thành công");
                }
            }

            catch (Exception ex)
            {
                // Handle any errors during the database operation
                ShowNotSuccessMessage("Nhân không được lưu thành công.");
            }
        
    }
        
        private async void ShowSuccessMessage(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Thành công",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Set the XamlRoot here too
            };

            await successDialog.ShowAsync();
        }
        private async void ShowNotSuccessMessage(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Fail",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Set the XamlRoot here too
            };

            await successDialog.ShowAsync();
        }
    }

}


