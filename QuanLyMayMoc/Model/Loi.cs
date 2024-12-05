using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class Loisp : INotifyPropertyChanged
    {
        private string _masp;
        private string _tensp;
        private double _giaban;


        public string MaSanPham
        {
            get => _masp;
            set { _masp = value; OnPropertyChanged(nameof(MaSanPham)); }
        }

        public string TenSanPham
        {
            get => _tensp;
            set { _tensp = value; OnPropertyChanged(nameof(TenSanPham)); }
        }
        public double GiaBan
        {
            get => _giaban;
            set { _giaban = value; OnPropertyChanged(nameof(GiaBan)); }
        }

        public string GetPropertyForColumn(int column)
        {
            switch (column)
            {
                case 0:
                    return MaSanPham;
                case 1:
                    return TenSanPham;
                case 2:
                    return GiaBan.ToString("F2"); // Chuyển đổi double sang chuỗi với 2 chữ số thập phân
                default:
                    return string.Empty;
            }
        }


        public void SetPropertyForColumn(int col, string value)
        {
            switch (col)
            {
                case 0:
                    MaSanPham = value;
                    break;
                case 1:
                    TenSanPham = value;
                    break;
                case 2:
                    if (double.TryParse(value, out double giaBan))
                    {
                        GiaBan = giaBan;
                    }
                    else
                    {
                        throw new ArgumentException($"Invalid value for GiaBan: {value}");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(col), "Invalid column index.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
