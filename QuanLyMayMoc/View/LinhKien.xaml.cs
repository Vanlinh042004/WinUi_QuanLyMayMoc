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
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";

        private int currentRow = 1;
        private int Columns = 3;
        private int EditingRowIndex = -1;
        private bool isUpdate = false;

        private Dictionary<int, Linhkien> rowLinkKienDictionary = new Dictionary<int, Linhkien>();
        private Dictionary<int, Linhkien> rowUpdateLinhkienDictionary = new Dictionary<int, Linhkien>();

        public MainViewModel ViewModel
        {
            get; set;
        }
        public LinhKien()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
            ViewModel = new MainViewModel();
            ViewModel.SaveToLinhKienTam(); // Lưu dữ liệu từ bảng linhkien vào bảng LinhKien_Tam
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
        private void OnLinhkienTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedLinhkien = (sender as FrameworkElement).DataContext as Linhkien;
            if (selectedLinhkien != null)
            {
                ViewModel.CurrentSelectedLinhkien = selectedLinhkien;
            }

            var grid = sender as Grid;
            var linhkien = grid.DataContext as Linhkien;

            foreach (var item in ViewModel.Listlinhkien)
            {
                item.IsSelected = false;
            }

            //if (linhkien != null)
            //{
                linhkien.IsSelected = true;
            //}
        }


     

        
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
                AddHoverEffect(textBox, Colors.Gray, Colors.Black); // Reuse hover effect
              
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

        private void ClearInputRows()
        {

            InputGrid.Children.Clear();


            currentRow = 1;
        }

        // Delete row data

        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại để người dùng nhập mã sản phẩm cần xóa
            //var inputTextBox = new TextBox();
            //var dialog = new ContentDialog
            //{
            //    Title = "Xác nhận xóa",
            //    Content = new StackPanel
            //    {
            //        Children =
            //{
            //    new TextBlock { Text = "Nhập Mã sản phẩm của linh kiện cần xóa:" },
            //    inputTextBox
            //}
            //    },
            //    PrimaryButtonText = "Xóa",
            //    CloseButtonText = "Hủy",
            //    XamlRoot = this.Content.XamlRoot // Đảm bảo ContentDialog được hiển thị trong ngữ cảnh của cửa sổ hiện tại
            //};

            //var result = await dialog.ShowAsync();

            //if (result == ContentDialogResult.Primary)
            //{
            //    string maSanPham = inputTextBox.Text;
            //    // Tìm selectedRow dựa trên mã sản phẩm
            //    for (int i = 1; i < InputGrid.RowDefinitions.Count; i++)
            //    {
            //        // Truy cập phần tử đầu tiên trong hàng thứ i (giả định mã sản phẩm nằm ở cột đầu tiên)
            //        var element = InputGrid.Children
            //            .Cast<UIElement>()
            //            .FirstOrDefault(e => Grid.GetRow((FrameworkElement)e) == i && Grid.GetColumn((FrameworkElement)e) == 0);

            //        if (element is Border border && border.Child is TextBlock textBlock)
            //        {

            //            if (textBlock.Text == maSanPham)
            //            {
            //                selectedRow = i;
            //                break;
            //            }
            //        }
            //    }

            //    // Xóa selectedRow nếu đã tìm thấy
            //    if (selectedRow != -1)
            //    {
            //        // Xóa các phần tử trong selectedRow
            //        for (int col = 0; col < Columns; col++)
            //        {
            //            var element = InputGrid.Children
            //                .Cast<UIElement>()
            //                .FirstOrDefault(e => Grid.GetRow((FrameworkElement)e) == selectedRow && Grid.GetColumn((FrameworkElement)e) == col);

            //            if (element != null)
            //            {
            //                InputGrid.Children.Remove(element);
            //            }
            //        }

            //        // Xóa định nghĩa hàng của selectedRow
            //        InputGrid.RowDefinitions.RemoveAt(selectedRow);

            //        // Đặt lại số hàng hiện tại
            //        currentRow--;
            //        // Xoa database
            //        DeleteRowFromDatabase(maSanPham);
            //    }
            //}
        }

  
      




        // Delete all row
        private void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
            //// Xóa tất cả các phần tử con, ngoại trừ hàng đầu tiên
            //for (int i = InputGrid.Children.Count - 1; i >= 0; i--)
            //{
            //    var element = InputGrid.Children[i];
            //    if (Grid.GetRow((FrameworkElement)element) > 0) // Chỉ xóa các phần tử không nằm ở hàng 0
            //    {
            //        InputGrid.Children.RemoveAt(i);
            //    }
            //}

            //// Xóa tất cả các định nghĩa hàng, ngoại trừ hàng đầu tiên
            //for (int row = InputGrid.RowDefinitions.Count - 1; row > 0; row--)
            //{
            //    InputGrid.RowDefinitions.RemoveAt(row);
            //}

            //// Đặt lại số hàng hiện tại
            //currentRow = 1;
            //// Xoa database
            //DeleteAllRowFromDatabase();
        }
     
        private void OnSaveDataClick(object sender, RoutedEventArgs e)
        {
            if (EditingRowIndex != -1 && isUpdate)
            {
                UpdateDataClick();
                ViewModel.CurrentSelectedLinhkien = null;
                rowUpdateLinhkienDictionary.Clear();
            }
            else
            {
                int stt = 0;
                foreach (var entry in rowLinkKienDictionary)
                {
                    stt++;
                    try
                    {

                        // Lưu dữ liệu vào ViewModel
                        SaveRowData(entry.Key);
                        ViewModel.Listlinhkien.Add(entry.Value);
                        // Thực hiện chèn dữ liệu vào bảng congviectamthoi
                        string mahieuduan = AppData.ProjectID + stt.ToString() + entry.Value.MaSanPham;
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
        }




        private void AddEditableRow(int rowIndex, Linhkien linhkien)
        {
            var newLinhkien = new Linhkien();
            rowUpdateLinhkienDictionary[rowIndex] = newLinhkien;
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;

               
                    var textBox = new TextBox
                    {
                        Margin = new Thickness(2),
                        PlaceholderText = $"R{rowIndex + 1}C{col + 1}",
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),

                        Text = linhkien.GetPropertyForColumn(col) // Hiển thị giá trị đã lưu
                    };
                    element = textBox;
                

                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, rowIndex);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
        }


        private void SaveUpdateRowData(int rowIndex)
        {
            try
            {
                if (rowUpdateLinhkienDictionary.TryGetValue(rowIndex, out Linhkien linhkien))
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
                    throw new KeyNotFoundException($"No linhkien found for row index {rowIndex}.");
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

        private async void UpdateDataClick()
        {
            try
            {
                isUpdate = false;
                if (rowUpdateLinhkienDictionary.TryGetValue(EditingRowIndex, out var linhkien))
                {
                    SaveUpdateRowData(EditingRowIndex);

                    ViewModel.UpdateSelectedLinhkien(linhkien);

                    ClearInputRows();

                    EditingRowIndex = -1;

                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = "Cập nhật dữ liệu thành công.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await successDialog.ShowAsync();
                    ViewModel.LoadDataFilter();
                }
                else
                {
                    // Hiển thị thông báo lỗi nếu không tìm thấy hàng đang chỉnh sửa
                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không tìm thấy hàng để cập nhật. Vui lòng kiểm tra lại.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi cập nhật dữ liệu: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
        }

        private async void OnUpdateRowDataClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentSelectedLinhkien != null)
            {
                isUpdate = true;
                var selectedLinhkien = ViewModel.CurrentSelectedLinhkien;

                // Tìm rowIndex bằng cách tìm vị trí của selectedLinhkien trong ListLinhkien
                var rowIndex = ViewModel.Listlinhkien.IndexOf(selectedLinhkien);

                if (rowIndex >= 0)
                {
                    EditingRowIndex = rowIndex; // Ghi nhớ chỉ số dòng đang chỉnh sửa
                    AddEditableRow(rowIndex, selectedLinhkien); // Thêm dòng chỉnh sửa
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy linhkien trong ListLinhkien
                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không tìm thấy linhkien trong danh sách.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
            else
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Bạn chưa chọn dòng.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await errorDialog.ShowAsync();
            }
        }

    }

}

