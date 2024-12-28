using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc
{
    public static class AppData
    {
        public static string ProjectID { get; set; }
        public static string ProjectName { get; set; }
        public static DateTime ProjectTimeCreate { get; set; }

        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string Name { get; set; }
        public static int UserId { get; set; }

        public static bool isEnableFunctionButtion { get; set; }
    }

}
