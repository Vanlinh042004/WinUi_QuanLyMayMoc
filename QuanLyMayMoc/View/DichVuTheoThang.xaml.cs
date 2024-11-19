

using Microsoft.UI;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Npgsql;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DichVuTheoThang : Page
    {
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";
        private int currentRow = 1;
        //  private int previousRow = 1;
        private int Columns = 16;
        private Dictionary<int, Task> rowTaskDictionary = new Dictionary<int, Task>();

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
            var newTask = new Task();

            // Lưu trữ đối tượng Task mới vào Dictionary
            rowTaskDictionary[currentRow] = newTask;
            // Thêm dòng mới vào Grid
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 0; col < Columns; col++)
            {
                FrameworkElement element;



                // Example usage for DatePicker and TextBox
                if (col == 1)
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
        private void OnTaskTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedTask = (sender as FrameworkElement)?.DataContext as Task;
            if (selectedTask != null)
            {
                ViewModel.CurrentSelectedTask = selectedTask;
            }
            // Lấy Grid (hoặc sender) và chuyển đổi thành Task
            var grid = sender as Grid;
            var service = grid.DataContext as Task;

            // Đặt lại trạng thái IsSelected cho tất cả các dòng
            foreach (var item in ViewModel.Tasks)
            {
                item.IsSelected = false; // Bỏ chọn các dòng khác
            }

            // Đánh dấu dòng đã chọn
            service.IsSelected = true;

            // Cập nhật lại danh sách để hiển thị sự thay đổi
            //  ViewModel.OnPropertyChanged(nameof(ViewModel.Tasks)); // Nếu cần
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
                // Perform deletion in ViewModel
                var selectedTask = ViewModel.CurrentSelectedTask; // Lưu lại dữ liệu trước khi xóa
                ViewModel.RemoveSelectedTask();

                // Perform deletion in database
                if (selectedTask != null)
                {
                    try
                    {
                        string deleteQuery = "DELETE FROM congviectamthoi WHERE macvduan = @macvduan";

                        using (var connection = new NpgsqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                            using (var command = new NpgsqlCommand(deleteQuery, connection))
                            {
                                command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);

                                await command.ExecuteNonQueryAsync();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Lỗi",
                            Content = $"Không thể xóa dòng trong cơ sở dữ liệu. Lỗi: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                        return; // Thoát nếu lỗi xảy ra
                    }
                }

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
                Content = "Bạn có chắc chắn muốn xóa tất cả dòng?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot // Set the XamlRoot here
            };

            // Show the dialog and get the result
            ContentDialogResult result = await deleteDialog.ShowAsync();

            // Check if the user clicked OK
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    // Perform deletion in ViewModel
                    ViewModel.RemoveAllTask();

                    // Perform deletion in the database
                    string deleteQuery = "DELETE FROM congviectamthoi";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (var command = new NpgsqlCommand(deleteQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    // Show success message
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Xóa thành công",
                        Content = "Tất cả các dòng đã được xóa thành công trong cơ sở dữ liệu và giao diện.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot // Set the XamlRoot here too
                    };

                    await successDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Không thể xóa tất cả dòng trong cơ sở dữ liệu. Lỗi: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }
        private void ClearInputRows()
        {

            InputGrid.Children.Clear();


            currentRow = 1;
        }

        private void SaveRowData(int rowIndex)
        {
            if (rowTaskDictionary.TryGetValue(rowIndex, out Task service))
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

        private async void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            // var newTask = new Task() { MaDuAn=AppData.ProjectID};
            //  ShowNotSuccessMessage("Tất cả các dòng mới đã được lưu thành công.");


            foreach (var entry in rowTaskDictionary)
            {
                entry.Value.MaDuAn = AppData.ProjectID;

                // Truy vấn số lượng dòng hiện tại từ congviectamthoi
                int currentRowCount;
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var countCommand = new NpgsqlCommand("SELECT COUNT(*) FROM congviectamthoi", connection))
                    {
                        currentRowCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
                    }
                }

                // Gán MaCVDuAn dựa trên số lượng dòng hiện có + 1
                string date = DateTime.Now.ToString("yyyy_MM_dd");
                string time = DateTime.Now.ToString("HH_mm_ss");
                entry.Value.MaCVDuAn = AppData.ProjectID +"_"+date+"_"+ time+"_"+(currentRowCount + 1).ToString();

                SaveRowData(entry.Key);
                ViewModel.Tasks.Add(entry.Value);

                // Prepare the INSERT statement for all fields
                string insertCongViecQuery = @"
        INSERT INTO congviectamthoi (
            stt, macvduan, ngaythuchien, hotenkh, sdt, diachi, tendichvu, manv, tennv, malinhkien, 
            tenlinhkien, soluonglinhkien, maloi, tenloi, soluongloi, phidichvu, ghichu, maduan
        ) VALUES (
            @stt, @macvduan, @ngaythuchien, @hotenkh, @sdt, @diachi, @tendichvu, @manv, @tennv, @malinhkien, 
            @tenlinhkien, @soluonglinhkien, @maloi, @tenloi, @soluongloi, @phidichvu, @ghichu, @maduan
        )";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(insertCongViecQuery, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@stt", entry.Value.Stt);
                        command.Parameters.AddWithValue("@macvduan", entry.Value.MaCVDuAn); // Mã công việc
                        command.Parameters.AddWithValue("@ngaythuchien", entry.Value.NgayThucHien == DateTime.MinValue ? (object)DBNull.Value : entry.Value.NgayThucHien);
                        command.Parameters.AddWithValue("@hotenkh", entry.Value.HoTenKH ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@sdt", entry.Value.SDT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@diachi", entry.Value.DiaChi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tendichvu", entry.Value.TenDichVu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@manv", entry.Value.MaNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tennv", entry.Value.TenNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@malinhkien", entry.Value.MaLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenlinhkien", entry.Value.TenLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluonglinhkien", entry.Value.SoLuongLK);
                        command.Parameters.AddWithValue("@maloi", entry.Value.MaLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenloi", entry.Value.TenLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluongloi", entry.Value.SoLuongLoi);
                        command.Parameters.AddWithValue("@phidichvu", entry.Value.PhiDichVu);
                        command.Parameters.AddWithValue("@ghichu", entry.Value.GhiChu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@maduan", entry.Value.MaDuAn ?? (object)DBNull.Value);

                        // Execute the query
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }



            rowTaskDictionary.Clear();
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
                Title = "Thất bại",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot // Set the XamlRoot here too
            };

            await successDialog.ShowAsync();
        }

        private void OnFilterByDateClick(object sender, RoutedEventArgs e)
        {
            if (filterDatePicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = filterDatePicker.Date.Date;

                ViewModel.LoadDataFilter(selectedDate);
               
            }
            else
            {
                ShowNotSuccessMessage("Vui lòng chọn ngày!");
            }

        }

        private void OnClearFilterClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadDataFilter();
        }

        
    }
}


