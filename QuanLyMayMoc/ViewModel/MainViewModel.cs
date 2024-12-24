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



    public class MainViewModel : INotifyPropertyChanged
    {
        IDao _dao;

       
        private Linhkien _currentSelectedLinhkien;
        public Linhkien CurrentSelectedLinhkien
        {
            get => _currentSelectedLinhkien;
            set
            {
                _currentSelectedLinhkien = value;
                OnPropertyChanged(nameof(CurrentSelectedLinhkien));
            }
        }
        private Loisp _currentSelectedLoi;
        public Loisp CurrentSelectedLoi
        {
            get => _currentSelectedLoi;
            set
            {
                _currentSelectedLoi = value;
                OnPropertyChanged(nameof(CurrentSelectedLoi));
            }
        }
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

        private ObservableCollection<GroupProductViewModel> _groupedProductItems;

        public ObservableCollection<GroupProductViewModel> GroupedProductItems
        {
            get => _groupedProductItems;
            set
            {
                if (_groupedProductItems != value)
                {
                    _groupedProductItems = value;
                    OnPropertyChanged(nameof(GroupedProductItems));
                }
            }
        }

        public ObservableCollection<YearGroupViewModel> GroupedYearItems { get; set; }

        public ObservableCollection<GroupServiceViewModel> GroupedServiceItems { get; set; }
        public ObservableCollection<MonthlyGroupViewModel> MonthlyGroupedItems { get; set; }

        public ObservableCollection<YearlyServiceGroupViewModel> GroupedYearServiecItems { get; set; }

        public ObservableCollection<ConsolidatedYearSummary> ConsolidatedYearSummaries { get; set; }

        public MainViewModel()
        {

            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Employees = new ObservableCollection<Employee>();
            Tasks = _dao.GetTasks();
            Listlinhkien = new ObservableCollection<Linhkien>();
            ListLoi = new ObservableCollection<Loisp>();
            MonthlyProductSummarys = _dao.GetMonthlyProductSummaries();
            MonthlyServiceSummarys = _dao.GetMonthlyServiceSummaries();
            GroupedYearItems = GroupProducts();
            GroupedYearServiecItems= GroupServices();
            ConsolidatedYearSummaries= SummaryYear();

        }

        public ObservableCollection<ConsolidatedYearSummary> SummaryYear()
        {
            // Tạo một danh sách rỗng để lưu kết quả tổng hợp
            var consolidatedYearSummaries = new ObservableCollection<ConsolidatedYearSummary>();

            // Lấy tất cả các năm từ cả hai danh sách
            var allYears = GroupedYearItems.Select(p => p.Year)
                                           .Union(GroupedYearServiecItems.Select(s => s.Year))
                                           .Distinct();

            foreach (var year in allYears)
            {
                // Lấy dữ liệu sản phẩm và dịch vụ theo năm
                var productYear = GroupedYearItems.FirstOrDefault(p => p.Year == year);
                var serviceYear = GroupedYearServiecItems.FirstOrDefault(s => s.Year == year);

                // Lấy tất cả các tháng từ cả sản phẩm và dịch vụ
                var allMonths = (productYear?.MonthlyGroups.Select(p => p.Month) ?? Enumerable.Empty<int>())
                                .Union(serviceYear?.MonthlyGroups.Select(s => s.Month) ?? Enumerable.Empty<int>())
                                .Distinct();

                var consolidatedMonthlySummaries = allMonths.Select(month =>
                {
                    // Lấy dữ liệu tháng từ sản phẩm và dịch vụ
                    var productMonth = productYear?.MonthlyGroups.FirstOrDefault(p => p.Month == month);
                    var serviceMonth = serviceYear?.MonthlyGroups.FirstOrDefault(s => s.Month == month);

                    double productFee = productMonth?.TotalMonth ?? 0; // Nếu không có thì là 0
                    double serviceFee = serviceMonth?.MonthlyTotalFee ?? 0; // Nếu không có thì là 0

                    return new ConsolidatedMonthSummary(month, serviceFee, productFee);
                }).ToList();

                // Thêm dữ liệu tổng hợp của năm vào danh sách
                consolidatedYearSummaries.Add(new ConsolidatedYearSummary(year, consolidatedMonthlySummaries));
            }

            // Trả về danh sách đã tổng hợp
            return consolidatedYearSummaries;
        }


        private ObservableCollection<YearGroupViewModel> GroupProducts()
        {
            var groupedByYear = MonthlyProductSummarys
                .GroupBy(p => p.Year)
                .Select(yearGroup => new YearGroupViewModel(
                    yearGroup.Key,
                    yearGroup.GroupBy(p => p.Month)
                        .Select(monthGroup => new GroupProductViewModel(
                            monthGroup.Key,
                            monthGroup,
                            monthGroup.Sum(item => item.Quantity * item.Price)
                        ))
                        .ToList()
                ))
                .ToList();

             return new ObservableCollection<YearGroupViewModel>(groupedByYear);
        }


        private ObservableCollection<YearlyServiceGroupViewModel> GroupServices()
        {
            var monthlyServiceSummaries = MonthlyServiceSummarys;

            // Gom nhóm theo năm từ dữ liệu MonthlyServiceSummary
            var yearlyGroupedServices = monthlyServiceSummaries
                .GroupBy(s => s.Year) // Group theo thuộc tính Year
                .Select(yearGroup =>
                {
                    // Trong mỗi năm, gom nhóm theo tháng
                    var monthlyGroups = yearGroup
                        .GroupBy(s => s.Month) // Group theo thuộc tính Month
                        .Select(monthGroup =>
                        {
                            // Trong mỗi tháng, gom nhóm theo nhân viên
                            var employeeGroups = monthGroup
                                .GroupBy(s => new { s.EmployeeCode, s.EmployeeName })
                                .Select(employeeGroup => new GroupServiceViewModel(
                                    monthGroup.Key,                                // Tháng
                                    employeeGroup.Key.EmployeeCode,               // Mã nhân viên
                                    employeeGroup.Key.EmployeeName,               // Tên nhân viên
                                    employeeGroup.ToList(),                       // Chi tiết công việc
                                    employeeGroup.Sum(s => s.ServiceFee),         // Tổng phí dịch vụ của nhân viên
                                    monthGroup.First().MonthlyTotalFee            // Tổng phí dịch vụ của tháng
                                )).ToList();

                            return new MonthlyGroupViewModel(
                                monthGroup.Key,                          // Tháng
                                monthGroup.First().MonthlyTotalFee,      // Tổng phí dịch vụ của tháng
                                employeeGroups                          // Danh sách nhân viên trong tháng
                            );
                        }).ToList();

                    return new YearlyServiceGroupViewModel(
                        yearGroup.Key,                                   // Năm
                        yearGroup.Sum(s => s.ServiceFee),                // Tổng phí dịch vụ của năm
                        monthlyGroups                                   // Danh sách nhóm theo tháng
                    );
                }).ToList();

            // Gán dữ liệu vào GroupedYearServiecItems
            return new ObservableCollection<YearlyServiceGroupViewModel>(yearlyGroupedServices);
        }

        public void RefreshData(DateTime ngaythuchien, string keyword)
        {


            Tasks.Clear();


            var filteredTasks = _dao.GetAllTasksFromDatabase(ngaythuchien, keyword);
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        public void RefreshData()
        {
            if (Tasks != null)
            {
                Tasks.Clear();
            }

            var allTasks = _dao.GetAllTasksFromDatabase();

            var sortedTasks = allTasks.OrderBy(task => task.Stt);


            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
            

            LoadDataEmployee();
            LoadSummary();
            LoadSummaryYear();

            RefreshLinhKien();
            RefreshLoi();

        }
        public void LoadSummary()
        {
            // Clear existing summaries
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

            // Clear the grouped items
            if (GroupedYearItems != null)
            {
                GroupedYearItems.Clear();
            }

            // Get new monthly product summaries and group products
            MonthlyProductSummarys = _dao.GetMonthlyProductSummaries();
            var groupedProducts = GroupProducts(); // Get grouped product items

            // Add grouped product items to GroupedYearItems
            foreach (var item in groupedProducts)
            {
                GroupedYearItems.Add(item);
            }

            // Clear and get new monthly service summaries and group services
            if (GroupedYearServiecItems != null)
            {
                GroupedYearServiecItems.Clear();
            }

            MonthlyServiceSummarys = _dao.GetMonthlyServiceSummaries();
            var groupedServices = GroupServices(); // Get grouped service items

            // Add grouped service items to GroupedYearServiecItems
            foreach (var item in groupedServices)
            {
                GroupedYearServiecItems.Add(item);
            }
        }

        public void LoadSummaryYear()
        {
            // If ConsolidatedYearSummaries is not null, clear the collection
            if (ConsolidatedYearSummaries != null)
            {
                ConsolidatedYearSummaries.Clear();
            }

            // Get the summary year data from the SummaryYear method
            var yearSummaries = SummaryYear();

            // Loop through each year summary and add it to ConsolidatedYearSummaries
            foreach (var yearSummary in yearSummaries)
            {
                ConsolidatedYearSummaries.Add(yearSummary);
            }
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

      

     

        public List<string> GetCustomerNames(string query)
        {

            return _dao.GetCustomerNamesFromDatabase(query);
        }

        public List<string> GetEmployeeCodes(string query)
        {

            return _dao.GetEmployeeCodesFromDatabase(query);
        }

        public List<string> GetPartCodes(string query)
        {

            return _dao.GetPartCodesFromDatabase(query);
        }

        public List<string> GetCoreCodes(string query)
        {

            return _dao.GetCoreCodesFromDatabase(query);
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
        public int CheckMaSanPhamLinhKienTonTaiKhac(string maSanPham)
        {
            return _dao.CheckLinhKienTonTaiKhac(maSanPham);
        }
        public int CheckLinhKienDuAnTonTai(string maDuAn)
        {
            return _dao.CheckLinhKienDuAnTonTai(maDuAn);
        }
        public int CheckLinhKienDuAnTamTonTai(string maDuAn)
        {
            return _dao.CheckLinhKienDuAnTamTonTai(maDuAn);
        }
        public int CheckDuAnTamTonTai(string maDuAn)
        {
            return _dao.CheckDuAnTam(maDuAn);
        }

        public int CheckDuAnTonTai(string maDuAn)
        {
            return _dao.CheckDuAn(maDuAn);
        }
        public void UpdateSelectedLinhkien(Linhkien newLinhkien, Linhkien CurrentSelectedLinhkien)
        {
            
            if (CurrentSelectedLinhkien != null)
            {
                string maHieuDuAn = CurrentSelectedLinhkien.MaSanPham + "_" + AppData.ProjectID;
                var updatedLinhkien = CurrentSelectedLinhkien;
                if(CheckMaSanPhamLinhKienTonTaiKhac(CurrentSelectedLinhkien.MaSanPham) > 0)
                {
                    _dao.UpdateLinhkien(updatedLinhkien, newLinhkien);

                    var linhkienIndex = Listlinhkien.IndexOf(updatedLinhkien);

                    if (linhkienIndex >= 0)
                    {
                        Listlinhkien[linhkienIndex] = newLinhkien;
                    }
                }
                else
                {
                    _dao.InsertLinhKienToDaTaBaseTemp(newLinhkien, maHieuDuAn);
                    var linhkienIndex = Listlinhkien.IndexOf(CurrentSelectedLinhkien);

                    if (linhkienIndex >= 0)
                    {
                        Listlinhkien[linhkienIndex] = newLinhkien;
                    }
                }

            }
            CurrentSelectedLinhkien = null;
        }
        public void RefreshLinhKien()
        {
            
            if (Listlinhkien != null)
            {
                Listlinhkien.Clear();
            }
            var allLinhKien = _dao.GetAllLinhKienTam();
            foreach (var linhkien in allLinhKien)
            {
                Listlinhkien.Add(linhkien);
            }

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
        public int CheckLoiTonTaiKhac(string maSanPham)
        {
            return _dao.CheckLoiTonTaiKhac(maSanPham);
        }
        public int CheckLoiDuAnTonTai(string maDuAn)
        {
            return _dao.CheckLoiDuAnTonTai(maDuAn);
        }
        public int CheckLoiDuAnTamTonTai(string maDuAn)
        {
            return _dao.CheckLoiDuAnTamTonTai(maDuAn);
        }
        public void RefreshLoi()
        {
            if (ListLoi != null)
            {
                ListLoi.Clear();
            }
            var allLoi = _dao.GetAllLoiTam();
            foreach (var loi in allLoi)
            {
                ListLoi.Add(loi);
            }
        }
        public void UpdateSelectedLoi(Loisp newLoi, Loisp CurrentSelectedLoi)
        {
            if (CurrentSelectedLoi != null)
            {

                if (CheckLoiTonTaiKhac(CurrentSelectedLoi.MaSanPham) > 0)
                {
                    _dao.UpdateLoisp(CurrentSelectedLoi, newLoi);
                    var loiIndex = ListLoi.IndexOf(CurrentSelectedLoi);
                    if (loiIndex >= 0)
                    {
                        ListLoi[loiIndex] = newLoi;
                    }
                }
                else
                {
                    _dao.InsertLoiToDaTaBaseTemp(newLoi, AppData.ProjectID);
                    var loiIndex = ListLoi.IndexOf(CurrentSelectedLoi);
                    if (loiIndex >= 0)
                    {
                        ListLoi[loiIndex] = newLoi;
                    }
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

            // Tạo danh sách tạm chứa kết quả tổng hợp từ Tasks
            var results = new List<dynamic>();

            // Duyệt qua từng task để tổng hợp dữ liệu linh kiện và lõi
            foreach (var task in Tasks)
            {
                if (!string.IsNullOrEmpty(task.MaLK))
                {
                    LoadLinhKienFromTemp();
                    var linhkien = Listlinhkien.FirstOrDefault(lk => lk.MaSanPham == task.MaLK);

                    results.Add(new
                    {
                        Year = task.NgayThucHien.Year,
                        Month = task.NgayThucHien.Month,
                        ProductCode = task.MaLK,
                        ProductName = linhkien?.TenSanPham ?? "Unknown Linhkien",
                        Price = linhkien?.GiaBan ?? 0,
                        Quantity = task.SoLuongLK,
                        TotalPrice = task.SoLuongLK * (linhkien?.GiaBan ?? 0.0)
                    });
                }

                if (!string.IsNullOrEmpty(task.MaLoi))
                {
                    LoadLoiFromTemp();
                    var loi = ListLoi.FirstOrDefault(l => l.MaSanPham == task.MaLoi);

                    results.Add(new
                    {
                        Year = task.NgayThucHien.Year,
                        Month = task.NgayThucHien.Month,
                        ProductCode = task.MaLoi,
                        ProductName = loi?.TenSanPham ?? "Unknown Loi",
                        Price = loi?.GiaBan ?? 0,
                        Quantity = task.SoLuongLoi,
                        TotalPrice = (double)(task.SoLuongLoi * (loi?.GiaBan ?? 0.0))
                    });
                }
            }

            // Nhóm dữ liệu theo năm, tháng và mã sản phẩm
            var groupedTasks = results
                .GroupBy(item => new { item.Year, item.Month, item.ProductCode })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    ProductCode = group.Key.ProductCode,
                    ProductName = group.First().ProductName,
                    Price = group.First().Price,
                    Quantity = group.Sum(item => item.Quantity),
                    TotalPrice = group.Sum(item => (double)item.TotalPrice)
                })
                .ToList();

            // Tạo danh sách MonthlyProductSummary từ dữ liệu đã nhóm
            foreach (var groupedTask in groupedTasks)
            {
                monthlyProductSummaries.Add(new MonthlyProductSummary
                {
                    Code = GenerateUniqueCode(),
                    Year = groupedTask.Year,
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
                .GroupBy(task => new { Year = task.NgayThucHien.Year, Month = task.NgayThucHien.Month })
                .ToDictionary(
                    group => group.Key, // Năm và Tháng
                    group => group.Sum(task => task.PhiDichVu) // Tổng phí dịch vụ của tháng đó
                );

            // Nhóm các Task theo năm, tháng, nhân viên và STT
            var groupedTasks = Tasks
                .GroupBy(task => new { Year = task.NgayThucHien.Year, Month = task.NgayThucHien.Month, EmployeeCode = task.MaNV, STT = task.Stt })
                .Select(group =>
                {
                    LoadDataEmployee();
                    var employee = Employees.FirstOrDefault(emp => emp.MaNhanVien == group.Key.EmployeeCode);

                    return new MonthlyServiceSummary
                    {
                        Code = GenerateUniqueCode(),
                        Year = group.Key.Year, // Năm
                        Month = group.Key.Month, // Tháng
                        EmployeeCode = group.Key.EmployeeCode, // Mã nhân viên
                        EmployeeName = employee?.HoTen ?? "Unknown Employee", // Tên nhân viên
                        STT = group.Key.STT, // STT của công việc
                        ServiceFee = group.Sum(task => task.PhiDichVu), // Phí dịch vụ cho công việc này
                        TotalServiceFee = group.Sum(task => task.PhiDichVu), // Tổng phí dịch vụ (trong nhóm công việc)
                        MonthlyTotalFee = monthlyTotalFees[new { Year = group.Key.Year, Month = group.Key.Month }] // Tổng phí dịch vụ của tháng
                    };
                })
                .ToList();

            return new ObservableCollection<MonthlyServiceSummary>(groupedTasks);
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
        public bool CheckTaskExists(Task congviec)
        {

            return _dao.CountTask(congviec) > 0;


        }
       
        public void SaveProjectWithDifferentName(Project project, string newProject)
        {
            _dao.SaveProjectWithDifferentName(project, newProject);
            RefreshData();
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


