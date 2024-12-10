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
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
        private int currentRow = 1;
        private int Columns = 3;
        private int EditingRowIndex = -1;
        private bool isUpdate = false;
        private Dictionary<int, Loisp> rowLoispDictionary = new Dictionary<int, Loisp>();
        private Dictionary<int, Loisp> rowUpdateLoispDictionary = new Dictionary<int, Loisp>();

        public MainViewModel ViewModel
        {
            get; set;
        }
        public Loi()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
            ViewModel = new MainViewModel();
            if (ViewModel.CheckLoiDuAnTonTai(AppData.ProjectID) > 0 || ViewModel.CheckLoiDuAnTamTonTai(AppData.ProjectID) > 0)
            {
                ViewModel.LoadLoiFromTemp();
            }
            else
            {
                ViewModel.LoadLoiFromDatabase();
            }
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Quản lý lõi");
            };
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
            rowLoispDictionary[currentRow] = newLoi;

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


        // Xóa 1 dòng
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

      

        // Xóa tất cả
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
      
        // Sửa


        private void AddEditableRow(int rowIndex, Loisp loisp)
        {
            var newLoi = new Loisp();
            rowUpdateLoispDictionary[rowIndex] = newLoi;
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;

                if (col == 0)
                {
                    // Cột 0 không tạo giao diện, chỉ lưu vào dictionary
                    newLoi.SetPropertyForColumn(0, loisp.GetPropertyForColumn(0));
                    continue;
                }
                var textBox = new TextBox
                {
                    Margin = new Thickness(2),
                    PlaceholderText = $"R{rowIndex + 1}C{col + 1}",
                    Background = new SolidColorBrush(Colors.White),
                    Foreground = new SolidColorBrush(Colors.Black),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1),

                    Text = loisp.GetPropertyForColumn(col) // Hiển thị giá trị đã lưu
                };
                element = textBox;


                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, rowIndex);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
        }

        private async void OnUpdateRowDataClick(object sender, RoutedEventArgs e)
        {
            var CurrentSelectedLoisp = (Loisp)LoiListView.SelectedItem;
            if (CurrentSelectedLoisp != null)
            {
                isUpdate = true;

                // Tìm rowIndex bằng cách tìm vị trí của selectedLoisp trong ListLoisp
                var rowIndex = ViewModel.ListLoi.IndexOf(CurrentSelectedLoisp);

                if (rowIndex >= 0)
                {
                    EditingRowIndex = rowIndex; // Ghi nhớ chỉ số dòng đang chỉnh sửa
                    AddEditableRow(rowIndex, CurrentSelectedLoisp); // Thêm dòng chỉnh sửa
                }
                else
                {
                    // Xử lý trường hợp không tìm thấy loisp trong ListLoisp
                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Không tìm thấy loisp trong danh sách.",
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

        // Luu
        private void SaveUpdateRowData(int rowIndex)
        {
            try
            {
                if (rowUpdateLoispDictionary.TryGetValue(rowIndex, out Loisp loisp))
                {
                    for (int col = 0; col < Columns; col++)
                    {
                        if (col == 0) continue;

                        var element = InputGrid.Children
                            .OfType<FrameworkElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);

                        if (element is TextBox textBox)
                        {
                            loisp.SetPropertyForColumn(col, textBox.Text);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unsupported element type encountered during save operation.");
                        }
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"No loisp found for row index {rowIndex}.");
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

        private async void UpdateDataClick(Loisp selectedLoi)
        {
            try
            {
                isUpdate = false;
                if (rowUpdateLoispDictionary.TryGetValue(EditingRowIndex, out var loisp))
                {
                    SaveUpdateRowData(EditingRowIndex);
                    ViewModel.UpdateSelectedLoi(loisp, selectedLoi);
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

        private void SaveRowData(int rowIndex)
        {
            try
            {
                if (rowLoispDictionary.TryGetValue(rowIndex, out Loisp loi))
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
        private async void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            if (EditingRowIndex != -1 && isUpdate)
            {
                var selectedLoi = (Loisp)LoiListView.SelectedItem;
                UpdateDataClick(selectedLoi);
                LoiListView.SelectedItem = null;
                rowUpdateLoispDictionary.Clear();
            }
            else
            {
                foreach (var entry in rowLoispDictionary)
                {
                    try
                    {
                        // Lưu dữ liệu vào ViewModel
                        SaveRowData(entry.Key);
                        // Nếu chỉ thêm dòng mà không nhập mã sản phẩm thì không lưu
                        if (entry.Value.MaSanPham == null || entry.Value.MaSanPham == "")
                        {
                            break;

                        }
                        // Thông báo không cho lưu nếu mã sản phẩm đã tồn tại
                        if (await ViewModel.CheckLoiTonTai(entry.Value.MaSanPham))
                        {
                            var dialog = new ContentDialog
                            {
                                Title = "Lỗi",
                                Content = $"Mã sản phẩm {entry.Value.MaSanPham} đã tồn tại.",
                                CloseButtonText = "OK",
                                XamlRoot = this.XamlRoot
                            };
                            await dialog.ShowAsync();
                            break;
                        }
                        ViewModel.ListLoi.Add(entry.Value);
                        // Thực hiện chèn dữ liệu vào bảng loiduantam
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

                rowLoispDictionary.Clear();
                ClearInputRows();
            }
        }
    }
}

