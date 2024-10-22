using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DichVuTheoThang : Page
    {
        private int currentRow = 1;
        private int Columns = 15;

        public DichVuTheoThang()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên

        }
        private void HideFirstRow()
        {
            // Tạo một hàng ẩn ở vị trí row 0
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // Tạo một phần tử rỗng và ẩn nó
            var emptyElement = new TextBox
            {
                Visibility = Visibility.Collapsed // Ẩn phần tử ở dòng đầu tiên
            };

            // Đặt phần tử vào row 0 và column 0 (có thể là dòng tiêu đề hoặc dòng không mong muốn)
            Grid.SetRow(emptyElement, 0);
            Grid.SetColumn(emptyElement, 0);
            InputGrid.Children.Add(emptyElement);
        }
        // Thêm dòng mới vào ExcelGrid
        private void AddNewRow()
        {

            // Thêm dòng mới vào Grid
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;

                //// Tạo DatePicker nếu ở cột 1 hoặc 8, ngược lại tạo TextBox
                //if (col == 1 || col == 8)
                //{
                //    element = new DatePicker
                //    {
                //        Margin = new Thickness(2),
                //        Width = 300, // Giảm chiều rộng của ô
                //        BorderBrush = new SolidColorBrush(Colors.Black),
                //        BorderThickness = new Thickness(1),
                //        Foreground = new SolidColorBrush(Colors.White), // Chữ màu đỏ
                //        Background = new SolidColorBrush(Colors.Black) // Nền màu đen
                //    };
                //}
                //else
                //{
                //    element = new TextBox
                //    {
                //        Margin = new Thickness(2),
                //        PlaceholderText = $"R{currentRow + 1}C{col + 1}",
                //        BorderBrush = new SolidColorBrush(Colors.Black), // Sử dụng đúng cách
                //        BorderThickness = new Thickness(1),
                //        Foreground = new SolidColorBrush(Colors.Black),  // Đặt màu chữ đen
                //    };
                //}

                // Example usage for DatePicker and TextBox
                if (col == 1 || col == 8)
                {
                    var datePicker = new DatePicker
                    {
                        Margin = new Thickness(2),
                        Width = 300,
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                    };
                    AddHoverEffect(datePicker, Colors.Gray, Colors.Black); // Reuse hover effect
                    element = datePicker;
                }
                else
                {
                    var textBox = new TextBox
                    {
                        Margin = new Thickness(2),
                        PlaceholderText = $"R{currentRow + 1}C{col + 1}",
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1)
                    };
                    AddHoverEffect(textBox, Colors.Gray, Colors.Black); // Reuse hover effect
                    element = textBox;
                }

                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, currentRow);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }


            currentRow++; // Tăng số hàng hiện tại
        }



        private void OnAddRowDataClick(object sender, RoutedEventArgs e)
        {

            AddNewRow();
        }
        // Add event handlers for TextBox and DatePicker hover effects
        private void AddHoverEffect(Control controlElement, Color hoverColor, Color normalColor)
        {
            // Handle PointerEntered event (hover start)
            controlElement.PointerMoved += (sender, e) =>
            {
                if (controlElement is DatePicker datePicker)
                {
                    datePicker.BorderBrush = new SolidColorBrush(hoverColor);  // Change to hover color
                }
                else if (controlElement is TextBox textBox)
                {
                    textBox.Foreground = new SolidColorBrush(hoverColor);  // Change to hover color
                }
                //controlElement.BorderBrush = new SolidColorBrush(hoverColor);  // Change to hover color
            };

            // Handle PointerExited event (hover end)
            controlElement.PointerExited += (sender, e) =>
            {
                if (controlElement is DatePicker datePicker)
                {
                    datePicker.BorderBrush = new SolidColorBrush(normalColor);  // Change back to normal color
                }
                else if (controlElement is TextBox textBox)
                {
                    textBox.Foreground = new SolidColorBrush(normalColor);  // Change back to normal color
                }
                //controlElement.BorderBrush = new SolidColorBrush(normalColor);  // Change back to normal color
            };
        }
    }



    public class ServiceData
    {
        public int STT { get; set; }
        public string Ngay { get; set; }
        public string MaSoCV { get; set; }
        public string HoTenKH { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string LoaiModel { get; set; }
        public string Model { get; set; }
        public string NgayLapDat { get; set; }
        public string TenDichVu { get; set; }
        public decimal PhiDichVu { get; set; }
        public string KTV { get; set; }
        public string MaLK { get; set; }
        public string TenLK { get; set; }
        public string GhiChu { get; set; }
    }
}
