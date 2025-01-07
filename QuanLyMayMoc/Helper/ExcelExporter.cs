// Thêm NuGet package: Install-Package ClosedXML

using ClosedXML.Excel;
using System.IO;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Drawing;

public class ExcelExporter
{
    public static async System.Threading.Tasks.Task ExportToExcel(ObservableCollection<QuanLyMayMoc.Task> tasks, string filePath)
    {
        await System.Threading.Tasks.Task.Run(() =>
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Dịch vụ");

                // Định dạng header
                var headerStyle = worksheet.Style;
                headerStyle.Font.Bold = true;
                headerStyle.Fill.BackgroundColor = XLColor.LightGray;
                headerStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Thêm headers
                worksheet.Cell(1, 1).Value = "STT";
                worksheet.Cell(1, 2).Value = "Ngày Thực Hiện";
                worksheet.Cell(1, 3).Value = "Họ Tên KH";
                worksheet.Cell(1, 4).Value = "SĐT";
                worksheet.Cell(1, 5).Value = "Địa Chỉ";
                worksheet.Cell(1, 6).Value = "Tên Dịch Vụ";
                worksheet.Cell(1, 7).Value = "Phí Dịch Vụ";
                worksheet.Cell(1, 8).Value = "Mã Linh Kiện";
                worksheet.Cell(1, 9).Value = "Số Lượng Linh Kiện";
                worksheet.Cell(1, 10).Value = "Mã Lõi";
                worksheet.Cell(1, 11).Value = "Số Lượng Lõi";
                worksheet.Cell(1, 12).Value = "Mã Nhân Viên";
                worksheet.Cell(1, 13).Value = "Ghi Chú";

                // Style cho header row
                worksheet.Row(1).Style = headerStyle;

                // Thêm data
                int row = 2;
                foreach (var task in tasks)
                {
                    worksheet.Cell(row, 1).Value = task.Stt;
                    worksheet.Cell(row, 2).Value = task.NgayThucHien;
                    worksheet.Cell(row, 3).Value = task.HoTenKH;
                    worksheet.Cell(row, 4).Value = task.SDT;
                    worksheet.Cell(row, 5).Value = task.DiaChi;
                    worksheet.Cell(row, 6).Value = task.TenDichVu;
                    worksheet.Cell(row, 7).Value = task.PhiDichVu;
                    worksheet.Cell(row, 8).Value = task.MaLK;
                    worksheet.Cell(row, 9).Value = task.SoLuongLK;
                    worksheet.Cell(row, 10).Value = task.MaLoi;
                    worksheet.Cell(row, 11).Value = task.SoLuongLoi;
                    worksheet.Cell(row, 12).Value = task.MaNV;
                    worksheet.Cell(row, 13).Value = task.GhiChu;

                    row++;
                }

                // Định dạng cột ngày tháng
                worksheet.Column(2).Style.DateFormat.Format = "dd/MM/yyyy";

                // Định dạng cột tiền tệ
                worksheet.Column(7).Style.NumberFormat.Format = "#,##0";

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Lưu file
                workbook.SaveAs(filePath);
            }
        });
    }
}