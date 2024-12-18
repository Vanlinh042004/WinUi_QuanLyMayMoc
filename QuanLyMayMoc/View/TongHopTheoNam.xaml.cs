using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QuanLyMayMoc.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TongHopTheoNam : Page
    {
        public MainViewModel ViewModel
        {
            get; set;
        }
        private DispatcherTimer refreshTimer; // Timer để tự động refresh

        public TongHopTheoNam()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            InitializeRefreshTimer();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Tổng hợp theo năm");
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

    }
}
