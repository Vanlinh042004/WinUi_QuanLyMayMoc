using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public class GroupProductViewModel
    {
        public int Month { get; set; }
        public ObservableCollection<MonthlyProductSummary> Items { get; set; }

        
        public double TotalMonth { get; set; }

        public GroupProductViewModel(int month, IEnumerable<MonthlyProductSummary> items, double totalMonth)
        {
            Month = month;
            Items = new ObservableCollection<MonthlyProductSummary>(items);

           
            TotalMonth = totalMonth;
        }
    }

    public class GroupServiceViewModel
    {
        public int Month { get; set; } 
        public string EmployeeCode { get; set; } 
        public string EmployeeName { get; set; } 
        public ObservableCollection<MonthlyServiceSummary> Items { get; set; } // Chi tiết công việc

        public double TotalServiceFee { get; set; } // Tổng phí dịch vụ theo nhân viên
        public double MonthlyTotalFee { get; set; } // Tổng phí dịch vụ theo tháng
       
        public GroupServiceViewModel(int month, string employeeCode, string employeeName, IEnumerable<MonthlyServiceSummary> items, double totalServiceFee, double monthlyTotalFee)
        {
            Month = month;
            EmployeeCode = employeeCode;
            EmployeeName = employeeName;
            Items = new ObservableCollection<MonthlyServiceSummary>(items);
            TotalServiceFee = totalServiceFee;
            MonthlyTotalFee = monthlyTotalFee;
        }
    }

    public class MonthlyGroupViewModel
    {
        public int Month { get; set; } // Tháng
        public double MonthlyTotalFee { get; set; } // Tổng tiền theo tháng
        public ObservableCollection<GroupServiceViewModel> EmployeeGroups { get; set; } // Nhóm theo nhân viên

        public MonthlyGroupViewModel(int month, double monthlyTotalFee, IEnumerable<GroupServiceViewModel> employeeGroups)
        {
            Month = month;
            MonthlyTotalFee = monthlyTotalFee;
            EmployeeGroups = new ObservableCollection<GroupServiceViewModel>(employeeGroups);
        }
    }



}
