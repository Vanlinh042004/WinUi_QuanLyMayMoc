using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc.Model
{
    public class Employee : INotifyPropertyChanged
    {
        private string _maNhanVienDuAn;
      
        private string _maNhanVien;
        private string _hoTen;
        private string _danToc;
        private string _gioiTinh;
        private DateTime _ngaySinh;
        private string _diaChi;
        private string _soDienThoai;
        private string _email;
        private string _phongBan;
        private string _CCCD;
        private string _trangThai;
      

        private DateTime _ngayKyHD;
       
      
      
        private string _anhDaiDien; // Path to the image
       
     

        // Contact Information
      
       
      
       
        private string _maDuAn;
        

        public string HoTen
        {
            get => _hoTen;
            set { _hoTen = value; OnPropertyChanged(nameof(HoTen)); }
        }
        public string MaDuAn
        {
            get => _maDuAn;
            set { _maDuAn = value; OnPropertyChanged(nameof(MaDuAn)); }
        }
        public string MaNhanVienDuAn
        {
            get => _maNhanVienDuAn;
            set { _maNhanVienDuAn = value; OnPropertyChanged(nameof(MaNhanVienDuAn)); }
        }

        public string MaNhanVien
        {
            get => _maNhanVien;
            set { _maNhanVien = value; OnPropertyChanged(nameof(MaNhanVien)); }
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
