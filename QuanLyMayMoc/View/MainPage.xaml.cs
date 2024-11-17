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
using System.Text.RegularExpressions;
using Npgsql;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            this.InitializeComponent();

            // disable the button "DichVuTheoThang"

            DichVuTheoThang.IsEnabled = false;
            QuanLyMayMoc.IsEnabled = false;
            DanhSachNhanVien.IsEnabled = false;
            TongHopMayTheoKy.IsEnabled = false;

        }

        private void dichVuTheoThangButton(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShellWindow
            this.FrameContent.Navigate(typeof(DichVuTheoThang));
        }

        private void quanLyMayMocButton(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShellWindow
            this.FrameContent.Navigate(typeof(QuanLyMayMoc));
        }

        private void danhSachNhanVienButton(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShellWindow
            this.FrameContent.Navigate(typeof(DanhSachNhanVien));
        }

        private void tongHopMayTheoKyButton(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShellWindow
            this.FrameContent.Navigate(typeof(TongHopMayTheoKy));
        }

        private async void TaoDuAnMoiClick(object sender, RoutedEventArgs e)
        {
            // Create the TextBox for input
            TextBox projectNameTextBox = new TextBox
            {
                PlaceholderText = "Nhập tên dự án",
                Width = 300
            };

            // Create the ContentDialog
            ContentDialog inputProjectNameDialog = new ContentDialog
            {
                Title = "Tạo dự án mới",
                Content = projectNameTextBox,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot // Set XamlRoot for the dialog to appear in the correct window context
            };

            // Show the dialog and wait for the result
            var result = await inputProjectNameDialog.ShowAsync();


            //Var stores the name of the project



            string pattern = @"^(?!\d)[A-Za-z0-9_]+$";

            // Check if the user clicked "OK"
            if (result == ContentDialogResult.Primary)
            {
                // Retrieve the project name
                string projectName = projectNameTextBox.Text;


                // Check if the project name is a valid file name
                if (!string.IsNullOrEmpty(projectName) && Regex.IsMatch(projectName, pattern))
                {
                    // Enable specific options if a valid project name is provided
                    DichVuTheoThang.IsEnabled = true;
                    QuanLyMayMoc.IsEnabled = true;
                    DanhSachNhanVien.IsEnabled = true;
                    TongHopMayTheoKy.IsEnabled = true;

                    string date = DateTime.Now.ToString("yyyy_MM_dd");
                    string time = DateTime.Now.ToString("HH_mm_ss");
                    string maDuAn = projectName + "(" + date + "-" + time + ")";
                    AppData.ProjectID = maDuAn;
                    AppData.ProjectName = projectName;
                    AppData.ProjectTimeCreate = DateTime.Now;
                }
                else
                {
                    // Show a warning if the project name is invalid
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Tên dự án không hợp lệ. Vui lòng nhập tên không chứa khoảng trắng, ký tự đặc biệt và không bắt đầu bằng số.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
            }


        }

        private void VeChungToiButton(object sender, RoutedEventArgs e)
        {
            //navigate to the AboutUs page
            this.FrameContent.Navigate(typeof(VeChungToi));
        }

        private void PhanHoiButton(object sender, RoutedEventArgs e)
        {
            //navigate to the Feedback page
            this.FrameContent.Navigate(typeof(PhanHoi));
        }

        private async void LuuDuAnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AppData.ProjectID))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Chưa có mã dự án. Vui lòng tạo dự án mới trước khi lưu.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }
            string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Chèn vào bảng duan
                    string insertDuanQuery = "INSERT INTO duan (maduan, tenduan,ngaythuchien) VALUES (@maDuAn, @tenDuAn,@ngaythuchien)";
                    using (var command = new NpgsqlCommand(insertDuanQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        command.Parameters.AddWithValue("@tenDuAn", AppData.ProjectName); // Thay thế bằng tên dự án thực tế nếu cần
                        command.Parameters.AddWithValue("@ngaythuchien", AppData.ProjectTimeCreate); // Thay thế bằng tên dự án thực tế nếu cần
                        await command.ExecuteNonQueryAsync();
                    }




                    // Chèn vào bảng Loi_DuAn
                    string insertLoiDuAnQuery = @" INSERT INTO Loi_DuAn (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                   SELECT CONCAT(mahieu, '_', @maDuAn), mahieu, tenloi, giaban, @maDuAn
                                                   FROM loi_tam";
                    using (var command = new NpgsqlCommand(insertLoiDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        await command.ExecuteNonQueryAsync();
                    }
                    // Chèn vào bảng LinhKien_DuAn
                    string insertLinhKienDuAnQuery = @" INSERT INTO LinhKien_DuAn (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                   SELECT CONCAT(mahieu,'_', @maDuAn), mahieu, tenlinhkien, giaban, @maDuAn
                                                   FROM linhkien_tam";
                    using (var command = new NpgsqlCommand(insertLinhKienDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                await new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Dự án đã được lưu thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi lưu dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

    }
}
