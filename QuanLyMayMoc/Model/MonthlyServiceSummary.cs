using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{


    public class MonthlyServiceSummary : INotifyPropertyChanged
    {
        private string _code;
        private int _month;
        private string _employeeCode;
        private string _employeeName;
        private int _stt;
        private double _serviceFee;
        private double _totalServiceFee;
        private double _monthlyTotalFee;


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

        public string EmployeeCode
        {
            get => _employeeCode;
            set
            {
                _employeeCode = value;
                OnPropertyChanged(nameof(EmployeeCode));
            }
        }

        public string EmployeeName
        {
            get => _employeeName;
            set
            {
                _employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        public int STT
        {
            get => _stt;
            set
            {
                _stt = value;
                OnPropertyChanged(nameof(STT));
            }
        }

        public double ServiceFee
        {
            get => _serviceFee;
            set
            {
                _serviceFee = value;
                OnPropertyChanged(nameof(ServiceFee));
            }
        }

        public double TotalServiceFee
        {
            get => _totalServiceFee;
            set
            {
                _totalServiceFee = value;
                OnPropertyChanged(nameof(TotalServiceFee));
            }
        }

        public double MonthlyTotalFee
        {
            get => _monthlyTotalFee;
            set
            {
                _monthlyTotalFee = value;
                OnPropertyChanged(nameof(MonthlyTotalFee));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
