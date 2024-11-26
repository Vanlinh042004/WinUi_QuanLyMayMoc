using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.ViewModel;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinhKien : Page
    {
        private int currentRow = 1;
        private int Columns = 3;
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
        private Dictionary<int, Linhkien> rowLinkKienDictionary = new Dictionary<int, Linhkien>();

        public MainViewModel ViewModel
        {
            get; set;
        }
        public LinhKien()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
<<<<<<< HEAD
            ViewModel = new MainViewModel(); 
            ViewModel.SaveToLinhKienTam(); // Lưu dữ liệu từ bảng linhkien vào bảng LinhKien_Tam
=======
            ViewModel = new MainViewModel();
            SaveToLinhKienTam(); // Lưu dữ liệu từ bảng linhkien vào bảng LinhKien_Tam
        }
        private void OnTaskTapped(object sender, TappedRoutedEventArgs e)
        {


>>>>>>> c52b2f52ae2ee76d25b731b3b5255c0f6ff245cf
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

<<<<<<< HEAD
        
        
=======

        private async void SaveToLinhKienTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        // Nếu dự án chưa tồn tại, thêm vào bảng `duan_tam`
                        string insertQuery = @" INSERT INTO linhkienduantam (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                SELECT CONCAT(mahieu,'_', @maDuAn), mahieu, tenlinhkien, giaban, @maDuAn
                                                FROM linhkien";
                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }
>>>>>>> c52b2f52ae2ee76d25b731b3b5255c0f6ff245cf
        // Thêm dòng mới
        #region AddNewRow
        private void AddNewRow()
        {

            var newLinhKien = new Linhkien();
            rowLinkKienDictionary[currentRow] = newLinhKien;
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
<<<<<<< HEAD
                AddHoverEffect(textBox, Colors.Gray, Colors.Black); 
=======
                AddHoverEffect(textBox, Colors.Gray, Colors.Black); // Reuse hover effect
>>>>>>> c52b2f52ae2ee76d25b731b3b5255c0f6ff245cf

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


        // Lưu
        private void SaveRowData(int rowIndex)
        {
            try
            {
                if (rowLinkKienDictionary.TryGetValue(rowIndex, out Linhkien linhkien))
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        var element = InputGrid.Children
                            .OfType<FrameworkElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);


                        if (element is TextBox textBox)
                        {
                            linhkien.SetPropertyForColumn(col, textBox.Text);
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

        private void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            int stt = 0;
            foreach (var entry in rowLinkKienDictionary)
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
                    ViewModel.linhkien.Add(entry.Value);
                    // Thực hiện chèn dữ liệu vào bảng congviectamthoi
                    string mahieuduan = entry.Value.MaSanPham + "_" + AppData.ProjectID;
                    ViewModel.InsertLinhKienToDaTaBaseTemp(entry.Value, mahieuduan);

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

            rowLinkKienDictionary.Clear();
            ClearInputRows();

        }
        private void ClearInputRows()
        {

            InputGrid.Children.Clear();


            currentRow = 1;
        }


        // Delete row data
        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
            // Lấy dòng đang được chọn
            var selectedItem = (Linhkien)LinhKienListView.SelectedItem;

            if (selectedItem != null)
            {
                // Xóa dòng khỏi ViewModel
                ViewModel.linhkien.Remove(selectedItem);
                LinhKienListView.SelectedItem = null;
                ViewModel.DeleteLinhKienTam(selectedItem.MaSanPham);
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

        // Delete all row
        private void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
            ViewModel.linhkien.Clear();
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
            ViewModel.DeleteAllLinhKienTam();
        }
    }
<<<<<<< HEAD












      
   
  
}
=======
>>>>>>> c52b2f52ae2ee76d25b731b3b5255c0f6ff245cf

}
