using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using QuanLyMayMoc.Model;

namespace QuanLyMayMoc
{
    public interface IDao
    {
         public ObservableCollection<Employee> GetEmployees();

     

        public ObservableCollection<Task> GetTasks();
        public ObservableCollection<Task> GetTasksFromTemp();
        public ObservableCollection<Task> GetTasksFromTemp(DateTime? ngaythuchien, string keyword);
        public List<string> GetCustomerNamesFromDatabase(string query);
       
       
        public void DeleteTask(Task selectedTask);
        public void UpdateTask(Task selectedTask, Task newTask);
        public void InsertProject(Project projectInsert);
        public void InsertProjectTemp(Project projectInsert);
        public void InsertAllDataFromTemp(string projectID);
        public void InsertEmployeeToDaTaBase(Employee newEmployee);
        public  int TimSttLonNhat(string maduan);
        public void InsertTaskToDaTaBaseTemp(Task newTask);
        public  void DeleteAllTasks(string maDuAn);
        public ObservableCollection<Linhkien> GetAllLinhKien();
        public ObservableCollection<Linhkien> GetAllLinhKienTam();

        public void SaveToLinhKienTam();
        public void DeleteAllLinhKienTam();
        public void DeleteLinhKienTam(string maLinhKien);
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan);
        public Task<bool> CheckLinhKienTonTai(string maSanPham);
        public void UpdateLinhkien(Linhkien selectedLinhkien, Linhkien newLinhkien);
        public ObservableCollection<Loisp> GetAllLoi();

        public ObservableCollection<Loisp> GetAllLoiTam();
        public void SaveToLoiTam();
        public void DeleteAllLoiTam();
        public void DeleteLoiTam(string maLoi);
        public void InsertLoiToDaTaBaseTemp(Loisp newLinhKien, string mahieuduan);
        public Task<bool> CheckLoiTonTai(string maSanPham);
        public  void UpdateLoisp(Loisp selectedLoi, Loisp newLoi);
        public void SaveProjectWithDifferentName(Project projectInsert);

    }


}
