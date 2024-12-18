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

    public class YearGroupViewModel
    {
        public int Year { get; set; }
        public ObservableCollection<GroupProductViewModel> MonthlyGroups { get; set; }
        public double TotalYear { get; set; }

        public YearGroupViewModel(int year, IEnumerable<GroupProductViewModel> monthlyGroups)
        {
            Year = year;
            MonthlyGroups = new ObservableCollection<GroupProductViewModel>(monthlyGroups);
            TotalYear = monthlyGroups.Sum(group => group.TotalMonth);
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

    public class YearlyServiceGroupViewModel
    {
        public int Year { get; set; } // Năm
        public double YearlyTotalFee { get; set; } // Tổng tiền theo năm
        public ObservableCollection<MonthlyGroupViewModel> MonthlyGroups { get; set; } // Nhóm theo tháng

        public YearlyServiceGroupViewModel(int year, double yearlyTotalFee, IEnumerable<MonthlyGroupViewModel> monthlyGroups)
        {
            Year = year;
            YearlyTotalFee = yearlyTotalFee;
            MonthlyGroups = new ObservableCollection<MonthlyGroupViewModel>(monthlyGroups);
        }
    }

    public class ConsolidatedMonthSummary
    {
        public int Month { get; set; } // Tháng
        public double TotalServiceFee { get; set; } // Tổng phí dịch vụ
        public double TotalProductFee { get; set; } // Tổng phí sản phẩm
        public double CombinedTotal => TotalServiceFee + TotalProductFee; // Tổng cộng

        public ConsolidatedMonthSummary(int month, double totalServiceFee, double totalProductFee)
        {
            Month = month;
            TotalServiceFee = totalServiceFee;
            TotalProductFee = totalProductFee;
        }
    }

    public class ConsolidatedYearSummary
    {
        public int Year { get; set; }
        public ObservableCollection<ConsolidatedMonthSummary> MonthlySummaries { get; set; }
        public double TotalYear => MonthlySummaries.Sum(x => x.CombinedTotal); // Tổng tiền cả năm

        public ConsolidatedYearSummary(int year, IEnumerable<ConsolidatedMonthSummary> monthlySummaries)
        {
            Year = year;
            MonthlySummaries = new ObservableCollection<ConsolidatedMonthSummary>(monthlySummaries);
        }
    }


}
