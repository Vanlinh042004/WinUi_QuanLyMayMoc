using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class Service : INotifyPropertyChanged
    {
        private int _stt;
        private DateTime _ngay;
        private string _maSoCV;
        private string _hoTenKH;
        private string _sdt;
        private string _diaChi;
        private string _loaiModel;
        private string _model;
        private DateTime _ngayLapDat;
        private string _tenDichVu;
        private string _phiDichVu;
        private string _ktvThucHien;
        private string _maLK;
        private string _tenLK;
        private string _ghiChu;

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

        public DateTime Ngay
        {
            get => _ngay;
            set { _ngay = value; OnPropertyChanged(nameof(Ngay)); }
        }

        public string MaSoCV
        {
            get => _maSoCV;
            set { _maSoCV = value; OnPropertyChanged(nameof(MaSoCV)); }
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

        public string LoaiModel
        {
            get => _loaiModel;
            set { _loaiModel = value; OnPropertyChanged(nameof(LoaiModel)); }
        }

        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(nameof(Model)); }
        }

        public DateTime NgayLapDat
        {
            get => _ngayLapDat;
            set { _ngayLapDat = value; OnPropertyChanged(nameof(NgayLapDat)); }
        }

        public string TenDichVu
        {
            get => _tenDichVu;
            set { _tenDichVu = value; OnPropertyChanged(nameof(TenDichVu)); }
        }

        public string PhiDichVu
        {
            get => _phiDichVu;
            set { _phiDichVu = value; OnPropertyChanged(nameof(PhiDichVu)); }
        }

        public string KTVThucHien
        {
            get => _ktvThucHien;
            set { _ktvThucHien = value; OnPropertyChanged(nameof(KTVThucHien)); }
        }

        public string MaLK
        {
            get => _maLK;
            set { _maLK = value; OnPropertyChanged(nameof(MaLK)); }
        }

        public string TenLK
        {
            get => _tenLK;
            set { _tenLK = value; OnPropertyChanged(nameof(TenLK)); }
        }

        public string GhiChu
        {
            get => _ghiChu;
            set { _ghiChu = value; OnPropertyChanged(nameof(GhiChu)); }
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
                case 0:
                    int.TryParse(value, out int stt); // Chuyển đổi value thành int
                    Stt = stt; // Gán giá trị cho Stt
                    break;

                case 2:
                    MaSoCV = value; // Mã số công việc
                    break;
                case 3:
                    HoTenKH = value; // Họ tên khách hàng
                    break;
                case 4:
                    SDT = value; // Số điện thoại
                    break;
                case 5:
                    DiaChi = value; // Địa chỉ
                    break;
                case 6:
                    LoaiModel = value; // Loại model
                    break;
                case 7:
                    Model = value; // Model
                    break;
                case 9:
                    TenDichVu = value; // Tên dịch vụ
                    break;
                case 10:
                    PhiDichVu = value; // Phí dịch vụ
                    break;
                case 11:
                    KTVThucHien = value; // Kỹ thuật viên thực hiện
                    break;
                case 12:
                    MaLK = value; // Mã linh kiện
                    break;
                case 13:
                    TenLK = value; // Tên linh kiện
                    break;
                case 14:
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
                    Ngay = value; // Chuyển đổi thành chuỗi nếu cần
                    break;
                case 8: // Giả định cột 14 là Ngày lắp đặt
                    NgayLapDat = value; // Gán giá trị cho ngày lắp đặt
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(col), "Invalid date column index.");
            }
        }
    }
}
