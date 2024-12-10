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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuanLyMayMoc : Page
    {

       
        public QuanLyMayMoc()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Quản lý máy móc");
            };
        }

        private void LinhKienButton(object sender, RoutedEventArgs e)
        {
            
                FrameContent.Navigate(typeof(LinhKien)); // Điều hướng trang LinhKien
            // Lần đầu tiên nhấn: Tải dữ liệu từ bảng gốc vào ViewModel
            //if (app.IsFirstLinhKienClick)
            //{
            //    if (FrameContent.Content is LinhKien currentPage)
            //    {
            //        currentPage.ViewModel.LoadLinhKienFromDatabase();  // Tải dữ liệu từ bảng gốc
            //    }

            //    // Đổi cờ sau lần nhấn đầu tiên
            //    app.IsFirstLinhKienClick = false;
            //}
            //else
            //{
            //    // Lần sau: Lấy dữ liệu từ bảng tạm
            //    if (FrameContent.Content is LinhKien currentPage)
            //    {
            //        currentPage.ViewModel.LoadLinhKienFromTemp();  // Tải dữ liệu từ bảng tạm
            //    }
            //}
        }

        private void LoiButton(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            if (FrameContent.Content is not Loi loiPage)
            {
                loiPage = new Loi();
                FrameContent.Navigate(typeof(Loi));
            }
            else
            {
                return;
            }
            if (app.IsFirstLoiClick)
            {
                if (FrameContent.Content is Loi currentPage)
                {
                    currentPage.ViewModel.LoadLoiFromDatabase();
                }
                app.IsFirstLoiClick = false;
            }
            else
            {
                if (FrameContent.Content is Loi currentPage)
                {
                    currentPage.ViewModel.LoadLoiFromTemp();
                }
            }
        }
    }
}
