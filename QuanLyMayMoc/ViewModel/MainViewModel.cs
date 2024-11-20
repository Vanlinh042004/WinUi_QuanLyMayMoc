using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                OnPropertyChanged(nameof(CurrentSelectedTask)); // Nếu ViewModel hỗ trợ INotifyPropertyChanged
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


        public DateTime Ngay { get; set; } = DateTime.MinValue;


        public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Employees = _dao.GetEmployees();
            Tasks = _dao.GetTasks();
            //LoadData(Ngay);

        }

        public void LoadDataFilter(DateTime ngaythuchien)
        {
           
            
                Tasks.Clear();
            

            var filteredTasks = _dao.GetTasksFromTemp(ngaythuchien);
            foreach (var task in filteredTasks)
            {
                Tasks.Add(task);
            }
        }

        public void LoadDataFilter()
        {
            
            
                Tasks.Clear();
            

            var allTasks = _dao.GetTasksFromTemp();
            foreach (var task in allTasks)
            {
                Tasks.Add(task);
            }
        }




        public void RemoveSelectedTask()
        {
            if (CurrentSelectedTask != null)
            {
                Tasks.Remove(CurrentSelectedTask);
                CurrentSelectedTask = null; // Clear selection
            }
        }
        public void RemoveAllTask()
        {

            Tasks.Clear();

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
