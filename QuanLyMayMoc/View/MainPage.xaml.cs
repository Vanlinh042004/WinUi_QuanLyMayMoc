
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
using Microsoft.UI.Windowing;
using Windows.UI.WindowManagement;
using QuanLyMayMoc.View.Mainpage;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current { get; private set; }

        public TextBlock HeaderText { get; private set; }

        private string projectName = "";
        public Project CurrentProject { get; set; }

        public MainViewModel ViewModel
        {
            get; set;
        }
        private void UpdateShellWindowTitle(string title)
        {
            // Access the ShellWindow instance and update the title
            if (App.MainShellWindow != null)
            {
                App.MainShellWindow.UpdateTitle(title + " - Quản lý máy móc");
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            ViewModel.ClearSummary();
            // disable the button "DichVuTheoThang"
            AppData.isEnableFunctionButtion = false;
            AppData.ProjectID = "";
            this.FrameContent.Navigate(typeof(MoDuAn));
            //buttonToggling();
            Current = this;
            HeaderText = HeaderTextBlock;
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
            await ShowProjectNameDialog();
            if (AppData.ProjectID == "")
            {
                this.FrameContent.Navigate(typeof(MoDuAn));
                return;
            }
            else
            {
                this.FrameContent.Navigate(typeof(DichVuTheoThang));
                UpdateShellWindowTitle(AppData.ProjectName);
            }
        }

        private async System.Threading.Tasks.Task ShowProjectNameDialog()
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
                    //DichVuTheoThang.IsEnabled = true;
                    //QuanLyMayMoc.IsEnabled = true;
                    //DanhSachNhanVien.IsEnabled = true;
                    //TongHopMayTheoKy.IsEnabled = true;

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
                //DichVuTheoThang.IsEnabled = false;
                //QuanLyMayMoc.IsEnabled = false;
                //DanhSachNhanVien.IsEnabled = false;
                //TongHopMayTheoKy.IsEnabled = false;
                projectName = "";
                AppData.ProjectID = "";
                AppData.ProjectName = "";
                AppData.ProjectTimeCreate = DateTime.MinValue;
            }
        }
        private void HuongDanSuDungButton(object sender, RoutedEventArgs e)
        {
            
            this.FrameContent.Navigate(typeof(HuongDanSuDung));
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
            if (!AppData.isEnableFunctionButtion == false)
            {
                //DichVuTheoThang.IsEnabled = true;
                //QuanLyMayMoc.IsEnabled = true;
                //DanhSachNhanVien.IsEnabled = true;
                //TongHopMayTheoKy.IsEnabled = true;
                AppData.isEnableFunctionButtion = false;
            }
            else
            {
                //DichVuTheoThang.IsEnabled = false;
                //QuanLyMayMoc.IsEnabled = false;
                //DanhSachNhanVien.IsEnabled = false;
                //TongHopMayTheoKy.IsEnabled = false;
                AppData.isEnableFunctionButtion = true;
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
                    Content = "Chưa mở dự án. Vui lòng mở dự án trước khi lưu.",
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
                // ViewModel.RefreshData();
            }
            else
            {
                return;
            }

            // Check if the project name is a valid file name
            if (!string.IsNullOrEmpty(projectName) && Regex.IsMatch(projectName, pattern))
            {
                // Enable specific options if a valid project name is provided
                //DichVuTheoThang.IsEnabled = true;
                //QuanLyMayMoc.IsEnabled = true;
                //DanhSachNhanVien.IsEnabled = true;
                //TongHopMayTheoKy.IsEnabled = true;

                string date = DateTime.Now.ToString("yyyy_MM_dd");
                string time = DateTime.Now.ToString("HH_mm_ss");
                string maDuAn = projectName + date + "_" + time;
                string oldProjectID = AppData.ProjectID;
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
                    ViewModel.SaveProjectWithDifferentName(CurrentProject, oldProjectID);
                    AppData.ProjectID = CurrentProject.ID;
                    AppData.ProjectName = CurrentProject.Name;
                    AppData.ProjectTimeCreate = CurrentProject.TimeCreate;
                    // Navigate to the ShellWindow
                    this.FrameContent.Navigate(typeof(MoDuAn), this);

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

        private async void XoaDuAnClick(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteProjectDialog = new ContentDialog
            {
                Title = "Bạn có chắc chắn muốn xóa dự án?",
                Content = "Hành động này sẽ xóa dự án của bạn",
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel",
                XamlRoot = this.XamlRoot // Set XamlRoot for the dialog to appear in the correct window context
            };
            var result = await deleteProjectDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrEmpty(AppData.ProjectID))
                {
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Chưa mở dự án. Vui lòng mở dự án trước khi xóa.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                    return;
                }
                try
                {
                    if (CurrentProject == null)
                    {
                        CurrentProject = new Project();
                        CurrentProject.ID = AppData.ProjectID;
                        CurrentProject.Name = AppData.ProjectName;
                        CurrentProject.TimeCreate = AppData.ProjectTimeCreate;
                    }

                    ViewModel.DeleteProject(AppData.ProjectID);
                    //navigate to MoDuAn
                    this.FrameContent.Navigate(typeof(MoDuAn), this);
                    AppData.ProjectID = "";
                    //buttonToggling();
                    await new ContentDialog
                    {
                        Title = "Thành công",
                        Content = "Dự án đã được xóa thành công.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
                catch (Exception ex)
                {
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Có lỗi xảy ra khi xóa dự án: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
            }
        }
        private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string selectedItemTag = args.SelectedItemContainer.Tag?.ToString();
                if (selectedItemTag != null)
                {
                    switch (selectedItemTag)
                    {
                        case "User":
                            this.FrameContent.Navigate(typeof(ChangePasswordPage));
                            return;
                        case "Logout":
                            AppData.Username = "";
                            AppData.UserId = 0;
                            AppData.ProjectID = "";
                            AppData.ProjectName = "";
                            AppData.ProjectTimeCreate = DateTime.MinValue;

                            // Xóa thông tin đăng nhập Google
                            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            string credPath = Path.Combine(currentDirectory, "token.json");

                            // Kiểm tra nếu tệp token tồn tại và xóa nó
                            if (Directory.Exists(credPath))
                            {
                                Directory.Delete(credPath, true); // Xóa toàn bộ thư mục token.json
                            }

                            // Điều hướng về trang đăng nhập
                            App.MainShellWindow.SetContentFrame(typeof(LoginPage));
                            return;

                    }
                }
                if (AppData.ProjectID == "")
                {
                    await ShowProjectNameDialog();
                    
                    if (AppData.ProjectID == "")
                    {
                        this.FrameContent.Navigate(typeof(MoDuAn));
                        return;
                    }
                    else
                    {
                        UpdateShellWindowTitle(AppData.ProjectName);
                    }
                }
                if (selectedItemTag != null)
                {
                    // Điều hướng đến các trang tương ứng
                    switch (selectedItemTag)
                    {
                        case "DichVuTheoThang":
                            FrameContent.Navigate(typeof(DichVuTheoThang));
                            //ChangeHeaderTextBlock("Dịch vụ theo tháng");
                            break;
                        case "QuanLyMayMoc":
                            FrameContent.Navigate(typeof(QuanLyMayMoc));
                            //ChangeHeaderTextBlock("Quản lý máy móc");
                            break;
                        case "DanhSachNhanVien":
                            FrameContent.Navigate(typeof(DanhSachNhanVien));
                            //ChangeHeaderTextBlock("Danh sách nhân viên");
                            break;
                        case "ThongkeMayMoc":
                            FrameContent.Navigate(typeof(TongHopMayTheoKy));
                            //ChangeHeaderTextBlock("Thống kê máy móc");
                            break;
                        case "LinhKien":
                            FrameContent.Navigate(typeof(LinhKien));
                            break;
                        case "Loi":
                            FrameContent.Navigate(typeof(Loi));
                            break;
                        case "TongHopMay":
                            FrameContent.Navigate(typeof(May));
                            break;
                        case "TongHopDichVu":
                            FrameContent.Navigate(typeof(DichVu));
                            break;
                        case "TongHopTheoNam":
                            FrameContent.Navigate(typeof(TongHopTheoNam));
                            break;
                    }
                }
            }
        }
        // Phương thức tĩnh để thay đổi TextBlock trong Header
        public static void ChangeHeaderTextBlock(string changed)
        {
            // Kiểm tra nếu Current không null (trong trường hợp đối tượng MainPage chưa được tạo)
            if (Current != null)
            {
                Current.HeaderTextBlock.Text = changed;
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void DemoClick(object sender, RoutedEventArgs e)
        {
            this.FrameContent.Navigate(typeof(Demo));
        }
    }
}
