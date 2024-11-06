using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{


    public class MainViewModel
{
      

        private Service _currentSelectedService;
        public Service CurrentSelectedService
        {
            get => _currentSelectedService;
            set
            {
                _currentSelectedService = value;
                OnPropertyChanged(nameof(CurrentSelectedService)); // Nếu ViewModel hỗ trợ INotifyPropertyChanged
            }
        }

        private ObservableCollection<Service> _filteredServices;
        public ObservableCollection<Service> FilteredServices
        {
            get => _filteredServices;
            set
            {
                _filteredServices = value;
                OnPropertyChanged(nameof(FilteredServices));
            }
        }


        public ObservableCollection<Employee> Employees
    {
        get; set;
    }
    public ObservableCollection<Service> Services
        {
            get; set;
        }


        public DateTime Ngay { get; set; } = DateTime.MinValue;


        public MainViewModel()
    {
        IDao dao = new MockDao();
        Employees = dao.GetEmployees();
        Services=dao.GetServices();
        //LoadData(Ngay);
    
        }

        public void LoadData(DateTime ngay)
        {
            IDao _dao=new MockDao();
            var items = _dao.GetServices( Ngay);
            Services = new ObservableCollection<Service>(items);
           

        }
       

        public void RemoveSelectedService()
        {
            if (CurrentSelectedService != null)
            {
                Services.Remove(CurrentSelectedService);
                CurrentSelectedService = null; // Clear selection
            }
        }
        public void RemoveAllService()
        {

            Services.Clear();
               
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
