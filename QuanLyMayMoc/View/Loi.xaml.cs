using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Npgsql;
using QuanLyMayMoc.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Loi : Page
    {
        private int currentRow = 1;
        private int Columns = 3;
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
        private Dictionary<int, Loisp> rowLoiDictionary = new Dictionary<int, Loisp>();

        public MainViewModel ViewModel
        {
            get; set;
        }
        public Loi()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
            ViewModel = new MainViewModel();
            ViewModel.SaveToLoiTam(); // Lưu dữ liệu từ bảng loi vào bảng loi_Tam

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

        // Thêm dòng mới
        #region AddNewRow
        private void AddNewRow()
        {

            var newLoi = new Loisp();
            rowLoiDictionary[currentRow] = newLoi;
            // Thêm dòng mới vào Grid
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;
                var textBox = new TextBox
                {
                    Margin = new Thickness(2),
                    Background = new SolidColorBrush(Colors.White),
                    Foreground = new SolidColorBrush(Colors.Black),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1)
                };
                AddHoverEffect(textBox, Colors.Gray, Colors.Black); 

                element = textBox;
                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, currentRow);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
            currentRow++; // Tăng số hàng hiện tại
        }


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
            };
        }
        private void OnAddRowDataClick(object sender, RoutedEventArgs e)
        {

            AddNewRow();
        }
        #endregion


        // Luu
        private void SaveRowData(int rowIndex)
        {
            try
            {
                if (rowLoiDictionary.TryGetValue(rowIndex, out Loisp loi))
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        var element = InputGrid.Children
                            .OfType<FrameworkElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);


                        if (element is TextBox textBox)
                        {
                            loi.SetPropertyForColumn(col, textBox.Text);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unsupported element type encountered during save operation.");
                        }
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"No link kien found for row index {rowIndex}.");
                }
            }
            catch (KeyNotFoundException ex)
            {
                // Log lỗi hoặc xử lý khác
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                // Log lỗi hoặc xử lý khác
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc xử lý khác
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void ClearInputRows()
        {

            InputGrid.Children.Clear();


            currentRow = 1;
        }
        private void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            int stt = 0;
            foreach (var entry in rowLoiDictionary)
            {
                stt++;
                try
                {

                    // Lưu dữ liệu vào ViewModel
                    SaveRowData(entry.Key);
                    // Nếu chỉ thêm dòng mà không nhập mã sản phẩm thì không lưu
                    if (entry.Value.MaSanPham == null || entry.Value.MaSanPham == "")
                    {
                        break;

                    }
                    ViewModel.ListLoi.Add(entry.Value);
                    // Thực hiện chèn dữ liệu vào bảng congviectamthoi
                    string mahieuduan = entry.Value.MaSanPham + "_" + AppData.ProjectID;
                    ViewModel.InsertLoiToDaTaBaseTemp(entry.Value, mahieuduan);

                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Có lỗi xảy ra khi lưu linh kiện: {ex.Message}",
                        CloseButtonText = "OK"
                    };
                }
            }

            rowLoiDictionary.Clear();
            ClearInputRows();
        }


        // Xoa 1 dong

        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
            // Lấy dòng đang được chọn
            var selectedItem = (Loisp)LoiListView.SelectedItem;

            if (selectedItem != null)
            {
                // Xóa dòng khỏi ViewModel
                ViewModel.ListLoi.Remove(selectedItem);
                LoiListView.SelectedItem = null;
                ViewModel.DeleteLoiTam(selectedItem.MaSanPham);
                // Hiển thị thông báo nếu cần
                await new ContentDialog
                {
                    Title = "Thành công",
                    Content = "Linh kiện được xóa thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
            else
            {
                // Hiển thị thông báo nếu không có dòng nào được chọn
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Chưa có Linh Kiện nào được chọn.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }




        // Xoa tat ca
        private void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
            ViewModel.ListLoi.Clear();
            // Xóa tất cả các phần tử con, ngoại trừ hàng đầu tiên
            for (int i = InputGrid.Children.Count - 1; i >= 0; i--)
            {
                var element = InputGrid.Children[i];
                if (Grid.GetRow((FrameworkElement)element) > 0) // Chỉ xóa các phần tử không nằm ở hàng 0
                {
                    InputGrid.Children.RemoveAt(i);
                }
            }

            // Xóa tất cả các định nghĩa hàng, ngoại trừ hàng đầu tiên
            for (int row = InputGrid.RowDefinitions.Count - 1; row > 0; row--)
            {
                InputGrid.RowDefinitions.RemoveAt(row);
            }

            // Đặt lại số hàng hiện tại
            currentRow = 1;
            ViewModel.DeleteAllLoiTam();
        }
       

        
    }
}

