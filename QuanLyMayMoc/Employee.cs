using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class Employee : INotifyPropertyChanged
    {
        private string _hoTen;
        private string _maNhanVien;
        private string _chucVu;
        private string _gioiTinh;
        private DateTime _ngayKyHD;
        private DateTime _ngaySinh;
        private string _maSoThue;
        private string _trangThai;
        private string _anhDaiDien; // Path to the image
        private string _danToc;
        private string _soNguoiPhuThuoc;
        private string _CCCD;

        // Contact Information
        private string _phongBan;
        private string _email;
        private string _soDienThoai;
        private string _diaChi;
        private string _nhanVienVanPhongCongTruong;

        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(nameof(HoTen)); }
        }

        public string MaNhanVien
        {
            get => _maNhanVien;
            set { _maNhanVien = value; OnPropertyChanged(nameof(MaNhanVien)); }
        }

        public string ChucVu
        {
            get => _chucVu;
            set { _chucVu = value; OnPropertyChanged(nameof(ChucVu)); }
        }
        public string GioiTinh
        {
            get => _gioiTinh;
            set { _gioiTinh = value; OnPropertyChanged(nameof(_gioiTinh)); }
        }

        public DateTime NgayKyHD
        {
            get => _ngayKyHD;
            set { _ngayKyHD = value; OnPropertyChanged(nameof(NgayKyHD)); }
        }

        public DateTime NgaySinh
        {
            get => _ngaySinh;
            set { _ngaySinh = value; OnPropertyChanged(nameof(NgaySinh)); }
        }

        public string MaSoThue
        {
            get => _maSoThue;
            set { _maSoThue = value; OnPropertyChanged(nameof(MaSoThue)); }
        }

        public string TrangThai
        {
            get => _trangThai;
            set { _trangThai = value; OnPropertyChanged(nameof(TrangThai)); }
        }

        public string AnhDaiDien
        {
            get => _anhDaiDien;
            set { _anhDaiDien = value; OnPropertyChanged(nameof(AnhDaiDien)); }
        }

        public string DanToc
        {
            get => _danToc;
            set { _danToc = value; OnPropertyChanged(nameof(DanToc)); }
        }

        public string SoNguoiPhuThuoc
        {
            get => _soNguoiPhuThuoc;
            set { _soNguoiPhuThuoc = value; OnPropertyChanged(nameof(SoNguoiPhuThuoc)); }
        }

        public string CCCD
        {
            get => _CCCD;
            set { _CCCD = value; OnPropertyChanged(nameof(CCCD)); }
        }

        // Contact Information
        public string PhongBan
        {
            get => _phongBan;
            set { _phongBan = value; OnPropertyChanged(nameof(PhongBan)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string SoDienThoai
        {
            get => _soDienThoai;
            set { _soDienThoai = value; OnPropertyChanged(nameof(SoDienThoai)); }
        }

        public string DiaChi
        {
            get => _diaChi;
            set { _diaChi = value; OnPropertyChanged(nameof(DiaChi)); }
        }
        
        public string NhanVienVanPhongCongTruong
        {
            get => _nhanVienVanPhongCongTruong;
            set { _nhanVienVanPhongCongTruong = value; OnPropertyChanged(nameof(NhanVienVanPhongCongTruong)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
