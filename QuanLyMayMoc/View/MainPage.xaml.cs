﻿//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Foundation;
//using Windows.Foundation.Collections;
//using Microsoft.UI.Xaml;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Controls.Primitives;
//using Microsoft.UI.Xaml.Data;
//using Microsoft.UI.Xaml.Input;
//using Microsoft.UI.Xaml.Media;
//using Microsoft.UI.Xaml.Navigation;
//using System.Text.RegularExpressions;

//// To learn more about WinUI, the WinUI project structure,
//// and more about our project templates, see: http://aka.ms/winui-project-info.

//namespace QuanLyMayMoc
//{
//    /// <summary>
//    /// An empty page that can be used on its own or navigated to within a Frame.
//    /// </summary>
//    public sealed partial class MainPage : Page
//    {
//        public MainPage()
//        {
//            this.InitializeComponent();

//            // disable the button "DichVuTheoThang"

//            DichVuTheoThang.IsEnabled = false;
//            QuanLyMayMoc.IsEnabled = false;
//            DanhSachNhanVien.IsEnabled = false;
//            TongHopMayTheoKy.IsEnabled = false;

//        }

//        private void dichVuTheoThangButton(object sender, RoutedEventArgs e)
//        {
//            // Navigate to the ShellWindow
//            this.FrameContent.Navigate(typeof(DichVuTheoThang));
//        }

//        private void quanLyMayMocButton(object sender, RoutedEventArgs e)
//        {
//            // Navigate to the ShellWindow
//            this.FrameContent.Navigate(typeof(QuanLyMayMoc));
//        }

//        private void danhSachNhanVienButton(object sender, RoutedEventArgs e)
//        {
//            // Navigate to the ShellWindow
//            this.FrameContent.Navigate(typeof(DanhSachNhanVien));
//        }

//        private void tongHopMayTheoKyButton(object sender, RoutedEventArgs e)
//        {
//            // Navigate to the ShellWindow
//            this.FrameContent.Navigate(typeof(TongHopMayTheoKy));
//        }

//        private async void TaoDuAnMoiClick(object sender, RoutedEventArgs e)
//        {
//            // Create the TextBox for input
//            TextBox projectNameTextBox = new TextBox
//            {
//                PlaceholderText = "Nhập tên dự án",
//                Width = 300
//            };

//            // Create the ContentDialog
//            ContentDialog inputProjectNameDialog = new ContentDialog
//            {
//                Title = "Tạo dự án mới",
//                Content = projectNameTextBox,
//                PrimaryButtonText = "OK",
//                CloseButtonText = "Cancel",
//                XamlRoot = this.XamlRoot // Set XamlRoot for the dialog to appear in the correct window context
//            };

//            // Show the dialog and wait for the result
//            var result = await inputProjectNameDialog.ShowAsync();


//            //Var stores the name of the project
//            string projectName;


//            string pattern = @"^(?!\d)[A-Za-z0-9_]+$";

//            // Check if the user clicked "OK"
//            if (result == ContentDialogResult.Primary)
//            {
//                // Retrieve the project name
//                projectName = projectNameTextBox.Text;
//                string date = DateTime.Now.ToString("yyyy_MM_dd");
//                string time = DateTime.Now.ToString("HH_mm_ss");
//                string maDuAn = projectName + date+"_" + time;
//                AppData.ProjectID = maDuAn;

//                // Check if the project name is a valid file name
//                if (!string.IsNullOrEmpty(projectName) && Regex.IsMatch(projectName, pattern))
//                {
//                    // Enable specific options if a valid project name is provided
//                    DichVuTheoThang.IsEnabled = true;
//                    QuanLyMayMoc.IsEnabled = true;
//                    DanhSachNhanVien.IsEnabled = true;
//                    TongHopMayTheoKy.IsEnabled = true;

//                    // Log or use the project name if needed
//                    System.Diagnostics.Debug.WriteLine($"Project created: {projectName}");
                   
//                }
//                else
//                {
//                    // Show a warning if the project name is invalid
//                    await new ContentDialog
//                    {
//                        Title = "Lỗi",
//                        Content = "Tên dự án không hợp lệ. Vui lòng nhập tên không chứa khoảng trắng, ký tự đặc biệt và không bắt đầu bằng số.",
//                        CloseButtonText = "OK",
//                        XamlRoot = this.XamlRoot
//                    }.ShowAsync();
//                }
//            }
//        }

//        private void VeChungToiButton(object sender, RoutedEventArgs e)
//        {
//            //navigate to the AboutUs page
//            this.FrameContent.Navigate(typeof(VeChungToi));
//        }

//        private void PhanHoiButton(object sender, RoutedEventArgs e)
//        {
//            //navigate to the Feedback page
//            this.FrameContent.Navigate(typeof(PhanHoi));
//        }
//    }
//}

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
        public string maDuAn;
        public string projectName;
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
                projectName = projectNameTextBox.Text;


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
                    maDuAn = projectName  + date + "_" + time ;
                    AppData.ProjectID=maDuAn;
                    AppData.ProjectName= projectName;
                    AppData.ProjectTimeCreate=DateTime.Now;
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
            if (string.IsNullOrEmpty(maDuAn))
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

                    // Kiểm tra xem dự án đã tồn tại hay chưa
                    string checkDuAnQuery = "SELECT COUNT(1) FROM duan WHERE maduan = @maDuAn";
                    long duAnExists;

                    using (var command = new NpgsqlCommand(checkDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        duAnExists = (long)await command.ExecuteScalarAsync();
                    }

                    // Nếu cần dùng kiểu `int` ở các đoạn code khác, bạn có thể chuyển đổi về `int` an toàn:
                    int duAnExistsAsInt = (int)duAnExists;

                    if (duAnExists == 0)
                    {
                        // Nếu dự án chưa tồn tại, thêm vào bảng `duan`
                        string insertDuanQuery = "INSERT INTO duan (maduan, tenduan, ngaythuchien) VALUES (@maDuAn, @tenDuAn, @ngayThucHien)";
                        using (var command = new NpgsqlCommand(insertDuanQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                            command.Parameters.AddWithValue("@tenDuAn", AppData.ProjectName);
                            command.Parameters.AddWithValue("@ngayThucHien", AppData.ProjectTimeCreate);
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    // Dù dự án đã tồn tại hay mới được thêm, tiến hành insert dữ liệu từ các bảng tạm thời
                    // Thêm dữ liệu từ nhanvientamthoi vào nhanvien
                    string insertNhanVien = @"
                        INSERT INTO nhanvien (
                            manvduan,
                            manv,
                            hoten,
                            dantoc,
                            gioitinh,
                            ngaysinh,
                            diachi,
                            sdt,
                            email,
                            phongban,
                            cccd,
                            trangthai,
                            ngaykyhopdong,
                            maduan
                        )
                        SELECT 
                            manvduan,
                            manv,
                            hoten,
                            dantoc,
                            gioitinh,
                            ngaysinh,
                            diachi,
                            sdt,
                            email,
                            phongban,
                            cccd,
                            trangthai,
                            ngaykyhopdong,
                            maduan
                        FROM nhanvientamthoi
                        WHERE maduan = @maDuAn;
                    ";

                    using (var command = new NpgsqlCommand(insertNhanVien, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        await command.ExecuteNonQueryAsync();
                    }

                    // Thêm dữ liệu từ congviectamthoi vào congviec
                    string insertCongViec = @"
                        INSERT INTO congviec (
                            macvduan,
                            stt,
                            tendichvu,
                            ngaythuchien,
                            hotenkh,
                            sdt,
                            diachi,
                            manv,
                            tennv,
                            malinhkien,
                            tenlinhkien,
                            soluonglinhkien,
                            maloi,
                            tenloi,
                            soluongloi,
                            phidichvu,
                            ghichu,
                            maduan
                        )
                        SELECT 
                            macvduan,
                            stt,
                            tendichvu,
                            ngaythuchien,
                            hotenkh,
                            sdt,
                            diachi,
                            manv,
                            tennv,
                            malinhkien,
                            tenlinhkien,
                            soluonglinhkien,
                            maloi,
                            tenloi,
                            soluongloi,
                            phidichvu,
                            ghichu,
                            maduan
                        FROM congviectamthoi
                        WHERE maduan = @maDuAn;
                    ";

                    using (var command = new NpgsqlCommand(insertCongViec, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        await command.ExecuteNonQueryAsync();
                    }

                    // Xóa tất cả các dòng trong nhanvientamthoi và congviectamthoi
                    string deleteTempTables = @"
                        DELETE FROM nhanvientamthoi WHERE maduan = @maDuAn;
                        DELETE FROM congviectamthoi WHERE maduan = @maDuAn;
                    ";

                    using (var command = new NpgsqlCommand(deleteTempTables, connection))
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