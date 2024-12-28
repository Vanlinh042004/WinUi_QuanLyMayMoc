using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using Npgsql;
using QuanLyMayMoc.Model;

namespace QuanLyMayMoc
{
    public interface IDao
    {
        public ObservableCollection<Employee> GetEmployees();
        public ObservableCollection<User> GetUsers(string projectId);


        public ObservableCollection<MonthlyProductSummary> GetMonthlyProductSummaries();
        public ObservableCollection<MonthlyServiceSummary> GetMonthlyServiceSummaries();




        // Lấy tất cả công việc từ bảng Task
        public ObservableCollection<Task> GetTasks();
        public ObservableCollection<Task> GetAllTasksFromDatabase();
        public ObservableCollection<Task> GetAllTasksFromDatabase(DateTime? ngaythuchien, string keyword);
        public List<string> GetCustomerNamesFromDatabase(string query);
        public List<string> GetEmployeeCodesFromDatabase(string query);
        public List<string> GetPartCodesFromDatabase(string query);
        public List<string> GetCoreCodesFromDatabase(string query);

        public ObservableCollection<Project> GetProjects();
        public void DeleteTask(Task selectedTask);
        public void UpdateTask(Task selectedTask, Task newTask);
        public void InsertProject(Project projectInsert);
        public void InsertProjectTemp(Project projectInsert);
        public void InsertAllDataFromTemp(string projectID);
        public void InsertEmployeeToDatabase(Employee newEmployee);
        public int TimSttLonNhat(string maduan);
        //public void InsertTaskToDaTaBaseTemp(Task newTask);
        public void InsertTaskToDaTaBaseTemp(Task newTask);
        public void DeleteAllTasks(string maDuAn);
        public ObservableCollection<Linhkien> GetAllLinhKien();
        public ObservableCollection<Linhkien> GetAllLinhKienTam();
        public ObservableCollection<Linhkien> GetAllLinhKienDuAn();
        public int CheckLinhKienDuAnTonTai(string maDuAn);
        public int CheckLinhKienDuAnTamTonTai(string maDuAn);
        public void SaveToLinhKienTam();
        public void SaveToLinhKienDuAnToTam();
        public void DeleteAllLinhKienTam();
        public void DeleteLinhKienTam(string maLinhKien);
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan);
        public Task<bool> CheckLinhKienTonTai(string maSanPham);
        public int CheckLinhKienTonTaiKhac(string maSanPham);
        public int CheckDuAnTam(string maDuAn);
        public int CheckDuAn(string maDuAn);
        public void UpdateLinhkien(Linhkien selectedLinhkien, Linhkien newLinhkien);
        public ObservableCollection<Loisp> GetAllLoi();
        public ObservableCollection<Loisp> GetAllLoiTam();
        public ObservableCollection<Loisp> GetAllLoiDuAn();
        public void SaveToLoiTam();
        public void SaveToLoiDuAnToTam();
        public void DeleteAllLoiTam();
        public void DeleteLoiTam(string maLoi);
        public void InsertLoiToDaTaBaseTemp(Loisp newLinhKien, string mahieuduan);
        public Task<bool> CheckLoiTonTai(string maSanPham);
        public int CheckLoiTonTaiKhac(string maSanPham);
        public int CheckLoiDuAnTonTai(string maDuAn);
        public int CheckLoiDuAnTamTonTai(string maDuAn);

        public void UpdateLoisp(Loisp selectedLoi, Loisp newLoi);
        public void SaveProjectWithDifferentName(Project projectInsert, string maDuAnCu);
        public void SummaryProduct(ObservableCollection<MonthlyProductSummary> monthlyProductSummaries);

        public void SummaryService(ObservableCollection<MonthlyServiceSummary> monthlyServiceSummaries);

        public void ClearSummary();

        public void DeleteProject(string maDuAn);
        public void DeleteProjectUser(string email );
        public void DeleteProjectUser(string email, string projectId );
        public int CountMaNhanVien(string MaNhanVien);
        public int CountTask(Task congviec);

        public void ClearAllTempData();

        public void AddUserToProject(User newUser, string Id, string maduan);
    }

}
