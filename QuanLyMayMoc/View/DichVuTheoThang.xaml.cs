

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
using System.Threading.Tasks;

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
       
        private int Columns = 16;
        private Dictionary<int, Task> rowTaskDictionary = new Dictionary<int, Task>();

        public MainViewModel ViewModel
        {
            get; set;
        }

        public DichVuTheoThang()
        {
            this.InitializeComponent();
            HideFirstRow(); 
            ViewModel = new MainViewModel();

        }
        private void HideFirstRow()
        {
            
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

         
            var emptyElement = new TextBox
            {
                Visibility = Visibility.Collapsed 
            };

           
            Grid.SetRow(emptyElement, 0);
            Grid.SetColumn(emptyElement, 0);
            InputGrid.Children.Add(emptyElement);
        }
      
        private void AddNewRow()
        {
            var newTask = new Task();

           
            rowTaskDictionary[currentRow] = newTask;
           
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 1; col < Columns; col++)
            {
                FrameworkElement element;



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
                    AddHoverEffect(datePicker, Colors.Red, Colors.Black); 
                    element = datePicker;
                }
                else if (col==2)
                {
                    var autoSuggestBox = new AutoSuggestBox
                    {
                        Margin = new Thickness(2),
                        PlaceholderText = $"Nhập họ tên khách hàng",
                        Width = 300,
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1)
                    };
                    autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                    autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
                    element = autoSuggestBox;
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
                    AddHoverEffect(textBox, Colors.Red, Colors.Black); 
                    element = textBox;
                }

                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, currentRow);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
            currentRow++; // Tăng số hàng hiện tại

        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var query = sender.Text;

                if (!string.IsNullOrEmpty(query))
                {
                    try
                    {
                        var suggestions = ViewModel.GetCustomerNames(query);
                        sender.ItemsSource = suggestions;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error fetching suggestions: {ex.Message}");
                    }
                }
                else
                {
                    sender.ItemsSource = null;
                }
            }
        }

        


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }



        private void OnAddRowDataClick(object sender, RoutedEventArgs e)
        {

            AddNewRow();
        }
       
        private void AddHoverEffect(Control controlElement, Color hoverColor, Color normalColor)
        {
           
            controlElement.PointerMoved += (sender, e) =>
            {
                if (controlElement is DatePicker datePicker)
                {
                    datePicker.BorderBrush = new SolidColorBrush(hoverColor);  
                }
                else if (controlElement is TextBox textBox)
                {
                    textBox.Foreground = new SolidColorBrush(hoverColor);  
                }
              
            };

         
            controlElement.PointerExited += (sender, e) =>
            {
                if (controlElement is DatePicker datePicker)
                {
                    datePicker.BorderBrush = new SolidColorBrush(normalColor);  
                }
                else if (controlElement is TextBox textBox)
                {
                    textBox.Foreground = new SolidColorBrush(normalColor);  
                }
               
            };
        }
        private void OnTaskTapped(object sender, TappedRoutedEventArgs e)
        {
            var selectedTask = (sender as FrameworkElement)?.DataContext as Task;
            if (selectedTask != null)
            {
                ViewModel.CurrentSelectedTask = selectedTask;
            }
          
            var grid = sender as Grid;
            var service = grid.DataContext as Task;

          
            foreach (var item in ViewModel.Tasks)
            {
                item.IsSelected = false; 
            }

          
            service.IsSelected = true;

        }

        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
           
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = "Bạn có chắc chắn muốn xóa dòng này?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot 
            };

           
            ContentDialogResult result = await deleteDialog.ShowAsync();

          
            if (result == ContentDialogResult.Primary)
            {
               
               
                ViewModel.RemoveSelectedTask();

                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Xóa thành công",
                    Content = "Dòng đã được xóa thành công.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot 
                };

                await successDialog.ShowAsync();
            }
            ViewModel.LoadDataFilter();
        }



        private async void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
           
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = "Bạn có chắc chắn muốn xóa tất cả dòng?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot 
            };

         
            ContentDialogResult result = await deleteDialog.ShowAsync();

          
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                   
                    ViewModel.RemoveAllTask();

                   
                    string deleteQuery = "DELETE FROM congviectamthoi";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        using (var command = new NpgsqlCommand(deleteQuery, connection))
                        {
                            await command.ExecuteNonQueryAsync();
                        }
                    }

                   
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Xóa thành công",
                        Content = "Tất cả các dòng đã được xóa thành công trong cơ sở dữ liệu và giao diện.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot 
                    };

                    await successDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                   
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
            try
            {
                if (rowTaskDictionary.TryGetValue(rowIndex, out Task service))
                {
                    for (int col = 1; col < Columns; col++)
                    {
                        var element = InputGrid.Children
                            .OfType<FrameworkElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);

                        if (element is AutoSuggestBox autoSuggestBox)
                        {
                            service.HoTenKH = autoSuggestBox.Text;
                        }
                        else if (element is TextBox textBox)
                        {
                            service.SetPropertyForColumn(col, textBox.Text);
                        }
                        else if (element is DatePicker datePicker)
                        {
                            service.SetPropertyForDateColumn(col, datePicker.Date.DateTime);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unsupported element type encountered during save operation.");
                        }
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"No task found for row index {rowIndex}.");
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


        private async void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            foreach (var entry in rowTaskDictionary)
            {
                entry.Value.MaDuAn = AppData.ProjectID;

              
                int maxStt = 0;
                maxStt = ViewModel.TimSttLonNhat(entry.Value.MaDuAn);

             
                int stt = maxStt + 1;
                entry.Value.Stt = stt;

                // Tạo MaCVDuAn
                string date = DateTime.Now.ToString("yyyy_MM_dd");
                string time = DateTime.Now.ToString("HH_mm_ss");
                entry.Value.MaCVDuAn = $"{AppData.ProjectID}_{date}_{time}_{stt}";

              
              
                try
                {
                    // Lưu dữ liệu vào ViewModel
                    SaveRowData(entry.Key);
                    ViewModel.Tasks.Add(entry.Value);
                    // Thực hiện chèn dữ liệu vào bảng congviectamthoi
                    ViewModel.InsertTaskToDatabaseTemp(entry.Value);

                }
                catch (Exception ex) {
                    var dialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Có lỗi xảy ra khi lưu công việc: {ex.Message}",
                        CloseButtonText = "OK"
                    };
                }
            }

            rowTaskDictionary.Clear();
            ClearInputRows();
        }



        private void OnFilterByDateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filterDatePicker.SelectedDate.HasValue) // Kiểm tra xem ngày có được chọn không
                {
                    DateTime selectedDate = filterDatePicker.SelectedDate.Value.Date; // Lấy giá trị ngày đã chọn
                    string keyword = SearchTextBox.Text; // Lấy từ khóa từ hộp tìm kiếm

                    ViewModel.LoadDataFilter(selectedDate, keyword); // Gọi phương thức load dữ liệu
                }
                else
                {
                    throw new InvalidOperationException("Date is not selected. Please select a date!");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Log lỗi hoặc hiển thị thông báo lỗi
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi chung
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }



        private void OnClearFilterClick(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadDataFilter();
        }

        private void OnSearchClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu người dùng không chọn ngày thì mặc định là DateTime.MinValue
            DateTime selectedDate = filterDatePicker.SelectedDate.HasValue
                ? filterDatePicker.SelectedDate.Value.Date // Lấy giá trị ngày nếu có
                : DateTime.MinValue; // Ngày mặc định nếu không chọn

           
            string keyword = SearchTextBox.Text;

          
            ViewModel.LoadDataFilter(selectedDate, keyword);
        }


    }
}


