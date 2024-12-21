using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View.Mainpage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangePasswordPage : Page
    {

        private string currentUsername; // Tên đăng nhập của người dùng hiện tại

        public ChangePasswordPage()
        {
            this.InitializeComponent();
            currentUsername = AppData.Username; // Truyền tên đăng nhập từ session hoặc trạng thái đăng nhập
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Thông tin tài khoản");
            };
        }

        private void NavigateToLoginPage()
        {
            // Access the ShellWindow instance and update the title
            if (App.MainShellWindow != null)
            {
                App.MainShellWindow.SetContentFrame(typeof(LoginPage));
            }
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.Password.Trim();
            string newPassword = NewPasswordBox.Password.Trim();
            string confirmNewPassword = ConfirmNewPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                ShowMessage("Mật khẩu không được để trống.");
                return;
            }

            if (newPassword != confirmNewPassword)
            {
                ShowMessage("Mật khẩu mới và xác nhận mật khẩu không khớp.");
                return;
            }

            try
            {
                string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // Lấy mật khẩu hash từ database
                    string storedHash = null;
                    using (var cmd = new NpgsqlCommand("SELECT password_hash FROM users WHERE username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("username", NpgsqlTypes.NpgsqlDbType.Varchar, currentUsername); // Xác định kiểu và giá trị
                        storedHash = (string)await cmd.ExecuteScalarAsync();
                    }

                    if (storedHash == null || !BCrypt.Net.BCrypt.Verify(oldPassword, storedHash))
                    {
                        ShowMessage("Mật khẩu cũ không chính xác.");
                        return;
                    }

                    // Hash mật khẩu mới và cập nhật
                    string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    using (var cmd = new NpgsqlCommand("UPDATE users SET password_hash = @password_hash WHERE username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("password_hash", hashedNewPassword);
                        cmd.Parameters.AddWithValue("username", currentUsername);
                        await cmd.ExecuteNonQueryAsync();
                    }

                    ShowMessage("Đổi mật khẩu thành công!");
                    //Frame.Navigate(typeof(LoginPage)); // Chuyển về trang đăng nhập hoặc trang chính
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Thông báo",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            dialog.ShowAsync();
        }

        public async void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin UserId hiện tại (giả sử được lưu trong AppData)
            int userId = AppData.UserId;

            if (userId < 0)
            {
                // Hiển thị thông báo lỗi nếu không tìm thấy UserId
                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot, // Gắn XamlRoot của trang hiện tại
                    Title = "Lỗi",
                    Content = "Không tìm thấy tài khoản để xóa.",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();
                return;
            }

            var confirmDialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot, // Đảm bảo gắn XamlRoot hiện tại
                Title = "Xác nhận xóa tài khoản",
                Content = "Bạn có chắc chắn muốn xóa tài khoản này? Hành động này không thể hoàn tác.",
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy"
            };
            var result = await confirmDialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
            {
                // Người dùng chọn hủy
                return;
            }

            try
            {
                // Kết nối với PostgreSQL
                using (var conn = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine"))
                {
                    await conn.OpenAsync();

                    // Thực hiện lệnh xóa
                    using (var cmd = new NpgsqlCommand("DELETE FROM users WHERE id = @userId", conn))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Hiển thị thông báo thành công
                            ContentDialog successDialog = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot, // Gắn XamlRoot của trang hiện tại
                                Title = "Thành công",
                                Content = "Tài khoản đã được xóa thành công.",
                                CloseButtonText = "Đóng"
                            };
                            await successDialog.ShowAsync();

                            NavigateToLoginPage();
                        }
                        else
                        {
                            // Hiển thị thông báo nếu không tìm thấy tài khoản
                            ContentDialog notFoundDialog = new ContentDialog
                            {
                                XamlRoot = this.XamlRoot, // Gắn XamlRoot của trang hiện tại
                                Title = "Lỗi",
                                Content = "Không tìm thấy tài khoản để xóa.",
                                CloseButtonText = "Đóng"
                            };
                            await notFoundDialog.ShowAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot, // Gắn XamlRoot của trang hiện tại
                    Title = "Lỗi",
                    Content = $"Đã xảy ra lỗi: {ex.Message}",
                    CloseButtonText = "Đóng"
                };
                await errorDialog.ShowAsync();

            }
        }

    }
}
