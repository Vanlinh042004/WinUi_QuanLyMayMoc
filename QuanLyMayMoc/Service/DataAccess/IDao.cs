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
        ObservableCollection<Employee> GetEmployees();
        ObservableCollection<Linhkien> GetAllLinhKien();
        ObservableCollection<Loisp> GetAllLoi();

        public ObservableCollection<Task> GetTasks();
        public ObservableCollection<Task> GetTasksFromTemp();
        public ObservableCollection<Task> GetTasksFromTemp(DateTime? ngaythuchien, string keyword);
        public List<string> GetCustomerNamesFromDatabase(string query);
        public ObservableCollection<Project> GetProjects();
        // Luu vao bang LinhKien
        public void DeleteTask(Task selectedTask);
        public void InsertProject(Project projectInsert);
        public void InsertProjectTemp(Project projectInsert);
        public void InsertAllDataFromTemp(string projectID);
        public void InsertEmployeeToDaTaBase(Employee newEmployee);


    }


}
