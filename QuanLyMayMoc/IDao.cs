﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public interface IDao
    {
        ObservableCollection<Employee> GetEmployees();
        public ObservableCollection<Service> GetServices(DateTime date);
        public ObservableCollection<Service> GetServices();
        
        
        // ObservableCollection<Service> GetServicesByDate(DateTime date);
        // Các phương thức khác...



    }
}
