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
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Util.Store;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

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

        //public async void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string username = UsernameTextBox.Text;
        //    string password = PasswordBox.Password;

        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //    {
        //        // Hiển thị thông báo lỗi
        //        ShowMessage("Tên đăng nhập và mật khẩu không được để trống.");
        //        return;
        //    }

        //    try
        //    {
        //        // Kết nối với database
        //        using (var conn = new NpgsqlConnection("Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine"))
        //        {
        //            await conn.OpenAsync();

        //            // Tìm user trong database
        //            using (var cmd = new NpgsqlCommand("SELECT id, password_hash FROM users WHERE username = @username", conn))
        //            {
        //                cmd.Parameters.AddWithValue("username", username);

        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    if (await reader.ReadAsync())
        //                    {
        //                        int userId = reader.GetInt32(0); // Lấy giá trị user_id (cột đầu tiên)
        //                        var dbPasswordHash = reader.GetString(1); // Lấy giá trị password_hash (cột thứ hai)

        //                        if (VerifyPassword(password, dbPasswordHash))
        //                        {
        //                            // Đăng nhập thành công
        //                            AppData.Username = username;
        //                            AppData.UserId = userId; // Lưu user_id vào ứng dụng
        //                            Frame.Navigate(typeof(MainPage)); // Chuyển đến trang chính
        //                        }
        //                        else
        //                        {
        //                            // Hiển thị thông báo sai mật khẩu
        //                            ShowMessage("Sai mật khẩu");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Hiển thị thông báo tài khoản không tồn tại
        //                        ShowMessage("Tài khoản không tồn tại");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi

        //    }
        //}

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




        private async void GoogleLoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] Scopes = { "email", "profile" };
                string ApplicationName = "QuanLyMayMoc-Window";

                using (var stream = new FileStream(@"E:\DOANWINDOW\WinUi_QuanLyMayMoc\QuanLyMayMoc\client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    string currentDirectory = AppDomain.CurrentDomain.BaseDirectory; // Lấy thư mục hiện tại
                    string credPath = Path.Combine(currentDirectory, "token.json"); // Tạo đường dẫn đến token.json

                    var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true));

                    // Tạo dịch vụ OAuth2 để lấy thông tin người dùng
                    var oauth2Service = new Google.Apis.Oauth2.v2.Oauth2Service(new Google.Apis.Services.BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = ApplicationName,
                    });

                    // Lấy thông tin người dùng (email, profile)
                    var userInfo = await oauth2Service.Userinfo.Get().ExecuteAsync();
                    string email = userInfo.Email; // Email đã xác thực
                    string name = userInfo.Name;  // Tên người dùng

                    // Lưu email vào AppData
                    AppData.Email = email;
                    AppData.Name = name;

                    // Hiển thị thông báo thành công
                    var success = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = $"Đăng nhập Google thành công với email: {email}\nTên: {name}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot // Đảm bảo ContentDialog hoạt động trong WinUI 3
                    };
                    await success.ShowAsync();

                    // Chuyển hướng đến MainPage
                    Frame.Navigate(typeof(MainPage));
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Đã xảy ra lỗi: {ex.Message}\n{ex.StackTrace}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                await errorDialog.ShowAsync();
            }
        }



    }
}
