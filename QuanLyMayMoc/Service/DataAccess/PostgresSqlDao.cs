using Npgsql;
using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace QuanLyMayMoc
{
    public class PostgresSqlDao : IDao
    {
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";

        public PostgresSqlDao()
        {
        }

        // Lấy danh sách tất cả các nhân viên
        public ObservableCollection<Employee> GetEmployees()
        {
           // string dbString = "SELECT mahieu, tenlinhkien, giaban FROM linhkien";
          //  DataTable InforMay = ExecuteQuery(dbString);
            string query = @"
        SELECT 
            hoten, manv,  gioitinh, ngaykyhopdong, ngaysinh, 
            trangthai,  dantoc, cccd, phongban, email, 
            sdt, diachi,maduan
        FROM nhanvien";

            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                HoTen = reader.IsDBNull(0) ? null : reader.GetString(0),
                                MaNhanVien = reader.IsDBNull(1) ? null : reader.GetString(1),
                                //ChucVu = reader.IsDBNull(2) ? null : reader.GetString(2),
                                GioiTinh = reader.IsDBNull(2) ? null : reader.GetString(2),
                                NgayKyHD = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                                NgaySinh = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                                TrangThai = reader.IsDBNull(5) ? null : reader.GetString(5),
                                AnhDaiDien ="Assets/couple.PNG", /*reader.IsDBNull(7) ? null : reader.GetString(7),*/
                                DanToc = reader.IsDBNull(6) ? null : reader.GetString(6),       // Cột 6: DanToc
                                CCCD = reader.IsDBNull(7) ? null : reader.GetString(7),         // Cột 7: CCCD
                                PhongBan = reader.IsDBNull(8) ? null : reader.GetString(8),     // Cột 8: PhongBan
                                Email = reader.IsDBNull(9) ? null : reader.GetString(9),        // Cột 9: Email
                                SoDienThoai = reader.IsDBNull(10) ? null : reader.GetString(10),// Cột 10: SoDienThoai
                                DiaChi = reader.IsDBNull(11) ? null : reader.GetString(11),     // Cột 11: DiaChi
                                MaDuAn = reader.IsDBNull(12) ? null : reader.GetString(12)      // Cột 12: MaDuAn

                            });
                        }
                    }
                }
            }

            return employees;
        }


        // Lấy danh sách tất cả các công việc
        public ObservableCollection<Task> GetTasks()
        {
            string query = @"
        SELECT 
    stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
    manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
    maloi, tenloi, soluongloi, phidichvu, ghichu, maduan
FROM congviec;";

            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                Stt = reader.IsDBNull(0) ? 0 : reader.GetInt32(0), // STT
                                NgayThucHien = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1), // NgayThucHien
                                HoTenKH = reader.IsDBNull(2) ? null : reader.GetString(2), // HoTenKH
                                SDT = reader.IsDBNull(3) ? null : reader.GetString(3), // SDT
                                DiaChi = reader.IsDBNull(4) ? null : reader.GetString(4), // DiaChi
                                TenDichVu = reader.IsDBNull(5) ? null : reader.GetString(5), // TenDichVu
                                MaNV = reader.IsDBNull(6) ? null : reader.GetString(6), // MaNV
                                TenNV = reader.IsDBNull(7) ? null : reader.GetString(7), // TenNV
                                MaLK = reader.IsDBNull(8) ? null : reader.GetString(8), // MaLinhKien
                                TenLK = reader.IsDBNull(9) ? null : reader.GetString(9), // TenLinhKien
                                SoLuongLK = reader.IsDBNull(10) ? 0 : reader.GetInt32(10), // SoLuongLinhKien
                                MaLoi = reader.IsDBNull(11) ? null : reader.GetString(11), // MaLoi
                                TenLoi = reader.IsDBNull(12) ? null : reader.GetString(12), // TenLoi
                                SoLuongLoi = reader.IsDBNull(13) ? 0 : reader.GetInt32(13), // SoLuongLoi
                                PhiDichVu = reader.IsDBNull(14) ? 0 : reader.GetInt32(14), // PhiDichVu
                                GhiChu = reader.IsDBNull(15) ? null : reader.GetString(15), // GhiChu
                                MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16) // MaDuAn
                            });
                        }
                    }

                }
            }

            return tasks;
        }


        // Lấy danh sách công việc theo ngày
        public ObservableCollection<Task> GetTasks(DateTime date)
        {
            string query = "SELECT id, name, ngaythuchien FROM tasks WHERE DATE(ngaythuchien) = @date";
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            //using (var connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("date", date);

            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                tasks.Add(new Task
            //                {
            //                    Id = reader.GetInt32(0),
            //                    Name = reader.GetString(1),
            //                    NgayThucHien = reader.GetDateTime(2)
            //                });
            //            }
            //        }
            //    }
            //}

            return tasks;
        }

        // Thêm công việc mới
        public void AddTask(Task newTask)
        {
            string query = "INSERT INTO tasks (name, ngaythuchien) VALUES (@name, @ngaythuchien)";

            //using (var connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand(query, connection))
            //    {
            //        command.Parameters.AddWithValue("name", newTask.Name);
            //        command.Parameters.AddWithValue("ngaythuchien", newTask.NgayThucHien);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        // Lấy danh sách tất cả các dự án
        public ObservableCollection<Project> GetProjects()
        {
            string query = "SELECT id, name, description FROM projects";
            ObservableCollection<Project> projects = new ObservableCollection<Project>();

            //using (var connection = new NpgsqlConnection(connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand(query, connection))
            //    {
            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                projects.Add(new Project
            //                {
            //                    ID = reader.GetInt32(0),
            //                    Name = reader.GetString(1),
            //                    TimeCreate = reader.GetString(2)
            //                });
            //            }
            //        }
            //    }
            //}

            return projects;
        }

        // Xóa tất cả các dòng trong bảng công việc
        public void DeleteAllTasks()
        {
            string query = "DELETE FROM tasks";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Xóa công việc theo ID
        public void DeleteTaskById(int taskId)
        {
            string query = "DELETE FROM tasks WHERE id = @id";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("id", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
