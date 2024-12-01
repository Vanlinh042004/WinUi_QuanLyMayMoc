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
        public ObservableCollection<Linhkien> GetAllLinhKien();
        public  ObservableCollection<Loisp> GetAllLoi();

        // Lấy tất cả công việc từ bảng Task
        public ObservableCollection<Task> GetTasks();
        public ObservableCollection<Task> GetTasksFromTemp();
        public ObservableCollection<Task> GetTasksFromTemp(DateTime? ngaythuchien, string keyword);
        public List<string> GetCustomerNamesFromDatabase(string query);
        public ObservableCollection<Project> GetProjects();
        public void DeleteTask(Task selectedTask);
        public void InsertProject(Project projectInsert);
        public void InsertProjectTemp(Project projectInsert);
        public void InsertAllDataFromTemp(string projectID);
        public void InsertEmployeeToDatabase(Employee newEmployee);
        public  int TimSttLonNhat(string maduan);

        public void InsertTaskToDatabaseTemp(Task newTask);
        public void SaveToLinhKienTam();
        public void DeleteAllLinhKienTam();
        public void DeleteLinhKienTam(string maLinhKien);
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan);
        public void SaveToLoiTam();
        public void DeleteAllLoiTam();
        public void DeleteLoiTam(string maLoi);
        public void InsertLoiToDaTaBaseTemp(Loisp newLinhKien, string mahieuduan);
        public void SaveProjectWithDifferentName(Project projectInsert, string oldProjectID);
        public void DeleteProject(Project selectedProject);



    }

}
