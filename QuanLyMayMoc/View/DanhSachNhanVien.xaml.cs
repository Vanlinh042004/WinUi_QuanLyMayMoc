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
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DanhSachNhanVien : Page
    {
        private Page _window;

        public MainViewModel ViewModel
        {
            get; set;
        }

        private string _selectedImagePath;

        private DispatcherTimer refreshTimer; // Timer để tự động refresh

        public DanhSachNhanVien()
        {
            this.InitializeComponent();
       
            // Initialize the list with sample data
            ViewModel = new MainViewModel();
            InitializeRefreshTimer();

            ViewModel.LoadDataEmployee();
            _window = new Page();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Danh sách nhân viên");
            };

        }

        private void InitializeRefreshTimer()
        {
            refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5) // Khoảng thời gian lặp lại (5 giây)
            };
            refreshTimer.Tick += (sender, e) => ViewModel.RefreshData();
            refreshTimer.Start();
        }

        protected override void OnNavigatedFrom(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // Dừng Timer khi trang không còn hiển thị
            refreshTimer?.Stop();
            base.OnNavigatedFrom(e);
        }



        private async void ChooseImageButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new FileOpenPicker();

            // Cần thiết để hỗ trợ WinUI 3 trên desktop
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(new Window());

            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        picker.FileTypeFilter.Add(".jpg");
        picker.FileTypeFilter.Add(".png");
        picker.FileTypeFilter.Add(".jpeg");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Sao chép file vào thư mục LocalFolder
                var localFolder = ApplicationData.Current.LocalFolder;
                var copiedFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);

                // Lưu tên ảnh vào biến tạm để sử dụng sau khi lưu vào database
                _selectedImagePath = copiedFile.Name;  // Chỉ lưu tên file, không bao gồm đường dẫn đầy đủ

                // Hiển thị đường dẫn file ảnh đã chọn (local path)
                SelectedImagePath.Text = copiedFile.Path;
                _selectedImagePath = copiedFile.Path;
            }
            else
        {
            SelectedImagePath.Text = "Không có ảnh nào được chọn.";
        }
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
                NgayKyHD = NgayDKHDInput.SelectedDate?.Date ?? DateTime.MinValue,
                CCCD = CCCDInput.Text ?? "Không có",
                SoDienThoai = SoDienThoaiInput.Text ?? "Không có",
                Email = EmailInput.Text ?? "Không có",
                DanToc = DanTocInput.Text ?? "Không có",
                DiaChi = DiaChiInput.Text ?? "Không có",
                TrangThai = TrangThaiInput.Text ?? "Không có",
                PhongBan = PhongBanInput.Text ?? "Không có",
                AnhDaiDien = _selectedImagePath/*"ms-appx:///Assets/"*/ /*+ AnhDaiDienInput.Text*/,
                MaDuAn = AppData.ProjectID,
            };

            try
            {
                // Kiểm tra mã nhân viên trong cơ sở dữ liệu
                bool isDuplicate =  ViewModel.CheckEmployeeExistsAsync(newEmployee.MaNhanVien);

                if (isDuplicate)
                {
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                    return; // Dừng việc thêm nhân viên mới
                }

                // Thêm nhân viên mới
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


