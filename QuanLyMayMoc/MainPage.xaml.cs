﻿using System;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // disable the button "DichVuTheoThang"

            DichVuTheoThang.IsEnabled = false;
            QuanLyMayMoc.IsEnabled = false;
            DanhSachNhanVien.IsEnabled = false;
            TongHopMayTheoKy.IsEnabled = false;

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
            string projectName;


            // Check if the user clicked "OK"
            if (result == ContentDialogResult.Primary)
            {
                // Retrieve the project name
                projectName = projectNameTextBox.Text;

                if (!string.IsNullOrEmpty(projectName))
                {
                    // Enable specific options if a project name is provided
                    DichVuTheoThang.IsEnabled = true;
                    QuanLyMayMoc.IsEnabled = true;
                    DanhSachNhanVien.IsEnabled = true;
                    TongHopMayTheoKy.IsEnabled = true;

                    // Log or use the project name if needed
                    System.Diagnostics.Debug.WriteLine($"Project created: {projectName}");
                }
                else
                {
                    // Optionally, show a warning if no name was entered
                    await new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Tên dự án không được để trống.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
            }
        }

        private void VeChungToiButton(object sender, RoutedEventArgs e)
        {
            //navigate to the AboutUs page
            this.FrameContent.Navigate(typeof(AboutUs));
        }
    }
}
