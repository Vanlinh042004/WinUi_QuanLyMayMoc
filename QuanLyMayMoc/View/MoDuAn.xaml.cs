using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.Service;
using QuanLyMayMoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static QuanLyMayMoc.AppData;




// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoDuAn : Page
    {
        private ObservableCollection<Project> projects;
        private MainViewModel mainViewModel = new MainViewModel();

        public MoDuAn()
        {
            this.InitializeComponent();

            //loadDuAnFromDB();
            projects = mainViewModel.getProjects();
            showDuAn();
        }
        public string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";

        private MainPage _mainPage;

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainPage = e.Parameter as MainPage;
        }
        //public class DuAn
        //{
        //    public string maDuAn { get; set; }
        //    public string tenDuAn { get; set; }
        //    public string ngayThucHien { get; set; }
        //}

        //public class DuAnList
        //{
        //    public static List<DuAn> Items { get; set; } = new List<DuAn>();
        //}

        public void showDuAn()
        {
            // show data stored in Items to MoDuAn page
            // with each item in Items, create a new line in a grid
            // and show the information of the item in the grid
            foreach (Project project in projects)
            {
                Button button = new Button();
                button.Content = project.TimeCreate + " " + project.Name;
                DataStackPanel.Children.Add(button);
                button.Click += (sender, e) =>
                {

                    _mainPage.buttonToggling();

                    AppData.ProjectID = project.ID;
                    AppData.ProjectName = project.Name;
                    AppData.ProjectTimeCreate = project.TimeCreate;

                    mainViewModel.loadNewData();

                    // when a button is clicked, navigate to another page
                    // and pass the information of the item to the page
                    this.Frame.Navigate(typeof(DichVuTheoThang), project);
                };
            }
        }


        //public void loadDuAnFromDB()
        //{
        //    // Load data from database
        //    using (var connection = new NpgsqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        using (var cmd = new NpgsqlCommand("SELECT * FROM duan", connection))
        //        {
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string maDuAn = reader.GetString(0);
        //                    string tenDuAn = reader.GetString(1);
        //                    // get the timestamp without time zone
        //                    string ngayThucHien = reader.GetDateTime(2).ToString();
        //                    DuAn duan = new DuAn { maDuAn = maDuAn, tenDuAn = tenDuAn, ngayThucHien = ngayThucHien };
        //                    DuAnList.Items.Add(duan);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
