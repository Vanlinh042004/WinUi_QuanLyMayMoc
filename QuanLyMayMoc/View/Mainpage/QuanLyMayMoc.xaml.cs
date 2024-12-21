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

        //private void LinhKienButton(object sender, RoutedEventArgs e)
        //{

        //    if (FrameContent.Content is not LinhKien linhkienPage)
        //    {
        //        linhkienPage = new LinhKien();
        //        FrameContent.Navigate(typeof(LinhKien));
        //    }
        //    else
        //    {
        //        return;
        //    }
           
        //}

        //private void LoiButton(object sender, RoutedEventArgs e)
        //{
            
        //    if (FrameContent.Content is not Loi loiPage)
        //    {
        //        loiPage = new Loi();
        //        FrameContent.Navigate(typeof(Loi));
        //    }
        //    else
        //    {
        //        return;
        //    }
           
        //}
    }
}
