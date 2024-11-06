using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc.ViewModel
{
    public class MockDao : IDao
    {
        private ObservableCollection<Service> services = new ObservableCollection<Service>()
    {
        new Service {
            Stt = 1,
            Ngay = new DateTime(2022, 5, 1),
            MaSoCV = "DV001",
            HoTenKH = "Trần Văn An",
            SDT = "0912345678",
            DiaChi = "123 Đường ABC",
            LoaiModel = "Điều hòa",
            Model = "Daikin XYZ123",
            NgayLapDat = new DateTime(2022, 5, 1),
            TenDichVu = "Bảo trì định kỳ",
            PhiDichVu = "500,000 VND",
            KTVThucHien = "Nguyễn Văn A",
            MaLK = "LK001",
            TenLK = "Linh kiện ABC",
            GhiChu = "Kiểm tra và bảo trì",
        },
        new Service {
            Stt = 2,
            Ngay = new DateTime(2022, 3, 1),
            MaSoCV = "DV002",
            HoTenKH = "Nguyễn Thị Bích",
            SDT = "0987654321",
            DiaChi = "456 Đường XYZ",
            LoaiModel = "Máy giặt",
            Model = "LG ABC456",
            NgayLapDat = new DateTime(2023, 2, 15),
            TenDichVu = "Thay linh kiện",
            PhiDichVu = "700,000 VND",
            KTVThucHien = "Lê Văn B",
            MaLK = "LK002",
            TenLK = "Linh kiện DEF",
            GhiChu = "Thay linh kiện bộ lọc",
        },
        new Service
        {
            Stt = 3,
            Ngay = new DateTime(2022, 6, 1),
            MaSoCV = "DV003",
            HoTenKH = "Lê Quốc Cường",
            SDT = "0934567890",
            DiaChi = "789 Đường DEF",
            LoaiModel = "Tủ lạnh",
            Model = "Samsung DEF789",
            NgayLapDat = new DateTime(2021, 8, 20),
            TenDichVu = "Kiểm tra bảo hành",
            PhiDichVu = "Miễn phí",
            KTVThucHien = "Phạm Thị C",
            MaLK = "LK003",
            TenLK = "Linh kiện GHI",
            GhiChu = "Kiểm tra bảo hành",
        },
        new Service
        {
            Stt = 4,
            Ngay = new DateTime(2022, 7, 1),
            MaSoCV = "DV004",
            HoTenKH = "Phạm Văn Dũng",
            SDT = "0945678901",
            DiaChi = "321 Đường GHI",
            LoaiModel = "Máy nước nóng",
            Model = "Panasonic GHI101",
            NgayLapDat = new DateTime(2020, 11, 10),
            TenDichVu = "Sửa chữa",
            PhiDichVu = "800,000 VND",
            KTVThucHien = "Trần Văn D",
            MaLK = "LK004",
            TenLK = "Linh kiện XYZ",
            GhiChu = "Sửa chữa bo mạch",
        },
    };

        public ObservableCollection<Employee> GetEmployees()
        {
            var result = new ObservableCollection<Employee>()
            {
                new Employee {
                    HoTen = "Võ Văn Lĩnh",
                    MaNhanVien = "NV001",
                    ChucVu = "Nhân viên",
                    GioiTinh="Nam",
                    NgayKyHD = new DateTime(2021, 5, 1),
                    NgaySinh = new DateTime(1990, 1, 1), // Ngày sinh
                    MaSoThue = "123456789",
                    TrangThai = "Đang làm việc",
                    AnhDaiDien = "../Assets/linh.jpg",
                    DanToc = "Kinh", // Dân tộc
                    SoNguoiPhuThuoc = "0", // Số người phụ thuộc
                    CCCD = "123456789012", // Số CCCD
                    PhongBan = "Phòng nhân sự",
                    Email = "a.nguyen@example.com",
                    SoDienThoai = "0901234567",
                    DiaChi = "123 Đường ABC",
                    NhanVienVanPhongCongTruong = "Văn phòng",
                },
                    new Employee {
                    HoTen = "Huỳnh Mẫn",
                    MaNhanVien = "NV002",
                    ChucVu = "Trưởng phòng",
                     GioiTinh="Nam",
                    NgayKyHD = new DateTime(2020, 8, 15),
                    NgaySinh = new DateTime(1985, 5, 20), // Ngày sinh
                    MaSoThue = "987654321",
                    TrangThai = "Đang làm việc",
                    AnhDaiDien = "../Assets/man.jpg",
                    DanToc = "Kinh", // Dân tộc
                    SoNguoiPhuThuoc = "1", // Số người phụ thuộc
                    CCCD = "987654321098", // Số CCCD
                    PhongBan = "Phòng kinh doanh",
                    Email = "b.tran@example.com",
                    SoDienThoai = "0987654321",
                    DiaChi = "456 Đường XYZ",
                    NhanVienVanPhongCongTruong = "Văn phòng",
                },

                    new Employee {
                    HoTen = "Giang Đức Nhật",
                    MaNhanVien = "NV003",
                    ChucVu = "Nhân viên",
                    GioiTinh="Nam",
                    NgayKyHD = new DateTime(2022, 3, 10),
                    NgaySinh = new DateTime(1992, 3, 15), // Ngày sinh
                    MaSoThue = "654321987",
                    TrangThai = "Đang làm việc",
                    AnhDaiDien = "../Assets/ducnhat.jpg",
                    DanToc = "Kinh", // Dân tộc
                    SoNguoiPhuThuoc = "2", // Số người phụ thuộc
                    CCCD = "456789123456", // Số CCCD
                    PhongBan = "Phòng kỹ thuật",
                    Email = "c.le@example.com",
                    SoDienThoai = "0912345678",
                    DiaChi = "789 Đường DEF",
                    NhanVienVanPhongCongTruong = "Văn phòng",
                },

                    new Employee {
                    HoTen = "Nguyễn Xuân Hạnh",
                    MaNhanVien = "NV004",
                    ChucVu = "Giám đốc",
                    GioiTinh="Nữ",
                    NgayKyHD = new DateTime(2019, 1, 20),
                    NgaySinh = new DateTime(1980, 8, 30), // Ngày sinh
                    MaSoThue = "321654987",
                    TrangThai = "Đang làm việc",
                    AnhDaiDien = "../Assets/hanh.jpg",
                    DanToc = "Kinh", // Dân tộc
                    SoNguoiPhuThuoc = "3", // Số người phụ thuộc
                    CCCD = "789123456789", // Số CCCD
                    PhongBan = "Ban giám đốc",
                    Email = "d.hoang@example.com",
                    SoDienThoai = "0923456789",
                    DiaChi = "321 Đường GHI",
                    NhanVienVanPhongCongTruong = "Văn phòng",
                    },
            };

            return result;
        }
        public ObservableCollection<Service> GetServices()
        {
            var result = services;
            return result;

        }
        public ObservableCollection<Service> GetServices(DateTime date)
        {
            var allServices = new ObservableCollection<Service>()
    {
        new Service {
            Stt = 1,
            Ngay = new DateTime(2022, 5, 1),
            MaSoCV = "DV001",
            HoTenKH = "Trần Văn An",
            SDT = "0912345678",
            DiaChi = "123 Đường ABC",
            LoaiModel = "Điều hòa",
            Model = "Daikin XYZ123",
            NgayLapDat = new DateTime(2022, 5, 1),
            TenDichVu = "Bảo trì định kỳ",
            PhiDichVu = "500,000 VND",
            KTVThucHien = "Nguyễn Văn A",
            MaLK = "LK001",
            TenLK = "Linh kiện ABC",
            GhiChu = "Kiểm tra và bảo trì",
        },
        new Service {
            Stt = 2,
            Ngay = new DateTime(2022, 3, 1),
            MaSoCV = "DV002",
            HoTenKH = "Nguyễn Thị Bích",
            SDT = "0987654321",
            DiaChi = "456 Đường XYZ",
            LoaiModel = "Máy giặt",
            Model = "LG ABC456",
            NgayLapDat = new DateTime(2023, 2, 15),
            TenDichVu = "Thay linh kiện",
            PhiDichVu = "700,000 VND",
            KTVThucHien = "Lê Văn B",
            MaLK = "LK002",
            TenLK = "Linh kiện DEF",
            GhiChu = "Thay linh kiện bộ lọc",
        },
        new Service {
            Stt = 3,
            Ngay = new DateTime(2022, 6, 1),
            MaSoCV = "DV003",
            HoTenKH = "Lê Quốc Cường",
            SDT = "0934567890",
            DiaChi = "789 Đường DEF",
            LoaiModel = "Tủ lạnh",
            Model = "Samsung DEF789",
            NgayLapDat = new DateTime(2021, 8, 20),
            TenDichVu = "Kiểm tra bảo hành",
            PhiDichVu = "Miễn phí",
            KTVThucHien = "Phạm Thị C",
            MaLK = "LK003",
            TenLK = "Linh kiện GHI",
            GhiChu = "Kiểm tra bảo hành",
        },
        new Service {
            Stt = 4,
            Ngay = new DateTime(2022, 7, 1),
            MaSoCV = "DV004",
            HoTenKH = "Phạm Văn Dũng",
            SDT = "0945678901",
            DiaChi = "321 Đường GHI",
            LoaiModel = "Máy nước nóng",
            Model = "Panasonic GHI101",
            NgayLapDat = new DateTime(2020, 11, 10),
            TenDichVu = "Sửa chữa",
            PhiDichVu = "800,000 VND",
            KTVThucHien = "Trần Văn D",
            MaLK = "LK004",
            TenLK = "Linh kiện XYZ",
            GhiChu = "Sửa chữa bo mạch",
        },
    };
            if (date == DateTime.MinValue)
            {
                return allServices;
            }

            else
            {
                var query = from service in allServices
                            where service.Ngay.Date == date.Date
                            select service;


                return new ObservableCollection<Service>(query);
            }
        }


        public MockDao()
        {

        }







    }
}
