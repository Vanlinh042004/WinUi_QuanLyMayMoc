using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ObservableCollection<Task> _filteredTasks;
        public ObservableCollection<Task> FilteredTasks
        {
            get => _filteredTasks;
            set
            {
                _filteredTasks = value;
                OnPropertyChanged(nameof(FilteredTasks));
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

        public void LoadData(DateTime ngay)
        {
            IDao _dao = new MockDao();
            var items = _dao.GetTasks(Ngay);
            Tasks = new ObservableCollection<Task>(items);


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
