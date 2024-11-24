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

        public void LoadDataFilter(DateTime ngaythuchien ,string keyword)
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



        public async void RemoveSelectedTask()
        {
            if (CurrentSelectedTask != null)
            {
                Tasks.Remove(CurrentSelectedTask);
               

                var selectedTask = CurrentSelectedTask; // Lưu lại dữ liệu trước khi xóa


               
                if (selectedTask != null)
                {
                    try
                    {
                        using (var connection = new NpgsqlConnection(connectionString))
                        {
                            await connection.OpenAsync();

                          
                            using (var transaction = await connection.BeginTransactionAsync())
                            {
                                try
                                {
                                   
                                    string deleteQueryTemp = "DELETE FROM congviectamthoi WHERE macvduan = @macvduan";
                                    using (var command = new NpgsqlCommand(deleteQueryTemp, connection))
                                    {
                                        command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                                        await command.ExecuteNonQueryAsync();
                                    }
                                    string deleteQuery = "DELETE FROM congviec WHERE macvduan = @macvduan";
                                    using (var command = new NpgsqlCommand(deleteQuery, connection))
                                    {
                                        command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                                        await command.ExecuteNonQueryAsync();
                                    }

                                  
                                    string updateQueryTemp = @"
                                UPDATE congviectamthoi
                                SET stt = stt - 1
                                WHERE maduan = @maduan AND stt > @stt";
                                    using (var command = new NpgsqlCommand(updateQueryTemp, connection))
                                    {
                                        command.Parameters.AddWithValue("@maduan", selectedTask.MaDuAn);
                                        command.Parameters.AddWithValue("@stt", selectedTask.Stt);
                                        await command.ExecuteNonQueryAsync();
                                    }

                                    string updateQuery = @"
                                UPDATE congviec
                                SET stt = stt - 1
                                WHERE maduan = @maduan AND stt > @stt";
                                    using (var command = new NpgsqlCommand(updateQuery, connection))
                                    {
                                        command.Parameters.AddWithValue("@maduan", selectedTask.MaDuAn);
                                        command.Parameters.AddWithValue("@stt", selectedTask.Stt);
                                        await command.ExecuteNonQueryAsync();
                                    }

                                  
                                    await transaction.CommitAsync();
                                }
                                catch
                                {
                                   
                                    await transaction.RollbackAsync();
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Lỗi",
                            Content = $"Không thể xóa dòng trong cơ sở dữ liệu. Lỗi: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };

                        await errorDialog.ShowAsync();
                        return; 
                    }
                }
            }
            CurrentSelectedTask = null; 
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



        public ObservableCollection<Linhkien> linhkien
        {
            get; set;
        }
        public ObservableCollection<Loisp> ListLoi
        {
            get; set;
        }
    }
}
