using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class MockDao : IDao
    {
        private ObservableCollection<Task> services = new ObservableCollection<Task>()
{
    //new Task {
    //    Stt = 1,
    //    NgayThucHien = new DateTime(2024, 11, 10),
    //    MaCV = "DV001",
    //    HoTenKH = "Trần Văn An",
    //    SDT = "0912345678",
    //    DiaChi = "123 Đường ABC",
    //    TenDichVu = "Bảo trì định kỳ",
    //    MaNV = "NV001",
    //    PhiDichVu = 500000,
    //    MaLK = "LK001",
    //    SoLuongLK = 2,
    //    MaLoi = "LOI001",
    //    SoLuongLoi = 1,
    //    GhiChu = "Kiểm tra và bảo trì",
    //},
    //new Task {
    //    Stt = 2,
    //    NgayThucHien = new DateTime(2022, 3, 1),
    //    MaCV = "DV002",
    //    HoTenKH = "Nguyễn Thị Bích",
    //    SDT = "0987654321",
    //    DiaChi = "456 Đường XYZ",
    //    TenDichVu = "Thay linh kiện",
    //    MaNV = "NV002",
    //    PhiDichVu = 700000,
    //    MaLK = "LK002",
    //    SoLuongLK = 1,
    //    MaLoi = "LOI002",
    //    SoLuongLoi = 1,
    //    GhiChu = "Thay linh kiện bộ lọc",
    //},
    //new Task
    //{
    //    Stt = 3,
    //    NgayThucHien = new DateTime(2022, 6, 1),
    //    MaCV = "DV003",
    //    HoTenKH = "Lê Quốc Cường",
    //    SDT = "0934567890",
    //    DiaChi = "789 Đường DEF",
    //    TenDichVu = "Kiểm tra bảo hành",
    //    MaNV = "NV003",
    //    PhiDichVu = 0, // Miễn phí
    //    MaLK = "LK003",
    //    SoLuongLK = 0,
    //    MaLoi = "LOI003",
    //    SoLuongLoi = 0,
    //    GhiChu = "Kiểm tra bảo hành",
    //},
    //new Task
    //{
    //    Stt = 4,
    //    NgayThucHien = new DateTime(2022, 7, 1),
    //    MaCV = "DV004",
    //    HoTenKH = "Phạm Văn Dũng",
    //    SDT = "0945678901",
    //    DiaChi = "321 Đường GHI",
    //    TenDichVu = "Sửa chữa",
    //    MaNV = "NV004",
    //    PhiDichVu = 800000,
    //    MaLK = "LK004",
    //    SoLuongLK = 1,
    //    MaLoi = "LOI004",
    //    SoLuongLoi = 2,
    //    GhiChu = "Sửa chữa bo mạch",
    //},
};


        public ObservableCollection<Employee> GetEmployees()
        {
            var result = new ObservableCollection<Employee>()
            {
                //new Employee {
                //    HoTen = "Võ Văn Lĩnh",
                //    MaNhanVien = "NV001",
                //    GioiTinh="Nam",
                //    NgayKyHD = new DateTime(2021, 5, 1),
                //    NgaySinh = new DateTime(1990, 1, 1), // Ngày sinh
                //    TrangThai = "Đang làm việc",
                //    AnhDaiDien = "../Assets/linh.jpg",
                //    DanToc = "Kinh", // Dân tộc
                //    CCCD = "123456789012", // Số CCCD
                //    PhongBan = "Phòng nhân sự",
                //    Email = "a.nguyen@example.com",
                //    SoDienThoai = "0901234567",
                //    DiaChi = "123 Đường ABC",
                //},
                //    new Employee {
                //    HoTen = "Huỳnh Mẫn",
                //    MaNhanVien = "NV002",
                //     GioiTinh="Nam",
                //    NgayKyHD = new DateTime(2020, 8, 15),
                //    NgaySinh = new DateTime(1985, 5, 20), // Ngày sinh
                //    TrangThai = "Đang làm việc",
                //    AnhDaiDien = "../Assets/man.jpg",
                //    DanToc = "Kinh", // Dân tộc
                //    //SoNguoiPhuThuoc = "1", // Số người phụ thuộc
                //    CCCD = "987654321098", // Số CCCD
                //    PhongBan = "Phòng kinh doanh",
                //    Email = "b.tran@example.com",
                //    SoDienThoai = "0987654321",
                //    DiaChi = "456 Đường XYZ",
                //   // NhanVienVanPhongCongTruong = "Văn phòng",
                //},

                //    new Employee {
                //    HoTen = "Giang Đức Nhật",
                //    MaNhanVien = "NV003",
                //   // ChucVu = "Nhân viên",
                //    GioiTinh="Nam",
                //    NgayKyHD = new DateTime(2022, 3, 10),
                //    NgaySinh = new DateTime(1992, 3, 15), // Ngày sinh
                //  //  MaSoThue = "654321987",
                //    TrangThai = "Đang làm việc",
                //    AnhDaiDien = "../Assets/ducnhat.jpg",
                //    DanToc = "Kinh", // Dân tộc
                //    //SoNguoiPhuThuoc = "2", // Số người phụ thuộc
                //    CCCD = "456789123456", // Số CCCD
                //    PhongBan = "Phòng kỹ thuật",
                //    Email = "c.le@example.com",
                //    SoDienThoai = "0912345678",
                //    DiaChi = "789 Đường DEF",
                //   // NhanVienVanPhongCongTruong = "Văn phòng",
                //},

                //    new Employee {
                //    HoTen = "Nguyễn Xuân Hạnh",
                //    MaNhanVien = "NV004",
                //   // ChucVu = "Giám đốc",
                //    GioiTinh="Nữ",
                //    NgayKyHD = new DateTime(2019, 1, 20),
                //    NgaySinh = new DateTime(1980, 8, 30), // Ngày sinh
                //  //  MaSoThue = "321654987",
                //    TrangThai = "Đang làm việc",
                //    AnhDaiDien = "../Assets/hanh.jpg",
                //    DanToc = "Kinh", // Dân tộc
                //   // SoNguoiPhuThuoc = "3", // Số người phụ thuộc
                //    CCCD = "789123456789", // Số CCCD
                //    PhongBan = "Ban giám đốc",
                //    Email = "d.hoang@example.com",
                //    SoDienThoai = "0923456789",
                //    DiaChi = "321 Đường GHI",
                //   // NhanVienVanPhongCongTruong = "Văn phòng",
                //    },
            };

            return result;
        }
        public ObservableCollection<Task> GetTasks()
        {
            ObservableCollection<Task> result = new ObservableCollection<Task>();
            foreach (var item in services)
            {

                result.Add(item);

            }
            return result;
        }


        public ObservableCollection<Task> GetTasks(DateTime date)
        {
            ObservableCollection<Task> result = new ObservableCollection<Task>();
            foreach (var item in services)
            {

                if (item.NgayThucHien.Date == date.Date)
                {
                    result.Add(item);
                }
            }
            return result;
        }





        public MockDao()
        {

        }
        public void AddTask(Task newTask)
        {
            // Thêm service mới vào danh sách dịch vụ
            services.Add(newTask);
        }

        public ObservableCollection<Project> GetProjects()
        {
            
            var result= new ObservableCollection<Project>()
            {
                new Project{
                ID="a",
                Name="Man",
                TimeCreate=new DateTime(2024,11,17),
                }
            };
            return result;
        }





    }
}
