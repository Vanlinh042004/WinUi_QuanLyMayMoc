

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
using Microsoft.UI.Xaml;
using System.Threading;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DichVuTheoThang : Page
    {
        private int EditingRowIndex = -1; // Giá trị mặc định là -1, nghĩa là không có dòng nào đang chỉnh sửa

        
        private int currentRow = 1;
        private bool isUpdate = false;

        private int Columns = 13;
        private Dictionary<int, Task> rowTaskDictionary = new Dictionary<int, Task>();
        private Dictionary<int, Task> rowUpdateTaskDictionary = new Dictionary<int, Task>();

        private DispatcherTimer refreshTimer; // Timer để tự động refresh
        public MainViewModel ViewModel
        {
            get; set;
        }

        public DichVuTheoThang()
        {
            this.InitializeComponent();
            HideFirstRow();
            ViewModel = new MainViewModel();
            //ViewModel.DeleteAllLinhKienTam();
            //ViewModel.DeleteAllLoiTam();
            ViewModel.RefreshData();


            // Thiết lập Timer
            InitializeRefreshTimer();

            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Dịch vụ theo tháng");
            };
            //AddNewRow();
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
        List<String> PlaceHolderText = new List<String> { "", "", "", "Nhập số điện thoại", "Nhập địa chỉ", "Nhập tên dịch vụ", "Nhập phí dịch vụ", "Nhập mã linh kiện", "Nhập số lượng linh kiện", "Nhập mã lõi", "Nhập số lượng lõi", "Nhập mã nhân viên", "Nhập ghi chú" };
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
                        RequestedTheme = ElementTheme.Light,
                        Width = 300,
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                    };
                    AddHoverEffect(datePicker, Colors.Red, Colors.Black);
                    element = datePicker;
                }
                else if (col == 2)
                {
                    element = CreateCustomerNameAutoSuggestBox();
                }
                else if (col == 11)
                {
                    element = CreateEmployeeCodeAutoSuggestBox();
                }
                else if (col == 7)
                {
                    element = CreatePartCodeAutoSuggestBox();
                }
                else if (col == 9)
                {
                    element = CreateCoreCodeAutoSuggestBox();
                }

                else
                {
                    int maxLength = 0;
                    if (col == 3)
                    {
                        maxLength = 10;
                    }
                    else
                    {
                        maxLength = 100; // Mặc định 100 ký tự
                    }
                    var textBox = new TextBox
                    {
                        Margin = new Thickness(2),
                        RequestedTheme = ElementTheme.Light,
                        PlaceholderText = PlaceHolderText[col],
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        MaxLength = maxLength
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


        // Phương thức này sẽ được gọi khi người dùng thay đổi văn bản trong AutoSuggestBox
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var query = sender.Text;

                if (!string.IsNullOrEmpty(query))
                {
                    try
                    {
                        List<string> suggestions = null;

                        if (sender.PlaceholderText.Contains("họ tên khách hàng"))
                        {
                            // Lấy danh sách gợi ý họ tên khách hàng
                            suggestions = ViewModel.GetCustomerNames(query);
                        }
                        else if (sender.PlaceholderText.Contains("mã nhân viên"))
                        {
                            // Lấy danh sách gợi ý mã nhân viên
                            suggestions = ViewModel.GetEmployeeCodes(query);
                        }
                        else if (sender.PlaceholderText.Contains("mã linh kiện"))
                        {
                            // Lấy danh sách gợi ý mã linh kiện
                            suggestions = ViewModel.GetPartCodes(query);
                        }
                        else if (sender.PlaceholderText.Contains("mã lõi"))
                        {
                            // Lấy danh sách gợi ý mã lõi
                            suggestions = ViewModel.GetCoreCodes(query);
                        }

                        //if (suggestions != null)
                        //{
                        sender.ItemsSource = suggestions;
                        // }
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

        // Sự kiện khi người dùng chọn một gợi ý từ AutoSuggestBox
        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }

        private AutoSuggestBox CreateCustomerNameAutoSuggestBox()
        {
            var autoSuggestBox = new AutoSuggestBox
            {
                Margin = new Thickness(2),
                RequestedTheme = ElementTheme.Light,
                PlaceholderText = "Nhập họ tên khách hàng",
                Width = 300,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1),
                
            };
            autoSuggestBox.TextChanged += (sender, args) =>
            {
                HandleAutoSuggestBoxTextChanged(sender, args, "customer");
            };
            autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            return autoSuggestBox;
        }

        private AutoSuggestBox CreateEmployeeCodeAutoSuggestBox()
        {
            var autoSuggestBox = new AutoSuggestBox
            {
                Margin = new Thickness(2),
                RequestedTheme = ElementTheme.Light,
                PlaceholderText = "Nhập mã nhân viên",
                Width = 300,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            autoSuggestBox.TextChanged += (sender, args) =>
            {
                HandleAutoSuggestBoxTextChanged(sender, args, "employee");
            };
            autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            return autoSuggestBox;
        }

        private AutoSuggestBox CreatePartCodeAutoSuggestBox()
        {
            var autoSuggestBox = new AutoSuggestBox
            {
                Margin = new Thickness(2),
                RequestedTheme = ElementTheme.Light,
                PlaceholderText = "Nhập mã linh kiện",
                Width = 300,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            autoSuggestBox.TextChanged += (sender, args) =>
            {
                HandleAutoSuggestBoxTextChanged(sender, args, "part");
            };
            autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            return autoSuggestBox;
        }

        private AutoSuggestBox CreateCoreCodeAutoSuggestBox()
        {
            var autoSuggestBox = new AutoSuggestBox
            {
                Margin = new Thickness(2),
                RequestedTheme = ElementTheme.Light,
                PlaceholderText = "Nhập mã lõi",
                Width = 300,
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Colors.Black),
                BorderBrush = new SolidColorBrush(Colors.Black),
                BorderThickness = new Thickness(1)
            };
            autoSuggestBox.TextChanged += (sender, args) =>
            {
                HandleAutoSuggestBoxTextChanged(sender, args, "core");
            };
            autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;

            return autoSuggestBox;
        }

        private void HandleAutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args, string type)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var query = sender.Text;

                if (!string.IsNullOrEmpty(query))
                {
                    try
                    {
                        List<string> suggestions = null;

                        switch (type)
                        {
                            case "customer":
                                suggestions = ViewModel.GetCustomerNames(query);
                                break;
                            case "employee":
                                suggestions = ViewModel.GetEmployeeCodes(query);
                                break;
                            case "part":
                                suggestions = ViewModel.GetPartCodes(query);
                                break;
                            case "core":
                                suggestions = ViewModel.GetCoreCodes(query);
                                break;
                        }

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

            if (ViewModel.CurrentSelectedTask == null)
            {
                ContentDialog noSelectionDialog = new ContentDialog
                {
                    Title = "Không có dòng nào được chọn",
                    Content = "Vui lòng chọn một dòng trước khi thực hiện xóa.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await noSelectionDialog.ShowAsync();
                return; 
            }

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
            ViewModel.RefreshData();
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
                    ViewModel.DeleteAllTask(AppData.ProjectID);
                    ViewModel.RefreshData();



                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Xóa thành công",
                        Content = "Tất cả các dòng đã được xóa thành công.",
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

                        // AutoSuggestBox
                        if (element is AutoSuggestBox autoSuggestBox)
                        {
                            if (autoSuggestBox.PlaceholderText.Contains("họ tên khách hàng"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.HoTenKH = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Họ tên khách hàng không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã nhân viên"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaNV = autoSuggestBox.Text;
                                }
                                else
                                {
                                    service.MaNV = "unkhow";
                                    Console.WriteLine("Mã nhân viên không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã linh kiện"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaLK = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Mã linh kiện không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã lõi"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaLoi = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Mã lõi không được để trống.");
                                }
                            }
                        }
                        // TextBox
                        else if (element is TextBox textBox)
                        {
                            service.SetPropertyForColumn(col, textBox.Text);
                        }
                        // DatePicker
                        else if (element is DatePicker datePicker)
                        {
                            if (datePicker.Date != null) // Kiểm tra nếu datePicker có dữ liệu
                            {
                                service.SetPropertyForDateColumn(col, datePicker.Date.DateTime);
                            }
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



        private void OnSaveRowDataClick(object sender, RoutedEventArgs e)
        {
            if (EditingRowIndex != -1&& isUpdate)
            {
                UpdateDataClick();
               ViewModel.CurrentSelectedTask = null;
                rowUpdateTaskDictionary.Clear();
              
               
            }
            else
            {
                foreach (var entry in rowTaskDictionary)
                {
                    entry.Value.MaDuAn = AppData.ProjectID;

                    try
                    {
                        // Lưu dữ liệu vào ViewModel
                        SaveRowData(entry.Key);
                        if (entry.Value.NgayThucHien == new DateTime(1601, 1, 1, 7, 0, 0))
                        {
                            entry.Value.NgayThucHien = DateTime.Now;
                        }
                        ViewModel.Tasks.Add(entry.Value);
                        int maxStt = 0;
                        maxStt = ViewModel.TimSttLonNhat(entry.Value.MaDuAn);


                        int stt = maxStt + 1;
                        entry.Value.Stt = stt;

                        // Tạo MaCVDuAn
                        string date = DateTime.Now.ToString("yyyy_MM_dd");
                        string time = DateTime.Now.ToString("HH_mm_ss");
                        entry.Value.MaCVDuAn = $"{AppData.ProjectID}_{date}_{time}_{stt}";
                        // Thực hiện chèn dữ liệu vào bảng congviectamthoi
                        ViewModel.InsertTaskToDaTaBaseTemp(entry.Value);


                    }
                    catch (Exception ex)
                    {
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
                ViewModel.RefreshData();

            }
          

        }




        private async void OnFilterByDateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (filterDatePicker.SelectedDate.HasValue) // Kiểm tra xem ngày có được chọn không
                {
                    DateTime selectedDate = filterDatePicker.SelectedDate.Value.Date; // Lấy giá trị ngày đã chọn
                    string keyword = SearchTextBox.Text; // Lấy từ khóa từ hộp tìm kiếm

                    ViewModel.RefreshData(selectedDate, keyword); // Gọi phương thức load dữ liệu
                }
                else
                {
                    // Hiển thị thông báo yêu cầu chọn ngày
                    var dialog = new ContentDialog
                    {
                        Title = "Thông báo",
                        Content = "Bạn chưa chọn ngày. Vui lòng chọn ngày để tiếp tục!",
                        CloseButtonText = "OK",
                        DefaultButton = ContentDialogButton.Close,
                        XamlRoot = this.XamlRoot // Gắn XamlRoot cho ContentDialog
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi chung
                var dialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Đã xảy ra lỗi: {ex.Message}",
                    CloseButtonText = "OK",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = this.XamlRoot
                };
                await dialog.ShowAsync();
            }

        }



        private void OnClearFilterClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RefreshData();
            
        }



        private async void OnSearchClick(object sender, RoutedEventArgs e)
        {
            // Kiểm tra nếu người dùng không chọn ngày thì mặc định là DateTime.MinValue
            DateTime selectedDate = filterDatePicker.SelectedDate.HasValue
                ? filterDatePicker.SelectedDate.Value.Date // Lấy giá trị ngày nếu có
                : DateTime.MinValue; // Ngày mặc định nếu không chọn

            string keyword = SearchTextBox.Text;

            // Kiểm tra nếu từ khóa rỗng hoặc chỉ chứa khoảng trắng
            if (string.IsNullOrWhiteSpace(keyword))
            {
                // Hiển thị thông báo yêu cầu nhập từ khóa
                var dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Vui lòng nhập mã nhân viên để tìm kiếm.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot // Đảm bảo dialog hiển thị trên cửa sổ hiện tại
                };

                await dialog.ShowAsync();
                return; // Dừng thực thi nếu không có từ khóa
            }

            // Gọi ViewModel để làm mới dữ liệu
            ViewModel.RefreshData(selectedDate, keyword);
        }


        private async void  OnUpdateRowDataClick(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentSelectedTask == null)
            {
                ContentDialog noSelectionDialog = new ContentDialog
                {
                    Title = "Không có dòng nào được chọn",
                    Content = "Vui lòng chọn một dòng trước khi thực hiện chỉnh sửa.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await noSelectionDialog.ShowAsync();
                return;
            }

            ContentDialog deleteDialog = new ContentDialog
            {
                Title = "Xác nhận chỉnh sửa",
                Content = "Bạn có chắc chắn muốn chỉnh sửa dòng này?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Hủy",
                XamlRoot = this.XamlRoot
            };


            ContentDialogResult result = await deleteDialog.ShowAsync();


            if (result == ContentDialogResult.Primary)
            {
               
                isUpdate=true;
                var selectedTask = ViewModel.CurrentSelectedTask;
               
                var rowIndex = selectedTask.Stt;

                EditingRowIndex = rowIndex; // Ghi nhớ chỉ số dòng đang chỉnh sửa
                //EditingRowIndex = 1; // Ghi nhớ chỉ số dòng đang chỉnh sửa
                AddEditableRow(rowIndex, selectedTask); // Thêm dòng chỉnh sửa
            }
        }

        private void AddEditableRow(int rowIndex, Task task)
        {
            // Gán Task cần chỉnh sửa vào Dictionary
            rowUpdateTaskDictionary[rowIndex] = task;

            // Thêm một dòng vào Grid
            InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (int col = 1; col < Columns; col++)
            {
                FrameworkElement element;

                if (col == 1) // DatePicker
                {
                    var datePicker = new DatePicker
                    {
                        Margin = new Thickness(2),
                        Width = 300,
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        RequestedTheme = ElementTheme.Light,
                        BorderThickness = new Thickness(1),
                        Date = task.NgayThucHien // Hiển thị ngày từ Task
                    };
                    element = datePicker;
                }
                else if (col == 2 || col == 7 || col == 9 || col == 11) // AutoSuggestBox cho các trường đặc biệt
                {
                    var autoSuggestBox = new AutoSuggestBox
                    {
                        Margin = new Thickness(2),
                        RequestedTheme = ElementTheme.Light,
                        PlaceholderText = GetPlaceholderTextForColumn(col), // Gán placeholder phù hợp
                        Width = 300,
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        Text = task.GetPropertyForColumn(col) // Lấy giá trị hiển thị từ Task
                    };
                    autoSuggestBox.TextChanged += AutoSuggestBox_TextChanged;
                    autoSuggestBox.SuggestionChosen += AutoSuggestBox_SuggestionChosen;
                    element = autoSuggestBox;
                }
                else // TextBox
                {
                    int maxLength = col == 3 ? 10 : 100; // Giới hạn ký tự (VD: Cột 3 là 10 ký tự)

                    var textBox = new TextBox
                    {
                        Margin = new Thickness(2),
                        PlaceholderText = PlaceHolderText[col],
                        Background = new SolidColorBrush(Colors.White),
                        Foreground = new SolidColorBrush(Colors.Black),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        MaxLength = maxLength, // Áp dụng giới hạn ký tự
                        RequestedTheme = ElementTheme.Light,
                        Text = task.GetPropertyForColumn(col) // Lấy giá trị từ Task
                    };
                    element = textBox;
                }

                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, rowIndex);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
        }

        private string GetPlaceholderTextForColumn(int column)
        {
            return column switch
            {
                2 => "Nhập họ tên khách hàng",
                7 => "Nhập mã linh kiện",
                9 => "Nhập mã lõi",
                11 => "Nhập mã nhân viên",
                _ => $"Nhập dữ liệu cột {column}"
            };
        }


        private void SaveUpdateRowData(int rowIndex)
        {
            try
            {
                if (rowUpdateTaskDictionary.TryGetValue(rowIndex, out Task service))
                {
                    for (int col = 1; col < Columns; col++)
                    {
                        var element = InputGrid.Children
                            .OfType<FrameworkElement>()
                            .FirstOrDefault(e => Grid.GetRow(e) == rowIndex && Grid.GetColumn(e) == col);

                        if (element is AutoSuggestBox autoSuggestBox)
                        {
                            if (autoSuggestBox.PlaceholderText.Contains("họ tên khách hàng"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.HoTenKH = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Họ tên khách hàng không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã nhân viên"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaNV = autoSuggestBox.Text;
                                }
                                else
                                {
                                    service.MaNV = "unkhow";
                                    Console.WriteLine("Mã nhân viên không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã linh kiện"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaLK = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Mã linh kiện không được để trống.");
                                }
                            }
                            else if (autoSuggestBox.PlaceholderText.Contains("mã lõi"))
                            {
                                if (!string.IsNullOrWhiteSpace(autoSuggestBox.Text))
                                {
                                    service.MaLoi = autoSuggestBox.Text;
                                }
                                else
                                {
                                    Console.WriteLine("Mã lõi không được để trống.");
                                }
                            }
                        }
                        else if (element is TextBox textBox)
                        {
                            service.SetPropertyForColumn(col, textBox.Text);
                        }
                        else if (element is DatePicker datePicker)
                        {
                            if (datePicker.Date != null) // Kiểm tra nếu datePicker có dữ liệu
                            {
                                service.SetPropertyForDateColumn(col, datePicker.Date.DateTime);
                            }

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
        private async void UpdateDataClick()
        {
            try
            {
               
                isUpdate = false;
                if ( rowUpdateTaskDictionary.TryGetValue(EditingRowIndex, out var task))
                {

                    SaveUpdateRowData(EditingRowIndex);

                    if (!ViewModel.CheckTaskExists(task))
                    {
                        // Hiển thị thông báo cho người dùng
                        ContentDialog dialog = new ContentDialog
                        {
                            Title = "Công việc không tồn tại",
                            Content = "Dòng công việc này đã bị người khác xóa khỏi cơ sở dữ liệu.",
                            PrimaryButtonText = "Bỏ qua",
                           // SecondaryButtonText = "Bỏ qua",
                            DefaultButton = ContentDialogButton.Primary,
                            XamlRoot = this.XamlRoot // Đảm bảo XamlRoot được sử dụng trong các hàm giao diện
                        };

                        // Xử lý kết quả từ ContentDialog
                        var result = await dialog.ShowAsync();

                        if (result == ContentDialogResult.Primary)
                        {
                            // Nếu người dùng chọn "Thêm lại", gọi hàm thêm mới task
                              //ViewModel.InsertTaskToDaTaBaseTemp(task);
                           
                              //  // Thông báo thành công
                              //  ContentDialog successDialog = new ContentDialog
                              //  {
                              //      Title = "Thành công",
                              //      Content = "Công việc đã được thêm vào tạm thời thành công\nNếu muốn lưu lại hãy nhấn lưu dự án!",
                              //      CloseButtonText = "Đóng",
                              //      XamlRoot = this.XamlRoot
                              //  };
                              //  await successDialog.ShowAsync();
                           
                        }
                        //else if (result == ContentDialogResult.Secondary)
                        //{
                        //    // Nếu người dùng chọn "Bỏ qua", không làm gì thêm
                        //}
                    }

                    else
                    {
                        ViewModel.UpdateSelectedTask(task);
                        ViewModel.RefreshData();

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
                        ViewModel.RefreshData();
                    }
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
        //private async void OnExportToExcelClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var savePicker = new FileSavePicker();
        //        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        //        savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
        //        savePicker.SuggestedFileName = "DichVu_" + DateTime.Now.ToString("yyyyMMdd");

        //        StorageFile file = await savePicker.PickSaveFileAsync();
        //        if (file != null)
        //        {
        //            await ExcelExporter.ExportToExcel(ViewModel.Tasks, file.Path);

        //            ContentDialog dialog = new ContentDialog()
        //            {
        //                Title = "Thông báo",
        //                Content = "Xuất file Excel thành công!",
        //                CloseButtonText = "OK"
        //            };
        //            await dialog.ShowAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ContentDialog dialog = new ContentDialog()
        //        {
        //            Title = "Lỗi",
        //            Content = "Có lỗi xảy ra: " + ex.Message,
        //            CloseButtonText = "OK"
        //        };

        //        if (XamlRoot != null)
        //        {
        //            dialog.XamlRoot = XamlRoot;
        //        }
        //        await dialog.ShowAsync();
        //    }
        //}

        private async void OnExportToExcelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create the picker
                var savePicker = new FileSavePicker();

                // Get the current window handle
                IntPtr hwnd = WindowNative.GetWindowHandle(App.MainShellWindow);

                // Initialize the picker with the window handle
                InitializeWithWindow.Initialize(savePicker, hwnd);

                // Configure the picker
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
                savePicker.SuggestedFileName = "DichVu_" + DateTime.Now.ToString("yyyyMMdd");

                // Show the picker and handle the result
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    await ExcelExporter.ExportToExcel(ViewModel.Tasks, file.Path);

                    var dialog = new ContentDialog()
                    {
                        Title = "Thông báo",
                        Content = "Xuất file Excel thành công!",
                        CloseButtonText = "OK",
                        XamlRoot = XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var dialog = new ContentDialog()
                {
                    Title = "Lỗi",
                    Content = "Có lỗi xảy ra: " + ex.Message,
                    CloseButtonText = "OK",
                    XamlRoot = XamlRoot
                };
                await dialog.ShowAsync();
            }
        }


    }
}


