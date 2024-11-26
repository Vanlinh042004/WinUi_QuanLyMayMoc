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
        public ObservableCollection<Project> GetProjects();
        public void DeleteTask(Task selectedTask);
        public void InsertProject(Project projectInsert);
        public void InsertProjectTemp(Project projectInsert);
        public void InsertAllDataFromTemp(string projectID);
        public void InsertEmployeeToDaTaBase(Employee newEmployee);
        public  int TimSttLonNhat(string maduan);
        public void InsertTaskToDaTaBaseTemp(Task newTask);
        public ObservableCollection<Linhkien> GetAllLinhKien();
        public void SaveToLinhKienTam();
        public void DeleteAllLinhKienTam();
        public void DeleteLinhKienTam(string maLinhKien);
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan);
        public  ObservableCollection<Loisp> GetAllLoi();
        public void SaveToLoiTam();
        public void DeleteAllLoiTam();
        public void DeleteLoiTam(string maLoi);
        public void InsertLoiToDaTaBaseTemp(Loisp newLinhKien, string mahieuduan);

<<<<<<< HEAD
=======
        public void SaveProjectWithDifferentName(Project projectInsert);
>>>>>>> c52b2f52ae2ee76d25b731b3b5255c0f6ff245cf
    }


}
