using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace QuanLyMayMoc.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MoDuAn : Page
    {
        public MoDuAn()
        {
            this.InitializeComponent();
            loadDuAnFromDB();
            showDuAn();
        }
        public string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=postgres";
        
        public class DuAn
        {
            public string maDuAn { get; set; }
            public string tenDuAn { get; set; }
            public string ngayThucHien { get; set; }
        }

        public class DuAnList
        {
            public static List<DuAn> Items { get; set; } = new List<DuAn>();
        }

        public void showDuAn()
        {
            // show data stored in Items to MoDuAn page
            // with each item in Items, create a new line in a grid
            // and show the information of the item in the grid
            foreach (DuAn duan in DuAnList.Items)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = duan.maDuAn + " " + duan.tenDuAn + " " + duan.ngayThucHien;
                DataStackPanel.Children.Add(textBlock);
            }

        }

        public void loadDuAnFromDB()
        {
            // Load data from database
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand("SELECT * FROM duan", connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string maDuAn = reader.GetString(0);
                            string tenDuAn = reader.GetString(1);
                            // get the timestamp without time zone
                            string ngayThucHien = reader.GetDateTime(2).ToString();
                            DuAn duan = new DuAn { maDuAn = maDuAn, tenDuAn = tenDuAn, ngayThucHien = ngayThucHien };
                            DuAnList.Items.Add(duan);
                        }
                    }
                }
            }
        }
    }
}
