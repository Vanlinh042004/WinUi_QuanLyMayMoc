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
using Npgsql;
using System.Security.Cryptography;
using System.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        public async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                // Hiển thị thông báo lỗi
                ShowMessage("Tên đăng nhập và mật khẩu không được để trống.");
                return;
            }

            try
            {
                // Kết nối với database
                using (var conn = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine"))
                {
                    await conn.OpenAsync();

                    // Tìm user trong database
                    using (var cmd = new NpgsqlCommand("SELECT id, password_hash FROM users WHERE username = @username", conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int userId = reader.GetInt32(0); // Lấy giá trị user_id (cột đầu tiên)
                                var dbPasswordHash = reader.GetString(1); // Lấy giá trị password_hash (cột thứ hai)

                                if (VerifyPassword(password, dbPasswordHash))
                                {
                                    // Đăng nhập thành công
                                    AppData.Username = username;
                                    AppData.UserId = userId; // Lưu user_id vào ứng dụng
                                    Frame.Navigate(typeof(MainPage)); // Chuyển đến trang chính
                                }
                                else
                                {
                                    // Hiển thị thông báo sai mật khẩu
                                    ShowMessage("Sai mật khẩu");
                                }
                            }
                            else
                            {
                                // Hiển thị thông báo tài khoản không tồn tại
                                ShowMessage("Tài khoản không tồn tại");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi

            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Giải hash và so sánh (dùng BCrypt, SHA256 hoặc thư viện tương tự)
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
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
