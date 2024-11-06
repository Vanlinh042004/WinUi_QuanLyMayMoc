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
        private int selectedRow = -1; // Hàng được chọn để xóa


        public LinhKien()
        {
            this.InitializeComponent();
            HideFirstRow(); // Thêm dòng đầu tiên
            //string m_tempPath = "E:\\Windows\\WinUi_QuanLyMayMoc\\Template\\DuAnMau\\db_QuanLyMayMoc.sqlite3";
            //C: \Users\Nhat\Desktop\temp\DataBase
            //string m_tempPath = "C:\\Users\\Nhat\\Desktop\\temp\\DataBase\\db_QuanLyMayMoc.sqlite3";

            // làm sao link đường dẫn tương đối đến folder Database trong solution
            //string m_tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "QuanLyMayMoc", "Database", "db_QuanLyMayMoc.sqlite3");

            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.Parent.Parent.FullName;
            string databasePath = Path.Combine(projectRoot, "Database", "db_QuanLyMayMoc.sqlite3");

            string m_tempPath = databasePath;


            DataProvider.InstanceTHDA.changePath(m_tempPath);
            string dbString = $"SELECT GM.MaHieu, GM.Ten, GM.Gia " +
                              $" FROM Tbl_MTC_ChiTietGiaMay GM";
            DataTable InforMay = DataProvider.InstanceTHDA.ExecuteQuery(dbString);
            PopulateGrid(InforMay);
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

        private void PopulateGrid(DataTable dataTable)
        {
            // Xóa các hàng hiện có (trừ hàng đầu tiên ẩn)
            for (int i = InputGrid.Children.Count - 1; i >= 0; i--)
            {
                var element = InputGrid.Children[i];
                if (Grid.GetRow((FrameworkElement)element) > 0)
                {
                    InputGrid.Children.RemoveAt(i);
                }
            }

            // Xóa các định nghĩa hàng hiện có (trừ hàng đầu tiên ẩn)
            for (int row = InputGrid.RowDefinitions.Count - 1; row > 0; row--)
            {
                InputGrid.RowDefinitions.RemoveAt(row);
            }

            // Thêm các hàng mới dựa trên DataTable
            int rowIndex = 1; // Bắt đầu từ 1 để bỏ qua hàng ẩn
            foreach (DataRow row in dataTable.Rows)
            {
                InputGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    var border = new Border
                    {
                        Background = new SolidColorBrush(Colors.White),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        BorderThickness = new Thickness(1),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    var textBlock = new TextBlock
                    {
                        Text = row[col].ToString(),
                        Margin = new Thickness(2),
                        Foreground = new SolidColorBrush(Colors.Black),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    border.Child = textBlock;

                    Grid.SetRow(border, rowIndex);
                    Grid.SetColumn(border, col);
                    InputGrid.Children.Add(border);
                }
                rowIndex++;
            }
            currentRow = rowIndex; // Đặt lại số hàng hiện tại
        }

        // Thêm dòng mới
        #region AddNewRow
        private void AddNewRow()
        {

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
                textBox.LostFocus += TextBox_LostFocus;
                element = textBox;
                // Đặt phần tử vào đúng vị trí trong Grid
                Grid.SetRow(element, currentRow);
                Grid.SetColumn(element, col);
                InputGrid.Children.Add(element);
            }
            currentRow++; // Tăng số hàng hiện tại
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                // Lấy hàng và cột của TextBox
                int row = Grid.GetRow(textBox);
                int col = Grid.GetColumn(textBox);

                // Lấy giá trị người dùng nhập
                string value = textBox.Text;

                // Lưu giá trị vào cơ sở dữ liệu
                SaveToDatabase(value);
            }
        }

        private void SaveToDatabase(string value)
        {
            string query = $"UPDATE Tbl_MTC_ChiTietGiaMay" +
                           $"SET Ten = 'Ha Loi'" +
                           $"WHERE MaHieu = @MaHieu";
            DataProvider.InstanceTHDA.ExecuteNonQuery(query, parameter: new object[] {value});
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

        // Add event handlers for TextBox hover effects



        // Delete row data

        private async void OnDeleteRowDataClick(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại để người dùng nhập mã sản phẩm cần xóa
            var inputTextBox = new TextBox();
            var dialog = new ContentDialog
            {
                Title = "Xác nhận xóa",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock { Text = "Nhập Mã sản phẩm của linh kiện cần xóa:" },
                inputTextBox
            }
                },
                PrimaryButtonText = "Xóa",
                CloseButtonText = "Hủy",
                XamlRoot = this.Content.XamlRoot // Đảm bảo ContentDialog được hiển thị trong ngữ cảnh của cửa sổ hiện tại
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string maSanPham = inputTextBox.Text;
                // Tìm selectedRow dựa trên mã sản phẩm
                for (int i = 1; i < InputGrid.RowDefinitions.Count; i++)
                {
                    // Truy cập phần tử đầu tiên trong hàng thứ i (giả định mã sản phẩm nằm ở cột đầu tiên)
                    var element = InputGrid.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow((FrameworkElement)e) == i && Grid.GetColumn((FrameworkElement)e) == 0);

                    if (element is Border border && border.Child is TextBlock textBlock)
                    {
                       
                        if (textBlock.Text == maSanPham)
                        {
                            selectedRow = i;
                            break;
                        }
                    }
                }

                // Xóa selectedRow nếu đã tìm thấy
                if (selectedRow != -1)
                {
                    // Xóa các phần tử trong selectedRow
                    for (int col = 0; col < Columns; col++)
                    {
                        var element = InputGrid.Children
                            .Cast<UIElement>()
                            .FirstOrDefault(e => Grid.GetRow((FrameworkElement)e) == selectedRow && Grid.GetColumn((FrameworkElement)e) == col);

                        if (element != null)
                        {
                            InputGrid.Children.Remove(element);
                        }
                    }

                    // Xóa định nghĩa hàng của selectedRow
                    InputGrid.RowDefinitions.RemoveAt(selectedRow);

                    // Đặt lại số hàng hiện tại
                    currentRow--;
                    // Xoa database
                    DeleteRowFromDatabase(maSanPham);
                }
            }
        }

        private void DeleteRowFromDatabase(string maSanPham)
        {
            string query = $"DELETE" +
                             $" FROM Tbl_MTC_ChiTietGiaMay " +
                             $" WHERE MaHieu = @MaHieu";
            DataProvider.InstanceTHDA.ExecuteNonQuery(query, parameter: new object[] {maSanPham});
        }    






        // Delete all row
        private void OnDeleteAllRowDataClick(object sender, RoutedEventArgs e)
        {
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
            // Xoa database
            DeleteAllRowFromDatabase();
        }
        private void DeleteAllRowFromDatabase()
        {
            string query = $"DELETE" +
                           $" FROM Tbl_MTC_ChiTietGiaMay ";
            DataProvider.InstanceTHDA.ExecuteNonQuery(query);
        }

    }
    public class ServiceData_LinhKien
    {
        public string MASP { get; set; }
        public string TENSP { get; set; }
        public string GIA { get; set; }
    }
}
