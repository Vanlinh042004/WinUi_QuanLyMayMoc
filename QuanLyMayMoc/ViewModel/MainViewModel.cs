using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";


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

        //link kien cho lĩnh
        private Linhkien _currentSelectedLinhkien;
        public Linhkien CurrentSelectedLinhkien
        {
            get => _currentSelectedLinhkien;
            set
            {
                _currentSelectedLinhkien = value;
                OnPropertyChanged(nameof(CurrentSelectedLinhkien)); // Nếu ViewModel hỗ trợ INotifyPropertyChanged
            }
        }
        //linhkien

        //Lỗi Lĩnh
        private Loisp _currentSelectedLoisp;
        public Loisp CurrentSelectedLoisp
        {
            get => _currentSelectedLoisp;
            set
            {
                _currentSelectedLoisp = value;
                OnPropertyChanged(nameof(CurrentSelectedLoisp)); // Nếu ViewModel hỗ trợ INotifyPropertyChanged
            }
        }
        //lỗi

        public ObservableCollection<Employee> Employees
        {
            get; set;
        }
        public ObservableCollection<Task> Tasks
        {
            get; set;
        }

        public ObservableCollection<Linhkien> Listlinhkien
        {
            get; set;
        }
        public ObservableCollection<Loisp> ListLoi
        {
            get; set;
        }

       // public DateTime Ngay { get; set; } = DateTime.MinValue;
        public XamlRoot XamlRoot { get; private set; }

        public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Employees = _dao.GetEmployees();
            Tasks = _dao.GetTasks();
            Listlinhkien = _dao.GetAllLinhKien();
           // ListLoi = _dao.GetAllLoi();


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
           
            Tasks.Clear();

           
            var allTasks = _dao.GetTasksFromTemp();

           
            var sortedTasks = allTasks.OrderBy(task => task.Stt);

           
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }


        public List<string> GetCustomerNames(string query)
        {

            return _dao.GetCustomerNamesFromDatabase(query);
        }



        public  void RemoveSelectedTask()
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

             
                _dao.UpdateTask(updatedTask,newTask);

              
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
            _dao.InsertAllDataFromTemp( projectID);

        }

        public void InsertToEmployees(Employee newEmployee)
        {
            _dao.InsertEmployeeToDaTaBase(newEmployee);
        }

        public int TimSttLonNhat(string maduan)
        {
            return _dao.TimSttLonNhat(maduan);
        }

        public void InsertTaskToDaTaBaseTemp(Task newTask)
        {
            _dao.InsertTaskToDaTaBaseTemp(newTask);
        }
        public void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan)
        {
            _dao.InsertLinhKienToDaTaBaseTemp(newLinhKien,mahieuduan);
        }
        
        public void DeleteAllTask(string maDuAn)
        {
            _dao.DeleteAllTasks(maDuAn);
        }


        //Linhkien

        public void UpdateSelectedLinhkien(Linhkien newLinhkien)
        {
            if (CurrentSelectedLinhkien != null)
            {
                var updatedLinhkien = CurrentSelectedLinhkien;

                _dao.UpdateLinhkien(updatedLinhkien, newLinhkien);

                var linhkienIndex = Listlinhkien.IndexOf(updatedLinhkien);

                if (linhkienIndex >= 0)
                {
                    Listlinhkien[linhkienIndex] = updatedLinhkien;
                }
            }
            CurrentSelectedLinhkien = null;
        }

        public void UpdateSelectedLoisp(Loisp newLoisp)
        {
            if (CurrentSelectedLoisp != null)
            {
                var updatedLoisp = CurrentSelectedLoisp;

                _dao.UpdateLoisp(updatedLoisp, newLoisp);

                var loispIndex = ListLoi.IndexOf(updatedLoisp);

                if (loispIndex >= 0)
                {
                    ListLoi[loispIndex] = updatedLoisp;
                }
            }
            CurrentSelectedLoisp = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



      
    }
}
