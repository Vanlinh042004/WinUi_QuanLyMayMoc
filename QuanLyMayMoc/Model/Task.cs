using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuanLyMayMoc
{
    public class Task : INotifyPropertyChanged
    {
        private string _maCVDuAn;

        private int _stt;

        private DateTime _ngayThucHien;

        private string _hoTenKH;

        private string _sdt;

        private string _diaChi;

        private string _tenDichVu;

        private string _maNV;

        private string _tenNV;

        private int _phiDichVu;

        private string _maLK;

        private string _tenLK;

        private int _soLuongLK;

        private string _maLoi;

        private string _tenLoi;

        private int _soLuongLoi;

        private string _ghiChu;

        private string _maDuAn;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int Stt
        {
            get => _stt;
            set { _stt = value; OnPropertyChanged(nameof(Stt)); }
        }
        
        public DateTime NgayThucHien
        {
            get => _ngayThucHien;
            set { _ngayThucHien = value; OnPropertyChanged(nameof(NgayThucHien)); }
        }

        public string MaCVDuAn
        {
            get => _maCVDuAn;
            set { _maCVDuAn = value; OnPropertyChanged(nameof(MaCVDuAn)); }
        }

        public string HoTenKH
        {
            get => _hoTenKH;
            set { _hoTenKH = value; OnPropertyChanged(nameof(HoTenKH)); }
        }

        public string SDT
        {
            get => _sdt;
            set { _sdt = value; OnPropertyChanged(nameof(SDT)); }
        }

        public string DiaChi
        {
            get => _diaChi;
            set { _diaChi = value; OnPropertyChanged(nameof(DiaChi)); }
        }

        public string MaNV
        {
            get => _maNV;
            set { _maNV = value; OnPropertyChanged(nameof(MaNV)); }
        }
        public string TenNV
        {
            get => _tenNV;
            set { _tenNV = value; OnPropertyChanged(nameof(TenNV)); }
        }

        [JsonProperty("MaLinhKien")]
        public string MaLK
        {
            get => _maLK;
            set { _maLK = value; OnPropertyChanged(nameof(MaLK)); }
        }

        [JsonProperty("TenLinhKien")]
        public string TenLK
        {
            get => _tenLK;
            set { _tenLK = value; OnPropertyChanged(nameof(TenLK)); }
        }

        [JsonProperty("SoLuongLinhKien")]
        public int SoLuongLK
        {
            get => _soLuongLK;
            set { _soLuongLK = value; OnPropertyChanged(nameof(SoLuongLK)); }
        }
        public string MaLoi
        {
            get => _maLoi;
            set { _maLoi = value; OnPropertyChanged(nameof(MaLoi)); }
        }
        public string TenLoi
        {
            get => _tenLoi;
            set { _tenLoi = value; OnPropertyChanged(nameof(TenLoi)); }
        }
        public int SoLuongLoi
        {
            get => _soLuongLoi;
            set { _soLuongLoi = value; OnPropertyChanged(nameof(SoLuongLoi)); }
        }


        [JsonProperty("TenDichVu")]
        public string TenDichVu
        {
            get => _tenDichVu;
            set { _tenDichVu = value; OnPropertyChanged(nameof(TenDichVu)); }
        }

        public int PhiDichVu
        {
            get => _phiDichVu;
            set { _phiDichVu = value; OnPropertyChanged(nameof(PhiDichVu)); }
        }

        public string GhiChu
        {
            get => _ghiChu;
            set { _ghiChu = value; OnPropertyChanged(nameof(GhiChu)); }
        }
        public string MaDuAn
        {
            get => _maDuAn;
            set { _maDuAn = value; OnPropertyChanged(nameof(MaDuAn)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SetPropertyForColumn(int col, string value)
        {
            switch (col)
            {
                //case 0:
                //    int.TryParse(value, out int stt); // Chuyển đổi value thành int
                //    Stt = stt; // Gán giá trị cho Stt
                //    break;


                case 2:
                    HoTenKH = value; // Họ tên khách hàng
                    break;
                case 3:
                    SDT = value; // Số điện thoại
                    break;
                case 4:
                    DiaChi = value; // Địa chỉ
                    break;
                case 5:
                   TenDichVu = value; // Loại model
                    break;
                case 6:
                    int.TryParse(value, out int phi); // Chuyển đổi value thành int                 
                    PhiDichVu = phi; // Model
                    break;
                case 7:
                    MaLK = value; // Model
                    break;              
               
           
                case 8:
                    TenLK = value; // Model
                    break;
                case 9:
                    int.TryParse(value, out int sllk); // Chuyển đổi value thành int                 
                    SoLuongLK = sllk;
                    break;
                case 10:
                    MaLoi = value; // Kỹ thuật viên thực hiện
                    break;
                case 11:
                    TenLoi = value; // Model
                    break;
                case 12:
                    int.TryParse(value, out int sll); // Chuyển đổi value thành int                              
                    SoLuongLoi = sll; // Mã linh kiện
                    break;
                case 13:
                    MaNV = value; // Tên dịch vụ
                    break;
                case 14:
                    TenNV = value; // Tên dịch vụ
                    break;
                case 15:
                    GhiChu = value; // Ghi chú
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(col), "Invalid column index.");
            }
        }

        // Phương thức để gán giá trị cho cột ngày
        public void SetPropertyForDateColumn(int col, DateTime value)
        {
            switch (col)
            {
                case 1: // Giả định cột 1 là Ngày
                    NgayThucHien = value; // Chuyển đổi thành chuỗi nếu cần
                    break;
               
                default:
                    throw new ArgumentOutOfRangeException(nameof(col), "Invalid date column index.");
            }
        }
    }
}
