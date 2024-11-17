using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyMayMoc.Model;

namespace QuanLyMayMoc
{
    public interface IDao
    {
        ObservableCollection<Employee> GetEmployees();
        public ObservableCollection<Task> GetTasks(DateTime date);
        public ObservableCollection<Task> GetTasks();
        public ObservableCollection<Project> GetProjects();
        public void AddTask(Task newTask);
    }


}
