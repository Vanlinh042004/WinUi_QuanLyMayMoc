
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
using QuanLyMayMoc.ViewModel;
using QuanLyMayMoc.View;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private string projectName;
        public Project CurrentProject { get; set; }
        public MainViewModel ViewModel
        {
            get; set;
        }


        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            // disable the button "DichVuTheoThang"

            buttonToggling();

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
                    string maDuAn = projectName + date + "_" + time;
                    AppData.ProjectID = maDuAn;
                    AppData.ProjectName = projectName;
                    AppData.ProjectTimeCreate = DateTime.Now;
                    CurrentProject = new Project
                    {
                        ID = AppData.ProjectID,
                        Name = AppData.ProjectName,
                        TimeCreate = AppData.ProjectTimeCreate
                    };
                    try
                    {
                        ViewModel.InsertProjectTemp(CurrentProject);  
                    }
                    catch (Exception ex)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Lỗi",
                            Content = $"Có lỗi xảy ra khi lưu dự án: {ex.Message}",
                            CloseButtonText = "OK"
                        };
                    }
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

            else
            {
                DichVuTheoThang.IsEnabled = false;
                QuanLyMayMoc.IsEnabled = false;
                DanhSachNhanVien.IsEnabled = false;
                TongHopMayTheoKy.IsEnabled = false;
                projectName = "";
                AppData.ProjectID = "";
                AppData.ProjectName = "";
                AppData.ProjectTimeCreate = DateTime.MinValue;
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
            if (CurrentProject == null)
            {
                CurrentProject = new Project();
            }
            try
            {

                CurrentProject.ID = AppData.ProjectID;
                CurrentProject.Name = AppData.ProjectName;
                CurrentProject.TimeCreate = AppData.ProjectTimeCreate;
                ViewModel.InsertProject(CurrentProject);
                ViewModel.InsertAllDataFromTemp(CurrentProject.ID);


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

        private void MoDuAnClick(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShellWindow
            this.FrameContent.Navigate(typeof(MoDuAn), this);

        }

        public void buttonToggling()
        {
            if (DichVuTheoThang.IsEnabled == false)
            {
                DichVuTheoThang.IsEnabled = true;
                QuanLyMayMoc.IsEnabled = true;
                DanhSachNhanVien.IsEnabled = true;
                TongHopMayTheoKy.IsEnabled = true;
            }
            else
            {
                DichVuTheoThang.IsEnabled = false;
                QuanLyMayMoc.IsEnabled = false;
                DanhSachNhanVien.IsEnabled = false;
                TongHopMayTheoKy.IsEnabled = false;
            }
        }

        private async void LuuDuAnVoiTenKhacClick(object sender, RoutedEventArgs e)
        {
            TextBox projectNameTextBox = new TextBox
            {
                PlaceholderText = "Nhập tên dự án khác:",
                Width = 300
            };
            // Create the ContentDialog
            ContentDialog inputProjectNameDialog = new ContentDialog
            {
                Title = "Lưu dự án với tên khác",
                Content = projectNameTextBox,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot // Set XamlRoot for the dialog to appear in the correct window context
            };
            if (string.IsNullOrEmpty(AppData.ProjectID))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Chưa mở dự án. Vui lòng tạo dự án mới trước khi lưu.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }
            // Show the dialog and wait for the result
            var result = await inputProjectNameDialog.ShowAsync();

            //Var stores the name of the project
            string projectName = projectNameTextBox.Text;
            string pattern = @"^(?!\d)[A-Za-z0-9_]+$";
            if (string.IsNullOrEmpty(projectName))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Tên dự án không được để trống.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }
            // Check if the user clicked "OK"
            if (result == ContentDialogResult.Primary)
            {
                // Retrieve the project name
                projectName = projectNameTextBox.Text;
            }
            else
            {
                return;
            }

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
                string maDuAn = projectName + date + "_" + time;
                AppData.ProjectID = maDuAn;
                AppData.ProjectName = projectName;
                AppData.ProjectTimeCreate = DateTime.Now;
                CurrentProject = new Project
                {
                    ID = AppData.ProjectID,
                    Name = AppData.ProjectName,
                    TimeCreate = AppData.ProjectTimeCreate
                };
                try
                {
                    //xử lý chỗ này đi ae
                    ViewModel.SaveProjectWithDifferentName(CurrentProject);

                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Có lỗi xảy ra khi lưu dự án: {ex.Message}",
                        CloseButtonText = "OK"
                    };
                }
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
}
