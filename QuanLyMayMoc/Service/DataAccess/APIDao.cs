using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing3D;

namespace QuanLyMayMoc.Service.DataAccess
{
    public class APIDao: IDao
    {
        private static HttpClient sharedClient = new()
        {
            //BaseAddress = new Uri("https://backend-windows-programming.vercel.app"),
            BaseAddress = new Uri("http://localhost:3000"),
        };

        public ObservableCollection<Employee> GetEmployees()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/nhanvien/all?maduan=" + AppData.ProjectID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ObservableCollection<Employee>>(jsonString);
            }
            return new ObservableCollection<Employee>();
        }

        public ObservableCollection<Linhkien> GetAllLinhKien()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/linhkien/all?maduan=" + AppData.ProjectID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ObservableCollection<Linhkien>>(jsonString);
            }
            return new ObservableCollection<Linhkien>();
        }

        public ObservableCollection<Task> GetTasks()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/congviec/all?maduan=" + AppData.ProjectID + "&issaved=1").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ObservableCollection<Task>>(jsonString);
            }
            return new ObservableCollection<Task>();
        }
        public ObservableCollection<Loisp> GetAllLoi()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/loi/all?maduan=" + AppData.ProjectID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ObservableCollection<Loisp>>(jsonString);
            }
            return new ObservableCollection<Loisp>();
        }

        public ObservableCollection<Project> GetProjects()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/duan").Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var jsonObject = JsonConvert.DeserializeObject<ObservableCollection<Project>>(jsonString);
                return jsonObject;
            }
            return new ObservableCollection<Project>();
        }

        public ObservableCollection<Task> GetTasksFromTemp()
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/congviec/all?maduan=" + AppData.ProjectID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ObservableCollection<Task>>(jsonString);
            }
            return new ObservableCollection<Task>();
        }

        public ObservableCollection<Task> GetTasksFromTemp(DateTime? ngaythuchien, string keyword)
        {
            throw new NotImplementedException();
        }

        public List<string> GetCustomerNamesFromDatabase(string query)
        {
            using HttpResponseMessage response = sharedClient.GetAsync("/khachhang/allNames?maduan=" + AppData.ProjectID).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                List<String> result = JsonConvert.DeserializeObject<List<String>>(jsonString);
                return result;
            }
            return new List<string>();
        }

        public void DeleteTask(Task selectedTask)
        {
            //using HttpResponseMessage response = sharedClient.GetAsync("/congviec/delete?macvduan=" + selectedTask.MaCVDuAn).Result;


            // using post method
            var content = new StringContent(JsonConvert.SerializeObject(selectedTask), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/congviec/deletetask", content).Result;
            if (response.IsSuccessStatusCode) {
                Console.WriteLine("Delete task successfully");
            }
            else {
                Console.WriteLine("Delete task failed");
            }
        }

        public void InsertProject(Project projectInsert)
        {
            var content = new StringContent(JsonConvert.SerializeObject(projectInsert), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/duan/insertproject", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Insert project successfully");
            }
            else
            {
                Console.WriteLine("Insert project failed");
            }
        }

        public void InsertProjectTemp(Project projectInsert)
        {
            var content = new StringContent(JsonConvert.SerializeObject(projectInsert), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/duan/insertprojectnotsaved", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Insert project successfully");
            }
            else
            {
                Console.WriteLine("Insert project failed");
            }
        }

        public void InsertAllDataFromTemp(string projectID)
        {
            var data = new { projectID }; // Wrap the string in an object
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = sharedClient.PostAsync("/duan/insertallfromtemp", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Insert all data from temp successfully");
            }
            else
            {
                Console.WriteLine("Insert all data from temp failed");
            }
        }


        public void InsertEmployeeToDatabase(Employee newEmployee)
        {
            var content = new StringContent(JsonConvert.SerializeObject(newEmployee), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/nhanvien/insert", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Insert employee successfully");
            }
            else
            {
                Console.WriteLine("Insert employee failed");
            }
        }

        public int TimSttLonNhat(string maduan)
        {
            int maxStt = 0;
            using HttpResponseMessage response = sharedClient.GetAsync("/congviec/maxstt?maduan=" + maduan).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(jsonString); // Log the JSON string

                var jsonToken = JToken.Parse(jsonString);
                if (jsonToken is JArray jsonArray)
                {
                    if (jsonArray.Count > 0)
                    {
                        maxStt = (int)jsonArray[0]["STT"];
                    }
                }
                else if (jsonToken is JObject jsonObject)
                {
                    if (jsonObject["stt"] != null)
                    {
                        maxStt = (int)jsonObject["stt"];
                    }

                }
            }
            return maxStt;
        }


        public void InsertTaskToDatabaseTemp(Task newTask)
        {
            var content = new StringContent(JsonConvert.SerializeObject(newTask), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/congviec/inserttask", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Insert task successfully");
            }
            else
            {
                Console.WriteLine("Insert task failed");
            }
        }

        public void SaveProjectWithDifferentName(Project projectInsert, string oldProjectID)
        {
            var data = new
            {
                Project = projectInsert,
                OldProjectID = oldProjectID
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/duan/saveprojectwithdifferentname", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Save project with different name successfully");
            }
            else
            {
                Console.WriteLine("Save project with different name failed");
            }
        }

        public void DeleteProject(Project selectedProject)
        {
            var content = new StringContent(JsonConvert.SerializeObject(selectedProject), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = sharedClient.PostAsync("/duan/deleteproject", content).Result;
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Delete project successfully");
            }
            else
            {
                Console.WriteLine("Delete project failed");
            }
        }
    }
}
