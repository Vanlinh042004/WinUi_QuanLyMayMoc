using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.Service;

namespace QuanLyMayMoc.ViewModel
{



    public class MainViewModel
    {
        IDao _dao;



        private Task _currentSelectedTask;
        public Task CurrentSelectedTask
        {
            get => _currentSelectedTask;
            set
            {
                _currentSelectedTask = value;
                OnPropertyChanged(nameof(CurrentSelectedTask));
            }
        }


        public ObservableCollection<Employee> Employees
        {
            get; set;
        }
        public ObservableCollection<Task> Tasks
        {
            get; set;
        }




        public ObservableCollection<MonthlyProductSummary> MonthlyProductSummarys
        {
            get; set;
        }

        public ObservableCollection<MonthlyServiceSummary> MonthlyServiceSummarys
        {
            get; set;
        }

        public ObservableCollection<GroupProductViewModel> GroupedProductItems { get; set; }
        public ObservableCollection<GroupServiceViewModel> GroupedServiceItems { get; set; }
        public ObservableCollection<MonthlyGroupViewModel> MonthlyGroupedItems { get; set; }


        public MainViewModel()
        {

            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Employees = new ObservableCollection<Employee>();
            Tasks = _dao.GetTasks();
            Listlinhkien = new ObservableCollection<Linhkien>();
            ListLoi = new ObservableCollection<Loisp>();
            MonthlyProductSummarys = _dao.GetMonthlyProductSummaries();
            MonthlyServiceSummarys = _dao.GetMonthlyServiceSummaries();
            GroupProducts();
            GroupServices();


        }

        private void GroupProducts()
        {
            var grouped = MonthlyProductSummarys
           .GroupBy(p => p.Month)
           .Select(g => new GroupProductViewModel(
               g.Key,
               g,
               g.Sum(item => item.Quantity * item.Price)
           ))
           .ToList();

            GroupedProductItems = new ObservableCollection<GroupProductViewModel>(grouped);
        }

        private void GroupServices()
        {
            var groupedServices = MonthlyServiceSummarys
          .GroupBy(s => new { s.Month, s.EmployeeCode, s.EmployeeName }) // Gom theo tháng và nhân viên
          .Select(g => new GroupServiceViewModel(
              g.Key.Month,
              g.Key.EmployeeCode,
              g.Key.EmployeeName,
              g,                                // Danh sách công việc
              g.First().TotalServiceFee,
              g.First().MonthlyTotalFee
          )).ToList();

            GroupedServiceItems = new ObservableCollection<GroupServiceViewModel>(groupedServices);

            // Gom nhóm theo tháng
            var monthlyGroups = GroupedServiceItems
             .GroupBy(g => g.Month) // Gom nhóm theo tháng
             .Select(g => new MonthlyGroupViewModel(
                 g.Key,                      // Tháng
                 g.First().MonthlyTotalFee,  // Lấy giá trị MonthlyTotalFee đầu tiên
                 g                           // Danh sách nhân viên và công việc trong tháng
             ))
             .ToList();



            MonthlyGroupedItems = new ObservableCollection<MonthlyGroupViewModel>(monthlyGroups);
        }

        public void LoadDataFilter(DateTime ngaythuchien, string keyword)
        {


            Tasks.Clear();


            var filteredTasks = _dao.GetTasksFromTemp(ngaythuchien, keyword);
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        public void LoadDataFilter()
        {
            if (Tasks != null)
            {
                Tasks.Clear();
            }

            var allTasks = _dao.GetTasksFromTemp();

            var sortedTasks = allTasks.OrderBy(task => task.Stt);


            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }

            if (MonthlyProductSummarys != null)
            {
                MonthlyProductSummarys.Clear();
            }

            if (MonthlyServiceSummarys != null)
            {
                MonthlyServiceSummarys.Clear();
            }

            ClearSummary();
            SummaryProduct();
            SummaryService();

        }

        public void LoadDataEmployee()
        {
            if (Employees != null)
            {
                Employees.Clear();
            }

            var allEmployees = _dao.GetEmployees();




            foreach (var employee in allEmployees)
            {
                Employees.Add(employee);
            }



        }
        public void InsertTaskToDaTaBaseTemp(Task newTask)
        {
            _dao.InsertTaskToDaTaBaseTemp(newTask);
        }

        public int CheckLinhKienDuAnTonTai(string maDuAn)
        {
            return _dao.CheckLinhKienDuAnTonTai(maDuAn);
        }
        public int CheckLinhKienDuAnTamTonTai(string maDuAn)
        {
            return _dao.CheckLinhKienDuAnTamTonTai(maDuAn);
        }

        public int CheckLoiDuAnTonTai(string maDuAn)
        {
            return _dao.CheckLoiDuAnTonTai(maDuAn);
        }
        public int CheckLoiDuAnTamTonTai(string maDuAn)
        {
            return _dao.CheckLoiDuAnTamTonTai(maDuAn);
        }

        public List<string> GetCustomerNames(string query)
        {

            return _dao.GetCustomerNamesFromDatabase(query);
        }

        public ObservableCollection<Project> getProjects()
        {
            return _dao.GetProjects();
        }


        public void RemoveSelectedTask()

        {
            if (CurrentSelectedTask != null)
            {
                Tasks.Remove(CurrentSelectedTask);


                var selectedTask = CurrentSelectedTask; // Lưu lại dữ liệu trước khi xóa



                if (selectedTask != null)
                {
                    _dao.DeleteTask(selectedTask);
                }
            }
            CurrentSelectedTask = null;
        }

        public void UpdateSelectedTask(Task newTask)
        {
            if (CurrentSelectedTask != null)
            {
                var updatedTask = CurrentSelectedTask;


                _dao.UpdateTask(updatedTask, newTask);


                var taskIndex = Tasks.IndexOf(updatedTask);

                if (taskIndex >= 0)
                {

                    Tasks[taskIndex] = updatedTask;
                }
            }
            CurrentSelectedTask = null;
        }


        public void RemoveAllTask()
        {

            Tasks.Clear();

        }

        public void InsertProject(Project project)
        {
            _dao.InsertProject(project);
        }
        public void InsertProjectTemp(Project project)
        {
            _dao.InsertProjectTemp(project);
        }
        public void InsertAllDataFromTemp(string projectID)
        {
            _dao.InsertAllDataFromTemp(projectID);

        }

        public void InsertToEmployees(Employee newEmployee)
        {
            _dao.InsertEmployeeToDatabase(newEmployee);
        }

        public int TimSttLonNhat(string maduan)
        {
            return _dao.TimSttLonNhat(maduan);
        }






        public void DeleteAllTask(string maDuAn)
        {
            _dao.DeleteAllTasks(maDuAn);
        }


        //Linhkien

        public ObservableCollection<Linhkien> Listlinhkien
        {
            get; set;
        }

        public void LoadLinhKienFromDatabase()
        {
            Listlinhkien = _dao.GetAllLinhKien();
            SaveToLinhKienTam(); // Lưu dữ liệu vào bảng tạm
        }

        public void LoadLinhKienFromTemp()
        {
            Listlinhkien = _dao.GetAllLinhKienTam();
        }
        public void LoadLinhKienFromDuAn()
        {
            Listlinhkien = _dao.GetAllLinhKienDuAn();
        }
        public void SaveToLinhKienTam()
        {
            _dao.SaveToLinhKienTam();
        }
        public void SaveToLinhKienDuAnToTam()
        {
            _dao.SaveToLinhKienDuAnToTam();
        }
        public void DeleteAllLinhKienTam()
        {
            _dao.DeleteAllLinhKienTam();

        }
        public void DeleteLinhKienTam(string maLinhKien)
        {
            _dao.DeleteLinhKienTam(maLinhKien);
        }
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan)
        {
            _dao.InsertLinhKienToDaTaBaseTemp(newLinhKien, mahieuduan);
        }
        public Task<bool> CheckMaSanPhamTonTai(string maSanPham)
        {
            return _dao.CheckLinhKienTonTai(maSanPham);
        }

        public void UpdateSelectedLinhkien(Linhkien newLinhkien, Linhkien CurrentSelectedLinhkien)
        {
            if (CurrentSelectedLinhkien != null)
            {
                var updatedLinhkien = CurrentSelectedLinhkien;

                _dao.UpdateLinhkien(updatedLinhkien, newLinhkien);

                var linhkienIndex = Listlinhkien.IndexOf(updatedLinhkien);

                if (linhkienIndex >= 0)
                {
                    Listlinhkien[linhkienIndex] = newLinhkien;
                }
            }
            CurrentSelectedLinhkien = null;
        }

        // Lõi
        public ObservableCollection<Loisp> ListLoi
        {
            set; get;
        }

        public void LoadLoiFromDatabase()
        {
            ListLoi = _dao.GetAllLoi();
            SaveToLoiTam();
        }
        public void LoadLoiFromTemp()
        {
            ListLoi = _dao.GetAllLoiTam();
        }
        public void LoadLoiFromDuAn()
        {
            ListLoi = _dao.GetAllLoiDuAn();
        }
        public void SaveToLoiTam()
        {
            _dao.SaveToLoiTam();
        }
        public void SaveToLoiDuAnToTam()
        {
            _dao.SaveToLoiDuAnToTam();
        }
        public void DeleteAllLoiTam()
        {
            _dao.DeleteAllLoiTam();
        }

        public void DeleteLoiTam(string maLoi)
        {
            _dao.DeleteLoiTam(maLoi);
        }

        public void InsertLoiToDaTaBaseTemp(Loisp newLoi, string mahieuduan)
        {
            _dao.InsertLoiToDaTaBaseTemp(newLoi, mahieuduan);
        }

        public Task<bool> CheckLoiTonTai(string maSanPham)
        {
            return _dao.CheckLoiTonTai(maSanPham);
        }


        public void UpdateSelectedLoi(Loisp newLoi, Loisp CurrentSelectedLoi)
        {
            if (CurrentSelectedLoi != null)
            {

                _dao.UpdateLoisp(CurrentSelectedLoi, newLoi);

                var loiIndex = ListLoi.IndexOf(CurrentSelectedLoi);

                if (loiIndex >= 0)
                {
                    ListLoi[loiIndex] = newLoi;
                }
            }
            CurrentSelectedLoi = null;
        }



        public void DeleteProject(string project)
        {
            _dao.DeleteProject(project);
        }
        public void SummaryProduct()
        {
            // Tổng hợp dữ liệu từ Tasks để tạo MonthlyProductSummarys
            var monthlyProductSummaries = AggregateTasksToMonthlyProductSummaries();

            // Gửi dữ liệu đã tổng hợp đến DAO để lưu vào database
            _dao.SummaryProduct(monthlyProductSummaries);
        }



        private ObservableCollection<MonthlyProductSummary> AggregateTasksToMonthlyProductSummaries()
        {
            var monthlyProductSummaries = new ObservableCollection<MonthlyProductSummary>();

            // Nhóm các Task theo tháng và sản phẩm
            var groupedTasks = Tasks
                     .SelectMany(task =>
                     {
                         var results = new List<dynamic>();

                         // Nếu có linh kiện
                         if (!string.IsNullOrEmpty(task.MaLK))
                         {
                             LoadLinhKienFromTemp();
                             var linhkien = Listlinhkien.FirstOrDefault(lk => lk.MaSanPham == task.MaLK);
                             results.Add(new
                             {
                                 Month = task.NgayThucHien.Month,
                                 ProductCode = task.MaLK,
                                 ProductName = linhkien?.TenSanPham ?? "Unknown Linhkien",
                                 Price = linhkien?.GiaBan ?? 0,
                                 Quantity = task.SoLuongLK,
                                 TotalPrice = task.SoLuongLK * (linhkien?.GiaBan ?? 0.0)
                             });
                         }

                         // Nếu có lỗi
                         if (!string.IsNullOrEmpty(task.MaLoi))
                         {
                             LoadLoiFromTemp();
                             var loi = ListLoi.FirstOrDefault(l => l.MaSanPham == task.MaLoi);
                             results.Add(new
                             {
                                 Month = task.NgayThucHien.Month,
                                 ProductCode = task.MaLoi,
                                 ProductName = loi?.TenSanPham ?? "Unknown Loi",
                                 Price = loi?.GiaBan ?? 0,
                                 Quantity = task.SoLuongLoi,
                                 TotalPrice = (double)(task.SoLuongLoi * (loi?.GiaBan ?? 0.0))
                             });
                         }

                         return results;
                     })
                     .GroupBy(item => new { item.Month, item.ProductCode })
                     .Select(group => new
                     {
                         Month = group.Key.Month,
                         ProductCode = group.Key.ProductCode,
                         ProductName = group.First().ProductName,
                         Price = group.First().Price,
                         Quantity = group.Sum(item => item.Quantity),
                         TotalPrice = group.Sum(item => (double)item.TotalPrice)
                     })
                     .ToList();


            // Tạo danh sách MonthlyProductSummary
            foreach (var groupedTask in groupedTasks)
            {
                monthlyProductSummaries.Add(new MonthlyProductSummary
                {
                    Code = GenerateUniqueCode(),
                    Month = groupedTask.Month,
                    ProductCode = groupedTask.ProductCode,
                    ProductName = groupedTask.ProductName,
                    Quantity = groupedTask.Quantity,
                    Price = groupedTask.Price,
                    TotalPrice = groupedTask.TotalPrice
                });
            }

            return monthlyProductSummaries;
        }

        public void SummaryService()
        {
            // Tổng hợp dữ liệu từ Tasks để tạo danh sách MonthlyServiceSummary
            var monthlyServiceSummaries = AggregateTasksToMonthlyServiceSummaries();

            // Gửi dữ liệu đã tổng hợp đến DAO để lưu vào database
            _dao.SummaryService(monthlyServiceSummaries);
        }

        private ObservableCollection<MonthlyServiceSummary> AggregateTasksToMonthlyServiceSummaries()
        {
            var monthlyServiceSummaries = new ObservableCollection<MonthlyServiceSummary>();

            // Tính tổng phí dịch vụ của cả tháng cho tất cả nhân viên
            var monthlyTotalFees = Tasks
                .GroupBy(task => task.NgayThucHien.Month)
                .ToDictionary(
                    group => group.Key, // Tháng
                    group => group.Sum(task => task.PhiDichVu) // Tổng phí dịch vụ của tháng đó
                );

            // Nhóm các Task theo tháng và nhân viên
            var groupedTasks = Tasks
                .GroupBy(task => new { Month = task.NgayThucHien.Month, EmployeeCode = task.MaNV })
                .Select(group => new
                {
                    Month = group.Key.Month,
                    EmployeeCode = group.Key.EmployeeCode,
                    EmployeeName = group.First().TenNV ?? "Unknown Employee",
                    TaskCode = group.First().MaCVDuAn ?? "Unknown Task",
                    TotalServiceFee = group.Sum(task => task.PhiDichVu) // Tổng phí dịch vụ của nhân viên
                })
                .ToList();

            // Tạo danh sách MonthlyServiceSummary
            foreach (var groupedTask in groupedTasks)
            {
                foreach (var task in Tasks.Where(t => t.MaNV == groupedTask.EmployeeCode && t.NgayThucHien.Month == groupedTask.Month))
                {
                    monthlyServiceSummaries.Add(new MonthlyServiceSummary
                    {
                        Code = GenerateUniqueCode(),
                        Month = groupedTask.Month,
                        EmployeeCode = groupedTask.EmployeeCode,
                        EmployeeName = groupedTask.EmployeeName,
                        TaskCode = task.MaCVDuAn ?? "Unknown Task",
                        ServiceFee = task.PhiDichVu,
                        TotalServiceFee = groupedTask.TotalServiceFee,
                        MonthlyTotalFee = monthlyTotalFees[groupedTask.Month] // Lấy tổng phí dịch vụ của tháng từ dictionary
                    });
                }
            }

            return monthlyServiceSummaries;
        }


        public string GenerateUniqueCode()
        {
            Random random = new Random();
            string randomPart = random.Next(1000, 9999).ToString(); // Phần ngẫu nhiên
            string timePart = DateTime.Now.ToString("yyyyMMddHHmmssfff"); // Phần thời gian
            return timePart + randomPart;
        }

        public void ClearSummary()
        {
            _dao.ClearSummary();
        }

        public bool CheckEmployeeExistsAsync(string MaNhanVien)
        {

            return _dao.CountMaNhanVien(MaNhanVien) > 0;


        }

        public void SaveProjectWithDifferentName(Project project, string newProject)
        {
            _dao.SaveProjectWithDifferentName(project, newProject);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearAllTempData()
        {
            _dao.ClearAllTempData();
        }
    }


}


