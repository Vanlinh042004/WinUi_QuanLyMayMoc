<<<<<<< HEAD
﻿using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //  private int previousRow = 1;
        private int Columns = 15;
        private Dictionary<int, Service> rowServiceDictionary = new Dictionary<int, Service>();

        public MainViewModel ViewModel
        {
            get; set;
        }

        public DichVuTheoThang()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
            ViewModel = new MainViewModel();

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
            var newService = new Service();

            // Lưu trữ đối tượng Service mới vào Dictionary
            rowServiceDictionary[currentRow] = newService;
            // Thêm dòng mới vào Grid
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;



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
                    AddHoverEffect(datePicker, Colors.Red, Colors.Black); // Reuse hover effect
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
                    AddHoverEffect(textBox, Colors.Red, Colors.Black); // Reuse hover effect
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
        private void OnServiceTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedService = (sender as FrameworkElement)?.DataContext as Service;
            if (selectedService != null)
            {
                ViewModel.CurrentSelectedService = selectedService;
            }
            // Lấy Grid (hoặc sender) và chuyển đổi thành Service
            var grid = sender as Grid;
            var service = grid.DataContext as Service;

            // Đặt lại trạng thái IsSelected cho tất cả các dòng
            foreach (var item in ViewModel.Services)
            {
                item.IsSelected = false; // Bỏ chọn các dòng khác
            }

            // Đánh dấu dòng đã chọn
            service.IsSelected = true;

            // Cập nhật lại danh sách để hiển thị sự thay đổi
            //  ViewModel.OnPropertyChanged(nameof(ViewModel.Services)); // Nếu cần
        }

        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
            // Create the confirmation dialog
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = "Bạn có chắc chắn muốn xóa dòng này?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot // Set the XamlRoot here
            };

            // Show the dialog and get the result
            ContentDialogResult result = await deleteDialog.ShowAsync();

            // Check if the user clicked OK
            if (result == ContentDialogResult.Primary)
            {
                // Perform deletion
                ViewModel.RemoveSelectedService();

                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Xóa thành công",
                    Content = "Dòng đã được xóa thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot // Set the XamlRoot here too
                };

                await successDialog.ShowAsync();
            }
        }

        private async void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
            // Create the confirmation dialog
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = "Bạn có chắc chắn muốn xóa tất cả dòng ?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot // Set the XamlRoot here
            };

            // Show the dialog and get the result
            ContentDialogResult result = await deleteDialog.ShowAsync();

            // Check if the user clicked OK
            if (result == ContentDialogResult.Primary)
            {
                // Perform deletion
                ViewModel.RemoveAllService();

                // Show success message
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Xóa thành công",
                    Content = "Tất cả các dòng đã được xóa thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot // Set the XamlRoot here too
                };

                await successDialog.ShowAsync();
            }
        }
        private void ClearInputRows()
        {
           
            InputGrid.Children.Clear();

           
            currentRow = 1;
        }

        private void SaveRowData(int rowIndex)
        {
            if (rowServiceDictionary.TryGetValue(rowIndex, out Service service))
            {
                for (int col = 0; col < Columns; col++)
                {
                    var element = InputGrid.Children
                        .OfType<FrameworkElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);

                    if (element is TextBox textBox)
                    {
                        service.SetPropertyForColumn(col, textBox.Text);
                    }
                    else if (element is DatePicker datePicker)
                    {
                        service.SetPropertyForDateColumn(col, datePicker.Date.DateTime);
                    }
                    else
                    {

                        ShowNotSuccessMessage("Lưu thất bại cho dữ liệu nhập vào không hợp lệ!");
                    }
                }
            }
        }

        private void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            var newService = new Service();
          
            foreach (var entry in rowServiceDictionary)
            {
                SaveRowData(entry.Key);
                ViewModel.Services.Add(entry.Value);
            }

            rowServiceDictionary.Clear(); 
            ShowSuccessMessage("Tất cả các dòng mới đã được lưu thành công.");
          
            ClearInputRows();
        }
        private async void ShowSuccessMessage(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Thành công",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Set the XamlRoot here too
            };

            await successDialog.ShowAsync();
        }
        private async void ShowNotSuccessMessage(string message)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "Fail",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Set the XamlRoot here too
            };

            await successDialog.ShowAsync();
        }

        private void OnFilterByDateClick(object sender, RoutedEventArgs e)
        {
            //if (filterDatePicker.SelectedDate.HasValue)
            //{
            //    DateTime selectedDate = filterDatePicker.Date.DateTime;

            //    ViewModel.LoadData(selectedDate);
            //    ShowSuccessMessage("Vui lòng chọn ngày lại!");
            //}
            //else
            //{
            //    ShowNotSuccessMessage("Vui lòng chọn ngày!");
            //}
           
        }

        private void OnClearFilterClick(object sender, RoutedEventArgs e)
        {

        }
    }
}







=======
﻿using Microsoft.UI;
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
>>>>>>> dd504fe8a2e3d58e48750f568e35dc2e74f0e281
