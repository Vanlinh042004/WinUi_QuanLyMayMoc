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
                    Frame.Navigate(typeof(LoginPage)); // Chuyển về trang đăng nhập hoặc trang chính
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
    }
}
