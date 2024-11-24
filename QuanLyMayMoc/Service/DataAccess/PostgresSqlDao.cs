using Npgsql;
using QuanLyMayMoc.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

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
                                MaDuAn = reader.IsDBNull(16) ? null : reader.GetString(16) ,// MaDuAn
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
                    if (ngaythuchien.HasValue && !string.IsNullOrEmpty(tennv)&&ngaythuchien != DateTime.MinValue)
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
                    else if (ngaythuchien.HasValue&& string.IsNullOrEmpty(tennv))
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
                    else if (!string.IsNullOrEmpty(tennv) && ( ngaythuchien == DateTime.MinValue))
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
                    else if(string.IsNullOrEmpty(tennv) && (ngaythuchien == DateTime.MinValue))
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
    }

}
