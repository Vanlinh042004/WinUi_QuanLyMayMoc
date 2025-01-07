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

            ViewModel.LoadDataEmployees();
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
                Interval = TimeSpan.FromSeconds(2) // Khoảng thời gian lặp lại (2 giây)
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
            Overlay.Visibility = Visibility.Visible;
            AddEmployeePopup.IsOpen = true; // Mở popup
        }

        // Sự kiện khi nhấn nút "Hủy"
        private void OnCancelEmployeeClicked(object sender, RoutedEventArgs e)
        {
            Overlay.Visibility = Visibility.Collapsed;
            AddEmployeePopup.IsOpen = false; // Đóng popup
        }


        private async void OnSaveEmployeeClicked(object sender, RoutedEventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(MaNhanVienInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập mã nhân viên.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(HoTenInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập họ tên.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (!NgaySinhInput.SelectedDate.HasValue)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng chọn ngày sinh.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if ((DateTime.Now.Year - NgaySinhInput.SelectedDate.Value.Year) < 18 ||
                ((DateTime.Now.Year - NgaySinhInput.SelectedDate.Value.Year) == 18 &&
                DateTime.Now.DayOfYear < NgaySinhInput.SelectedDate.Value.DayOfYear))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Nhân viên phải đủ 18 tuổi.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(CCCDInput.Text) || CCCDInput.Text.Length != 12 || !CCCDInput.Text.All(char.IsDigit))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "CCCD phải là 12 ký tự số.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(SoDienThoaiInput.Text) || SoDienThoaiInput.Text.Length != 10 || !SoDienThoaiInput.Text.All(char.IsDigit))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Số điện thoại phải là 10 ký tự số.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (!NgayDKHDInput.SelectedDate.HasValue || NgayDKHDInput.SelectedDate.Value > DateTime.Now)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Ngày ký hợp đồng phải nhỏ hơn hoặc bằng ngày hiện tại.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập email.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            // Kiểm tra định dạng email
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(EmailInput.Text, emailPattern))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập email hợp lệ.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(DiaChiInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập địa chỉ.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(DanTocInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập dân tộc.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(TrangThaiInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập trạng thái.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }

            if (string.IsNullOrWhiteSpace(PhongBanInput.Text))
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Vui lòng nhập phòng ban.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return;
            }
            var newEmployee = new Employee
            {
                MaNhanVienDuAn = MaNhanVienInput.Text + AppData.ProjectID,
                MaNhanVien = MaNhanVienInput.Text,
                HoTen = HoTenInput.Text,
                GioiTinh = (GioiTinhInput.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Khác",
                NgaySinh = NgaySinhInput.SelectedDate.Value.Date,
                NgayKyHD = NgayDKHDInput.SelectedDate.Value.Date,
                CCCD = CCCDInput.Text,
                SoDienThoai = SoDienThoaiInput.Text,
                Email = EmailInput.Text,
                DanToc = DanTocInput.Text ?? "Không có",
                DiaChi = DiaChiInput.Text,
                TrangThai = TrangThaiInput.Text ?? "Không có",
                PhongBan = PhongBanInput.Text ?? "Không có",
                AnhDaiDien = _selectedImagePath,
                MaDuAn = AppData.ProjectID,
            };

            try
            {
                // Kiểm tra mã nhân viên trong cơ sở dữ liệu
                if (ViewModel.CheckEmployeeExistsAsync(newEmployee.MaNhanVien))
                {
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Mã nhân viên đã tồn tại. Vui lòng nhập mã khác.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                    return;
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


