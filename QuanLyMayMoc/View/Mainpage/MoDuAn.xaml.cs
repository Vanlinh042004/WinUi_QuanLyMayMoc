using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.Service;
using QuanLyMayMoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using static QuanLyMayMoc.AppData;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoDuAn : Page
    {
        private ObservableCollection<Project> projects;
        private MainViewModel mainViewModel = new MainViewModel();

        // ...

        public MoDuAn()
        {
            this.InitializeComponent();

            //loadDuAnFromDB();
            projects = mainViewModel.getProjects();
            // print in the UI that let the user know there is no project
            if (projects.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Không có dự án nào";
                DataStackPanel.Children.Add(textBlock);
            }
            showDuAn();
            mainViewModel.ClearAllTempData();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Mở dự án");
            };
        }
      

        private MainPage _mainPage;

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainPage = e.Parameter as MainPage;
        }


       


        private async void SendAuthCodeToEmail(string recipientEmail, string authCode)
        {
            try
            {
                // Cấu hình các thông tin gửi email
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mandeotv1234@gmail.com", "wyvr ptnk brbk ular"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mandeotv1234@gmail.com"),
                    Subject = "Mã xác thực",
                    Body = $"Mã xác thực của bạn là: {authCode}",
                    IsBodyHtml = false, // Nếu cần gửi email dạng HTML, thay thành true
                };

                mailMessage.To.Add(recipientEmail);

                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);

                // Xử lý sau khi gửi email thành công (nếu cần)
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                var errorDialog = new ContentDialog
                {
                    XamlRoot = this.Content.XamlRoot, // Đảm bảo thuộc tính XamlRoot được gán đúng
                    Title = "Lỗi",
                    Content = $"Có lỗi khi gửi mã xác thực: {ex.Message}",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
            }
        }

        public string GenerateUniqueCode()
        {
            Random random = new Random();
            string randomPart = random.Next(1000, 9999).ToString(); // Phần ngẫu nhiên
            string timePart = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // Phần thời gian
            return timePart + randomPart;
        }

        // Hàm gọi để yêu cầu quyền truy cập và xác thực
        private async void HandleAccessRequest(Project project)
        {
            var dialog = new ContentDialog
            {
                XamlRoot = this.Content.XamlRoot, // Đảm bảo thuộc tính XamlRoot được gán đúng
                Title = "Quyền truy cập",
                Content = "Bạn không có quyền truy cập dự án này. Vui lòng xin cấp quyền từ chủ sở hữu.",
                CloseButtonText = "Hủy",
                PrimaryButtonText = "OK",
               // DefaultButton = ContentDialogButton.Primary
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string authCode = GenerateAuthCode(); // Tạo mã xác thực
                SendAuthCodeToEmail(project.Email, authCode); // Gửi mã xác thực tới email của chủ sở hữu

                // Lưu mã xác thực và thời gian hết hạn
                StoreAuthCode(project.Email, authCode);

                // Hiển thị ô nhập mã xác thực
                TextBox inputBox = new TextBox { PlaceholderText = "Nhập mã xác thực" };
                var authDialog = new ContentDialog
                {
                    XamlRoot = this.Content.XamlRoot, // Đảm bảo thuộc tính XamlRoot được gán đúng
                    Title = "Xác thực",
                    Content = inputBox,
                    PrimaryButtonText = "Xác nhận",
                    CloseButtonText = "Hủy",
                    DefaultButton = ContentDialogButton.Primary
                };

                var authResult = await authDialog.ShowAsync();

                if (authResult == ContentDialogResult.Primary)
                {
                    if (ValidateAuthCode(project.Email, inputBox.Text))
                    {
                        // Mã xác thực đúng, cho phép truy cập
                        GrantAccessToProject(project);
                       addUserToProject(project);
                    }
                    else
                    {
                        // Mã xác thực sai, hiển thị lỗi
                        var errorDialog = new ContentDialog
                        {
                            XamlRoot = this.Content.XamlRoot, // Đảm bảo thuộc tính XamlRoot được gán đúng
                            Title = "Lỗi xác thực",
                            Content = "Mã xác thực không đúng. Vui lòng thử lại.",
                            CloseButtonText = "OK"
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

       private void addUserToProject(Project project)
        {
            string Id = GenerateUniqueCode();
            User newUser = new User { Email = AppData.Email, Name = AppData.Name };
            mainViewModel.AddUserToProject(newUser, Id, project.ID);
        }
      
        // Hàm tạo mã xác thực
        private string GenerateAuthCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Mã 6 chữ số
        }

        // Hàm lưu mã xác thực và thời gian hết hạn
        private void StoreAuthCode(string email, string authCode)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(5); // Mã xác thực hết hạn sau 5 phút

            // Lưu mã xác thực và thời gian hết hạn vào authCodes
            authCodes[email] = (authCode, expirationTime);
        }

        // Hàm kiểm tra mã xác thực
        // authCodes lưu trữ mã xác thực và thời gian hết hạn
        private Dictionary<string, (string AuthCode, DateTime ExpirationTime)> authCodes = new Dictionary<string, (string, DateTime)>();

        private bool ValidateAuthCode(string email, string inputCode)
        {
            if (authCodes.ContainsKey(email))
            {
                var (storedCode, expirationTime) = authCodes[email];

                if (DateTime.Now <= expirationTime) // Kiểm tra xem mã xác thực có hết hạn không
                {
                    return inputCode == storedCode; // So sánh mã xác thực nhập vào với mã đã lưu
                }
            }

            return false; // Trả về false nếu không tồn tại email hoặc mã hết hạn
        }


        // Hàm cấp quyền truy cập vào dự án
        private void GrantAccessToProject(Project project)
        {
            AppData.isEnableFunctionButtion = true;

            AppData.ProjectID = project.ID;
            AppData.ProjectName = project.Name;
            AppData.ProjectTimeCreate = project.TimeCreate;

            this.Frame.Navigate(typeof(DichVuTheoThang), project);

            UpdateShellWindowTitle(AppData.ProjectName);
        }


        public void showDuAn()
        {
            // Hiển thị dữ liệu được lưu trong Items lên MoDuAn page
            foreach (Project project in projects)
            {
                Button button = new Button();
                DataStackPanel.Children.Add(button);
                button.Margin = new Thickness(0, 10, 0, 0);
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.RequestedTheme = ElementTheme.Light;

                // Tạo một Grid để chứa các TextBlock
                Grid grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.Padding = new Thickness(10, 10, 10, 10); // Thêm khoảng cách

                // Định nghĩa ba cột trong Grid
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) }); // Cột 1: Thời gian (200px)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }); // Cột 2: Tên dự án
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(300) }); // Cột 3: Người tạo dự án (300px)

                // Tạo TextBlock cho thời gian
                TextBlock timeTextBlock = new TextBlock();
                timeTextBlock.Text = project.TimeCreate.ToString("g");
                timeTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                timeTextBlock.VerticalAlignment = VerticalAlignment.Center;
                timeTextBlock.FontSize = 16; // Tăng kích thước chữ
                Grid.SetColumn(timeTextBlock, 0); // Đặt ở cột 1

                // Tạo TextBlock cho tên dự án
                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.Text = project.Name;
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                nameTextBlock.FontWeight = FontWeights.Bold; // Chữ in đậm
                nameTextBlock.FontSize = 16; // Tăng kích thước chữ
                Grid.SetColumn(nameTextBlock, 1); // Đặt ở cột 2

                // Tạo TextBlock cho người tạo dự án
                TextBlock creatorTextBlock = new TextBlock();
                creatorTextBlock.Text = $"   Người tạo: {project.NameEmail}";
                creatorTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                creatorTextBlock.VerticalAlignment = VerticalAlignment.Center;
               // creatorTextBlock.FontStyle = FontStyle.Italic; // Chữ nghiêng
                creatorTextBlock.FontSize = 14; // Kích thước nhỏ hơn một chút
                Grid.SetColumn(creatorTextBlock, 2); // Đặt ở cột 3

                // Thêm các TextBlock vào Grid
                grid.Children.Add(timeTextBlock);
                grid.Children.Add(nameTextBlock);
                grid.Children.Add(creatorTextBlock);

                button.Content = grid;

                button.Click += async (sender, e) =>
                {
                    if (AppData.Email == project.Email)
                    {
                        // Nếu là dự án của người dùng, mở bình thường
                        AppData.isEnableFunctionButtion = true;

                        AppData.ProjectID = project.ID;
                        AppData.ProjectName = project.Name;
                        AppData.ProjectTimeCreate = project.TimeCreate;
                        addUserToProject(project);

                        this.Frame.Navigate(typeof(DichVuTheoThang), project);

                        UpdateShellWindowTitle(AppData.ProjectName);
                    }
                    else
                    {
                        HandleAccessRequest(project);
                    }
                };

            }
        }


        private void UpdateShellWindowTitle(string title)
        {
            // Access the ShellWindow instance and update the title
            if (App.MainShellWindow != null)
            {
                App.MainShellWindow.UpdateTitle(title + " - Quản lý máy móc");
            }
        }

      
    }
}
