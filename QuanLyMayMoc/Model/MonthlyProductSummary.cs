using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    using System.ComponentModel;

    public class MonthlyProductSummary : INotifyPropertyChanged
    {
        private string _code;
        private int _month;
        private int _year; // Thuộc tính năm mới
        private string _productCode;
        private string _productName;
        private int _quantity;
        private double _price;
        private double _totalPrice;
        private double _totalYear;

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        public int Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChanged(nameof(Month));
            }
        }

        public int Year // Thuộc tính năm
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public string ProductCode
        {
            get => _productCode;
            set
            {
                _productCode = value;
                OnPropertyChanged(nameof(ProductCode));
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }

        public double TotalYear
        {
            get => _totalYear;
            set
            {
                _totalYear = value;
                OnPropertyChanged(nameof(TotalYear));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
