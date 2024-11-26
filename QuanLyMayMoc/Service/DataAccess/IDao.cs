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

      //  public  ObservableCollection<Loi> GetAllLoi();

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
        public  int TimSttLonNhat(string maduan);

        public void InsertTaskToDaTaBaseTemp(Task newTask);

        public void SaveProjectWithDifferentName(Project projectInsert);

        public void DeleteProject(Project selectedProject);
    }


}
