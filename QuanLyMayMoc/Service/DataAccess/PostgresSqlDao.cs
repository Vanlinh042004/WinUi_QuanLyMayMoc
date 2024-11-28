﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Npgsql;
using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using static QuanLyMayMoc.View.MoDuAn;

namespace QuanLyMayMoc
{
    public class PostgresSqlDao : IDao
    {
        private string connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Password=1234;Database=machine";

        public XamlRoot XamlRoot { get; private set; }

        public PostgresSqlDao()
        {
        }

        // Lấy danh sách tất cả các nhân viên
        public ObservableCollection<Employee> GetEmployees()
        {

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
                                AnhDaiDien = "Assets/couple.PNG", /*reader.IsDBNull(7) ? null : reader.GetString(7),*/
                                //DanToc = reader.IsDBNull(6) ? null : reader.GetString(6),       // Cột 6: DanToc
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
                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan,macvduan
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
                                MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16),// MaDuAn
                                MaCVDuAn = reader.IsDBNull(17) ? null : reader.GetString(17) // MacvDuAn

                            });
                        }
                    }

                }
            }

            return tasks;
        }
        public ObservableCollection<Task> GetTasksFromTemp()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();

            string query = @"
                    SELECT 
                    stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                    manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                    maloi, tenloi, soluongloi, phidichvu, ghichu, maduan,macvduan
                    FROM congviec
                    WHERE maduan=@maduan;";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maduan", AppData.ProjectID);
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
                                MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16),// MaDuAn
                                MaCVDuAn = reader.IsDBNull(17) ? null : reader.GetString(17) // MacvDuAn

                            });
                        }
                    }
                    connection.Close();
                }
            }

            string querytemp = @"
                    SELECT 
                    stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                    manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                    maloi, tenloi, soluongloi, phidichvu, ghichu, maduan,macvduan
                    FROM congviectamthoi;";



            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(querytemp, connection))
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
                                MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16),// MaDuAn
                                MaCVDuAn = reader.IsDBNull(17) ? null : reader.GetString(17) // MacvDuAn

                            });
                        }
                    }

                }
                connection.Close();
            }




            return tasks;
        }


        // Lấy danh sách công việc theo ngày
        public ObservableCollection<Task> GetTasksFromTemp(DateTime? ngaythuchien, string tennv)
        {
            var tasks = new ObservableCollection<Task>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Biến lưu câu truy vấn SQL
                    string query = "";

                    // Xây dựng câu truy vấn dựa trên các điều kiện
                    if (ngaythuchien.HasValue && !string.IsNullOrEmpty(tennv) && ngaythuchien != DateTime.MinValue)
                    {
                        // Cả hai điều kiện đều có giá trị
                        query = @"
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviec
                            WHERE ngaythuchien = @ngaythuchien AND tennv ILIKE @tennv AND maduan = @maduan
                            UNION ALL
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviectamthoi
                            WHERE ngaythuchien = @ngaythuchien AND tennv ILIKE @tennv AND maduan = @maduan";
                    }
                    else if (ngaythuchien.HasValue && string.IsNullOrEmpty(tennv))
                    {
                        // Chỉ có ngày thực hiện
                        query = @"
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviec
                            WHERE ngaythuchien = @ngaythuchien AND maduan = @maduan
                            UNION ALL
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviectamthoi
                            WHERE ngaythuchien = @ngaythuchien AND maduan = @maduan";
                    }
                    else if (!string.IsNullOrEmpty(tennv) && (ngaythuchien == DateTime.MinValue))
                    {
                        // Có cả tên nhân viên và ngày thực hiện (ngaythuchien hợp lệ)
                        query = @"
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviec
                            WHERE  tennv ILIKE @tennv AND maduan = @maduan
                            UNION ALL
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviectamthoi
                            WHERE  tennv ILIKE @tennv AND maduan = @maduan";
                    }
                    else if (string.IsNullOrEmpty(tennv) && (ngaythuchien == DateTime.MinValue))
                    {
                        // Chỉ có tên nhân viên
                        query = @"
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviec
              
                            UNION ALL
                            SELECT 
                                stt, ngaythuchien, hotenkh, sdt, diachi, tendichvu, 
                                manv, tennv, malinhkien, tenlinhkien, soluonglinhkien, 
                                maloi, tenloi, soluongloi, phidichvu, ghichu, maduan, macvduan
                            FROM congviectamthoi
                          ";
                    }


                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        // Thêm tham số vào câu truy vấn
                        if (ngaythuchien.HasValue)
                        {
                            command.Parameters.AddWithValue("@ngaythuchien", ngaythuchien);
                        }

                        if (!string.IsNullOrEmpty(tennv))
                        {
                            command.Parameters.AddWithValue("@tennv", $"%{tennv}%");
                        }

                        command.Parameters.AddWithValue("@maduan", AppData.ProjectID);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new Task
                                {
                                    Stt = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    NgayThucHien = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1),
                                    HoTenKH = reader.IsDBNull(2) ? null : reader.GetString(2),
                                    SDT = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    DiaChi = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    TenDichVu = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    MaNV = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    TenNV = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    MaLK = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    TenLK = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    SoLuongLK = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                    MaLoi = reader.IsDBNull(11) ? null : reader.GetString(11),
                                    TenLoi = reader.IsDBNull(12) ? null : reader.GetString(12),
                                    SoLuongLoi = reader.IsDBNull(13) ? 0 : reader.GetInt32(13),
                                    PhiDichVu = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                                    GhiChu = reader.IsDBNull(15) ? null : reader.GetString(15),
                                    MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16),
                                    MaCVDuAn = reader.IsDBNull(17) ? null : reader.GetString(17)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi tải dữ liệu: {ex.Message}");
            }

            return tasks;
        }

        // Lấy danh sách tất cả các dự án
        public ObservableCollection<Project> GetProjects()
        {
            ObservableCollection<Project> projects = new ObservableCollection<Project>();
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
                            DateTime dateTime = reader.GetDateTime(2);
                            Project duan = new Project { ID = maDuAn, Name = tenDuAn, TimeCreate = dateTime };
                            projects.Add(duan);
                        }
                    }
                }
            }

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

        public List<string> GetCustomerNamesFromDatabase(string query)
        {
            var customerNames = new List<string>(); // Danh sách tên khách hàng sẽ trả về

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open(); // Mở kết nối

                    // Truy vấn SQL từ bảng congviectamthoi
                    string sqlQueryTemp = @"
                                            SELECT DISTINCT hotenkh 
                                            FROM congviectamthoi
                                            WHERE hotenkh ILIKE @query
                                            LIMIT 10";

                    using (var command = new NpgsqlCommand(sqlQueryTemp, connection))
                    {
                        command.Parameters.AddWithValue("@query", $"%{query}%");

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerNames.Add(reader.GetString(0));
                            }
                        }
                    }

                    // Truy vấn SQL từ bảng congviec
                    string sqlQuery = @"
            SELECT DISTINCT hotenkh 
            FROM congviec
            WHERE maduan = @maduan AND hotenkh ILIKE @query
            LIMIT 10";

                    using (var command = new NpgsqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@query", $"%{query}%");
                        command.Parameters.AddWithValue("@maduan", AppData.ProjectID);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customerNames.Add(reader.GetString(0));
                            }
                        }
                    }

                    connection.Close(); // Đóng kết nối
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Lỗi khi lấy dữ liệu: {ex.Message}");
            }

            return customerNames; // Trả về danh sách tên khách hàng
        }

        public async void DeleteTask(Task selectedTask)
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

        public async void InsertProject(Project projectInsert)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem dự án đã tồn tại hay chưa
                    string checkDuAnQuery = "SELECT COUNT(1) FROM duan WHERE maduan = @maDuAn";
                    long duAnExists;

                    using (var command = new NpgsqlCommand(checkDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", projectInsert.ID);
                        duAnExists = (long)await command.ExecuteScalarAsync();
                    }

                    // Nếu cần dùng kiểu `int` ở các đoạn code khác, bạn có thể chuyển đổi về `int` an toàn:
                    int duAnExistsAsInt = (int)duAnExists;

                    if (duAnExists == 0)
                    {
                        // Nếu dự án chưa tồn tại, thêm vào bảng `duan`
                        string insertDuanQuery = "INSERT INTO duan (maduan, tenduan, ngaythuchien) VALUES (@maDuAn, @tenDuAn, @ngayThucHien)";
                        using (var command = new NpgsqlCommand(insertDuanQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", projectInsert.ID);
                            command.Parameters.AddWithValue("@tenDuAn", projectInsert.Name);
                            command.Parameters.AddWithValue("@ngayThucHien", projectInsert.TimeCreate);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi lưu dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        public async void InsertProjectTemp(Project projectInsert)
        {

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Nếu dự án chưa tồn tại, thêm vào bảng `duan`
                string insertDuanQuery = "INSERT INTO duan_tam (maduan, tenduan, ngaythuchien) VALUES (@maDuAn, @tenDuAn, @ngayThucHien)";
                using (var command = new NpgsqlCommand(insertDuanQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectInsert.ID);
                    command.Parameters.AddWithValue("@tenDuAn", projectInsert.Name);
                    command.Parameters.AddWithValue("@ngayThucHien", projectInsert.TimeCreate);
                    await command.ExecuteNonQueryAsync();
                }
            }

        }

        public ObservableCollection<Linhkien> GetAllLinhKien()
        {
            string query = @"SELECT mahieu, tenlinhkien, giaban
                             FROM linhkien";

            ObservableCollection<Linhkien> linhkiens = new ObservableCollection<Linhkien>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            linhkiens.Add(new Linhkien
                            {
                                MaSanPham = reader.IsDBNull(0) ? null : reader.GetString(0),
                                TenSanPham = reader.IsDBNull(1) ? null : reader.GetString(1),
                                GiaBan = reader.IsDBNull(2) ? 0 : reader.GetDouble(2)
                            });

                        }
                    }
                }
            }

            return linhkiens;
        }


        //insert data các dòng từ các bảng tạm thời và xóa dữ liệu trong bảng tạm thời bao gồm cả dự án và data
        public async void InsertAllDataFromTemp(string projectID)
        {

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string insertNhanVien = @"
                        INSERT INTO nhanvien (
                            manvduan,
                            manv,
                            hoten,
                            dantoc,
                            gioitinh,
                            ngaysinh,
                            diachi,
                            sdt,
                            email,
                            phongban,
                            cccd,
                            trangthai,
                            ngaykyhopdong,
                            maduan
                        )
                        SELECT 
                            manvduan,
                            manv,
                            hoten,
                            dantoc,
                            gioitinh,
                            ngaysinh,
                            diachi,
                            sdt,
                            email,
                            phongban,
                            cccd,
                            trangthai,
                            ngaykyhopdong,
                            maduan
                        FROM nhanvientamthoi
                        WHERE maduan = @maDuAn;
                    ";

                using (var command = new NpgsqlCommand(insertNhanVien, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectID);
                    await command.ExecuteNonQueryAsync();
                }
                // Thêm dữ liệu từ congviectamthoi vào congviec
                string insertCongViec = @"
                        INSERT INTO congviec (
                            macvduan,
                            stt,
                            tendichvu,
                            ngaythuchien,
                            hotenkh,
                            sdt,
                            diachi,
                            manv,
                            tennv,
                            malinhkien,
                            tenlinhkien,
                            soluonglinhkien,
                            maloi,
                            tenloi,
                            soluongloi,
                            phidichvu,
                            ghichu,
                            maduan
                        )
                        SELECT 
                            macvduan,
                            stt,
                            tendichvu,
                            ngaythuchien,
                            hotenkh,
                            sdt,
                            diachi,
                            manv,
                            tennv,
                            malinhkien,
                            tenlinhkien,
                            soluonglinhkien,
                            maloi,
                            tenloi,
                            soluongloi,
                            phidichvu,
                            ghichu,
                            maduan
                        FROM congviectamthoi
                        WHERE maduan = @maDuAn;
                    ";

                using (var command = new NpgsqlCommand(insertCongViec, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectID);
                    await command.ExecuteNonQueryAsync();
                }

                // Xóa tất cả các dòng trong nhanvientamthoi và congviectamthoi
                string deleteTempTables = @"
                        DELETE FROM duan_tam;
                        DELETE FROM nhanvientamthoi WHERE maduan = @maDuAn;
                        DELETE FROM congviectamthoi WHERE maduan = @maDuAn;
                    ";

                using (var command = new NpgsqlCommand(deleteTempTables, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectID);
                    await command.ExecuteNonQueryAsync();
                }
            }


        }

        public async void InsertEmployeeToDatabase(Employee newEmployee)
        {
            string insertEmployeeQuery = @"
                INSERT INTO nhanvientamthoi 
                (manvduan,manv, hoten, gioitinh, ngaysinh, diachi, sdt, email,phongban, cccd,trangthai,ngaykyhopdong,maduan  ) 
                VALUES 
                (@manvduan,@manv, @hoten, @gioitinh, @ngaysinh, @diachi, @sdt, @email,@phongban, @cccd, @trangthai,@ngaykyhopdong,@maduan)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(insertEmployeeQuery, connection))
                {
                    command.Parameters.AddWithValue("@manvduan", newEmployee.MaNhanVien + AppData.ProjectID);
                    command.Parameters.AddWithValue("@manv", newEmployee.MaNhanVien);
                    command.Parameters.AddWithValue("@hoten", newEmployee.HoTen);
                    command.Parameters.AddWithValue("@gioitinh", newEmployee.GioiTinh);
                    command.Parameters.AddWithValue("@ngaysinh", newEmployee.NgaySinh);
                    command.Parameters.AddWithValue("@diachi", newEmployee.DiaChi);
                    command.Parameters.AddWithValue("@sdt", newEmployee.SoDienThoai);
                    command.Parameters.AddWithValue("@email", newEmployee.Email);
                    command.Parameters.AddWithValue("@phongban", newEmployee.PhongBan);
                    command.Parameters.AddWithValue("@cccd", newEmployee.CCCD);
                    command.Parameters.AddWithValue("@trangthai", newEmployee.TrangThai);
                    command.Parameters.AddWithValue("@ngaykyhopdong", newEmployee.NgayKyHD);

                    command.Parameters.AddWithValue("@maduan", newEmployee.MaDuAn);


                    await command.ExecuteNonQueryAsync();
                }
            }
        }

            public ObservableCollection<Loisp> GetAllLoi()
            {
                string query = @"SELECT mahieu, tenloi, giaban
                             FROM loi";

                ObservableCollection<Loisp> lois = new ObservableCollection<Loisp>();

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lois.Add(new Loisp
                                {
                                    MaSanPham = reader.IsDBNull(0) ? null : reader.GetString(0),
                                    TenSanPham = reader.IsDBNull(1) ? null : reader.GetString(1),
                                    GiaBan = reader.IsDBNull(2) ? 0 : reader.GetDouble(2)
                                });

                            }
                        }
                    }
                }

                return lois;

            }

            public int TimSttLonNhat(string maduan)
            {
                int maxStt = 0; // Giá trị mặc định nếu không có kết quả
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open(); // Mở kết nối

                    string maxSttQuery = @"
                        SELECT COALESCE(MAX(stt), 0)
                        FROM (
                            SELECT stt FROM congviectamthoi WHERE maduan = @maduan
                            UNION ALL
                            SELECT stt FROM congviec WHERE maduan = @maduan
                        ) AS combined";

                    using (var command = new NpgsqlCommand(maxSttQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maduan", maduan);

                        var result = command.ExecuteScalar(); // Lấy kết quả
                        if (result != null && int.TryParse(result.ToString(), out int parsedResult))
                        {
                            maxStt = parsedResult; // Chuyển đổi thành số nguyên
                        }
                    }
                }
                return maxStt;
            }

            public async void InsertTaskToDatabaseTemp(Task newTask)
            {
                string insertCongViecQuery = @"
                INSERT INTO congviectamthoi (
                    stt, macvduan, ngaythuchien, hotenkh, sdt, diachi, tendichvu, manv, tennv, malinhkien, 
                    tenlinhkien, soluonglinhkien, maloi, tenloi, soluongloi, phidichvu, ghichu, maduan
                ) VALUES (
                    @stt, @macvduan, @ngaythuchien, @hotenkh, @sdt, @diachi, @tendichvu, @manv, @tennv, @malinhkien, 
                    @tenlinhkien, @soluonglinhkien, @maloi, @tenloi, @soluongloi, @phidichvu, @ghichu, @maduan
                )";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand(insertCongViecQuery, connection))
                    {
                        // Thêm các tham số cho truy vấn
                        command.Parameters.AddWithValue("@stt", newTask.Stt);
                        command.Parameters.AddWithValue("@macvduan", newTask.MaCVDuAn);
                        command.Parameters.AddWithValue("@ngaythuchien", newTask.NgayThucHien == DateTime.MinValue ? (object)DBNull.Value : newTask.NgayThucHien);
                        command.Parameters.AddWithValue("@hotenkh", newTask.HoTenKH ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@sdt", newTask.SDT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@diachi", newTask.DiaChi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tendichvu", newTask.TenDichVu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@manv", newTask.MaNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tennv", newTask.TenNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@malinhkien", newTask.MaLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenlinhkien", newTask.TenLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluonglinhkien", newTask.SoLuongLK);
                        command.Parameters.AddWithValue("@maloi", newTask.MaLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenloi", newTask.TenLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluongloi", newTask.SoLuongLoi);
                        command.Parameters.AddWithValue("@phidichvu", newTask.PhiDichVu);
                        command.Parameters.AddWithValue("@ghichu", newTask.GhiChu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@maduan", newTask.MaDuAn ?? (object)DBNull.Value);

                        // Thực thi truy vấn
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

        //public ObservableCollection<Loi> GetAllLoi()
        //{
        //ObservableCollection < Loi > res = new ObservableCollection<Loi >();
        //return res;
        //}

        public void SaveProjectWithDifferentName(Project projectToSave, string oldProjectID)
        {

        }

        public void DeleteProject(Project projectToDelete)
        {
            // query to delete row from table duan that has maDuAn = projectToDelete.ID
            string query = "DELETE FROM duan WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table nhanvien that has maduan = projectToDelete.ID
            query = "DELETE FROM nhanvien WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table congviec that has maduan = projectToDelete.ID
            query = "DELETE FROM congviec WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table linhkien that has maduan = projectToDelete.ID
            query = "DELETE FROM linhkien_duan WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table loi that has maduan = projectToDelete.ID
            query = "DELETE FROM loi_duan WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table duan_tam that has maduan = projectToDelete.ID
            query = "DELETE FROM duan_tam WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table nhanvien_tam that has maduan = projectToDelete.ID
            query = "DELETE FROM nhanvientamthoi WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table congviec_tam that has maduan = projectToDelete.ID  
            query = "DELETE FROM congviectamthoi WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table linhkien_tam that has maduan = projectToDelete.ID
            query = "DELETE FROM linhkienduantam WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table loi_tam that has maduan = projectToDelete.ID
            query = "DELETE FROM loiduantam WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            //query to delete row from table duan_tam that has maduan = projectToDelete.ID
            query = "DELETE FROM duan_tam WHERE maduan = @maDuAn";
            // execute query
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectToDelete.ID);
                    command.ExecuteNonQuery();
                }
            }

            
        }
    }

}
