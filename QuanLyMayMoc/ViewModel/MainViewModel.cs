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




        public ObservableCollection<Employee> Employees
        {
            get; set;
        }
        public ObservableCollection<Task> Tasks
        {
            get; set;
        }


        public DateTime Ngay { get; set; } = DateTime.MinValue;
        public XamlRoot XamlRoot { get; private set; }

        public MainViewModel()
        {
            _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;
            Employees = _dao.GetEmployees();
            Tasks = _dao.GetTasks();
            linhkien = _dao.GetAllLinhKien();
            ListLoi = _dao.GetAllLoi();




        }

        public void loadNewData()
        {
            Employees = _dao.GetEmployees();
            Tasks = _dao.GetTasks();
            linhkien = _dao.GetAllLinhKien();
            ListLoi = _dao.GetAllLoi();
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
            foreach (var task in allTasks)
            {
                Tasks.Add(task);
            }
        }

        public List<string> GetCustomerNames(string query)
        {

            return _dao.GetCustomerNamesFromDatabase(query);
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

        public void InsertTaskToDatabaseTemp(Task newTask)
        {
            _dao.InsertTaskToDatabaseTemp(newTask);
        }
        public ObservableCollection<Linhkien> linhkien
        {
            get; set;
        }
        public void SaveToLinhKienTam()
        {
            _dao.SaveToLinhKienTam();
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
        public ObservableCollection<Loisp> ListLoi
        {
            get; set;
        }

        public void SaveToLoiTam()
        {
            _dao.SaveToLoiTam();
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

        public void SaveProjectWithDifferentName(Project project, string oldProjectID)
        {
            _dao.SaveProjectWithDifferentName(project, oldProjectID);
        }

        public void DeleteProject(Project project)
        {
            _dao.DeleteProject(project);
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




      
    }

        //public void SaveProjectWithDifferentName(Project project)
        //{
        //    _dao.SaveProjectWithDifferentName(project);
        //}
    } 


