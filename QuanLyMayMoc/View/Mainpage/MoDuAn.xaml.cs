using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.Service;
using QuanLyMayMoc.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        // ...

        public MoDuAn()
        {
            this.InitializeComponent();

            //loadDuAnFromDB();
            projects = mainViewModel.getProjects();
            // print in the UI that let the user know there is no project
            if (projects.Count == 0)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Không có dự án nào";
                DataStackPanel.Children.Add(textBlock);
            }
            showDuAn();
            mainViewModel.ClearAllTempData();
            this.Loaded += (sender, args) =>
            {
                MainPage.ChangeHeaderTextBlock("Mở dự án");
            };
        }
      

        private MainPage _mainPage;

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainPage = e.Parameter as MainPage;
        }
       

        public void showDuAn()
        {
            // show data stored in Items to MoDuAn page
            // with each item in Items, create a new line in a grid
            // and show the information of the item in the grid

            foreach (Project project in projects)
            {
                Button button = new Button();
                DataStackPanel.Children.Add(button);
                button.Margin = new Thickness(0, 10, 0, 0);
                button.HorizontalAlignment = HorizontalAlignment.Stretch;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.RequestedTheme = ElementTheme.Light;

                // Create a Grid to host the TextBlocks
                Grid grid = new Grid();
                grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                grid.Padding = new Thickness(10, 10, 10, 10); // Add padding to the left

                // Define two columns in the Grid
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(200) }); // Fixed width for time
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }); // Auto stretch for name

                // Create TextBlock for time
                TextBlock timeTextBlock = new TextBlock();
                timeTextBlock.Text = project.TimeCreate.ToString("g");
                timeTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                timeTextBlock.VerticalAlignment = VerticalAlignment.Center;
                timeTextBlock.FontSize = 16; // Increase the font size
                Grid.SetColumn(timeTextBlock, 0); // Set to first column

                // Create TextBlock for name
                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock.Text = project.Name;
                nameTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                nameTextBlock.FontWeight = FontWeights.Bold;  // Make the text bold
                nameTextBlock.FontSize = 16; // Increase the font size
                Grid.SetColumn(nameTextBlock, 1); // Set to second column

                // Add TextBlocks to Grid
                grid.Children.Add(timeTextBlock);
                grid.Children.Add(nameTextBlock);

                button.Content = grid;

                button.Click += (sender, e) =>
                {
                    AppData.isEnableFunctionButtion = true;
                    //_mainPage.buttonToggling();

                    AppData.ProjectID = project.ID;
                    AppData.ProjectName = project.Name;
                    AppData.ProjectTimeCreate = project.TimeCreate;

                   // mainViewModel.loadNewData();

                    // when a button is clicked, navigate to another page
                    // and pass the information of the item to the page
                    this.Frame.Navigate(typeof(DichVuTheoThang), project);

                    UpdateShellWindowTitle(AppData.ProjectName);
                };
            }
        }
        private void UpdateShellWindowTitle(string title)
        {
            // Access the ShellWindow instance and update the title
            if (App.MainShellWindow != null)
            {
                App.MainShellWindow.UpdateTitle(title + " - Quản lý máy móc");
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
