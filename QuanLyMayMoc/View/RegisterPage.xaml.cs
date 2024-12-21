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
using BCrypt.Net;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Tên đăng nhập và mật khẩu không được để trống.");
                return;
            }

            if (password != confirmPassword)
            {
                ShowMessage("Mật khẩu và xác nhận mật khẩu không khớp.");
                return;
            }

            try
            {
                // Kết nối database
                string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    // Kiểm tra xem username đã tồn tại chưa
                    using (var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", conn))
                    {
                        checkCmd.Parameters.AddWithValue("username", username);
                        var count = (long)await checkCmd.ExecuteScalarAsync();
                        if (count > 0)
                        {
                            ShowMessage("Tên đăng nhập đã tồn tại.");
                            return;
                        }
                    }

                    // Hash mật khẩu và thêm người dùng mới
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                    using (var insertCmd = new NpgsqlCommand("INSERT INTO users (username, password_hash) VALUES (@username, @password_hash)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("username", username);
                        insertCmd.Parameters.AddWithValue("password_hash", hashedPassword);
                        await insertCmd.ExecuteNonQueryAsync();
                    }

                    ShowMessage("Đăng ký thành công!");
                    Frame.Navigate(typeof(LoginPage)); // Điều hướng về trang đăng nhập
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

        private void QuayLaiDangNhapClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
