using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Npgsql;
using QuanLyMayMoc.Model;
using QuanLyMayMoc.View;
using System;
using System.Collections;
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


        public ObservableCollection<Employee> GetEmployees()
        {
            string query = @"
            SELECT 
                hoten, manv, gioitinh, ngaykyhopdong, ngaysinh, 
                trangthai, dantoc, cccd, phongban, email, 
                sdt, diachi, maduan, anhdaidien
            FROM nhanvien
            WHERE maduan = @maduan";

            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Truyền giá trị cho tham số @maduan
                    command.Parameters.AddWithValue("@maduan", AppData.ProjectID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                HoTen = reader.IsDBNull(0) ? null : reader.GetString(0),
                                MaNhanVien = reader.IsDBNull(1) ? null : reader.GetString(1),
                                GioiTinh = reader.IsDBNull(2) ? null : reader.GetString(2),
                                NgayKyHD = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                                NgaySinh = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                                TrangThai = reader.IsDBNull(5) ? null : reader.GetString(5),
                                DanToc = reader.IsDBNull(6) ? null : reader.GetString(6),
                                CCCD = reader.IsDBNull(7) ? null : reader.GetString(7),
                                PhongBan = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Email = reader.IsDBNull(9) ? null : reader.GetString(9),
                                SoDienThoai = reader.IsDBNull(10) ? null : reader.GetString(10),
                                DiaChi = reader.IsDBNull(11) ? null : reader.GetString(11),
                                MaDuAn = reader.IsDBNull(12) ? null : reader.GetString(12),
                                AnhDaiDien = reader.IsDBNull(13) ? null : reader.GetString(13)
                                // DaLuu = !reader.IsDBNull(14) && reader.GetBoolean(14) // Nếu cần sử dụng DaLuu
                            });
                        }
                    }
                }
            }

            return employees;
        }




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
                    FROM congviectamthoi
                    WHERE maduan=@maduan;";



            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(querytemp, connection))
                {
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
                connection.Close();
            }




            return tasks;
        }
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



        //insert data các dòng từ các bảng tạm thời và xóa dữ liệu trong bảng tạm thời bao gồm cả dự án và data
        public async void InsertAllDataFromTemp(string projectID)
        {

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Them du lieu tu linhkienduantam vào linhkien_duan
                // Xóa dữ liệu cũ trong bảng linhkien_duan
                string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkien_duan
                                                   WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    await command.ExecuteNonQueryAsync();
                }
                // Thêm dữ liệu từ linhkienduantam vào linhkien_duan
                string insertLinhKienDuAnQuery = @" INSERT INTO LinhKien_DuAn (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                   SELECT mahieuduan, mahieu, tenlinhkien, giaban, maduan
                                                   FROM linhkienduantam
                                                    WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(insertLinhKienDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    await command.ExecuteNonQueryAsync();
                }
                // Them du lieu tu loiduantam vào loisp_duan
                // Xóa dữ liệu cũ trong bảng loi_duan
                string deleteLoiDuAnQuery = @" DELETE 
                                                FROM loi_duan
                                                WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(deleteLoiDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    await command.ExecuteNonQueryAsync();
                }
                // Thêm dữ liệu từ loiduantam vào loi_duan
                string insertLoiDuAnQuery = @" INSERT INTO Loi_DuAn (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                   SELECT mahieuduan, mahieu, tenloi, giaban, maduan
                                                   FROM loiduantam
                                                   WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(insertLoiDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    await command.ExecuteNonQueryAsync();
                }


                // Cập nhật giá trị DaLuu thành 1 cho tất cả các nhân viên có MaDuAn = @maDuAn
                string updateDaLuuQuery = @"
                        UPDATE nhanvien
                        SET daluu = B'1'
                        WHERE maduan = @maDuAn;
                    ";

                using (var command = new NpgsqlCommand(updateDaLuuQuery, connection))
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

                // Xóa tất cả các dòng trong nhanvientamthoi và congviectamthoi, linhkientam, loitam
                string deleteTempTables = @"
                        DELETE FROM duan_tam;
                        DELETE FROM congviectamthoi WHERE maduan = @maDuAn;
                        DELETE FROM linhkienduantam WHERE maduan = @maDuAn;
                        DELETE FROM loiduantam WHERE maduan = @maDuAn;

                    ";


                using (var command = new NpgsqlCommand(deleteTempTables, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", projectID);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async void InsertTaskToDaTaBaseTemp(Task newTask)
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
                    command.Parameters.AddWithValue("@ngaythuchien", newTask.NgayThucHien == System.DateTime.MinValue ? (object)DateTime.Now : (object)newTask.NgayThucHien);
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
        public async void InsertEmployeeToDatabase(Employee newEmployee)
        {
            string insertEmployeeQuery = @"
                INSERT INTO nhanvien
                (manvduan, manv, hoten, gioitinh, ngaysinh, diachi, sdt, email, phongban, cccd, trangthai, ngaykyhopdong, maduan,anhdaidien,dantoc, daluu) 
                VALUES 
                (@manvduan, @manv, @hoten, @gioitinh, @ngaysinh, @diachi, @sdt, @email, @phongban, @cccd, @trangthai, @ngaykyhopdong, @maduan,@anhdaidien,@dantoc, @daluu)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(insertEmployeeQuery, connection))
                {
                    // Gán giá trị cho các tham số trong câu truy vấn
                    command.Parameters.AddWithValue("@manvduan", newEmployee.MaNhanVien + AppData.ProjectID);
                    command.Parameters.AddWithValue("@manv", newEmployee.MaNhanVien);
                    command.Parameters.AddWithValue("@hoten", newEmployee.HoTen ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@gioitinh", newEmployee.GioiTinh ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ngaysinh", newEmployee.NgaySinh != DateTime.MinValue ? newEmployee.NgaySinh : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@diachi", newEmployee.DiaChi ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@sdt", newEmployee.SoDienThoai ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@email", newEmployee.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@phongban", newEmployee.PhongBan ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@cccd", newEmployee.CCCD ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@trangthai", newEmployee.TrangThai ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ngaykyhopdong", newEmployee.NgayKyHD != DateTime.MinValue ? newEmployee.NgayKyHD : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@maduan", newEmployee.MaDuAn ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@anhdaidien", newEmployee.AnhDaiDien ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@dantoc", newEmployee.DanToc ?? (object)DBNull.Value);
                    // Gán giá trị mặc định cho cột DaLuu là 0
                    command.Parameters.AddWithValue("@daluu", NpgsqlTypes.NpgsqlDbType.Bit, "0");


                    // Thực thi câu lệnh SQL
                    await command.ExecuteNonQueryAsync();
                }
            }
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


        public async void DeleteAllTasks(string maDuAn)
        {
            string deleteQuery = "DELETE FROM congviec WHERE maduan = @maduan";


            string deleteQueryTemp = "DELETE FROM congviectamthoi WHERE maduan = @maduan";


            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(deleteQueryTemp, connection))
                {
                    command.Parameters.AddWithValue("@maduan", maDuAn);
                    await command.ExecuteNonQueryAsync();
                }
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@maduan", maDuAn);
                    await command.ExecuteNonQueryAsync();
                }
            }

        }
        public async void UpdateTask(Task selectedTask, Task newTask)
        {
            using (var connection = new NpgsqlConnection(connectionString))

            {
                await connection.OpenAsync();

                // Kiểm tra xem task có tồn tại trong bảng `congviec` hay không
                string checkCongViecQuery = "SELECT COUNT(*) FROM congviec WHERE macvduan = @macvduan";
                int countInCongViec = 0;
                using (var command = new NpgsqlCommand(checkCongViecQuery, connection))
                {
                    command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                    countInCongViec = Convert.ToInt32(await command.ExecuteScalarAsync());
                }

                if (countInCongViec > 0)
                {
                    // Cập nhật bảng `congviec`
                    string updateCongViecQuery = @"
                        UPDATE congviec
                        SET 
                            ngaythuchien = @ngaythuchien,
                            hotenkh = @hotenkh,
                            sdt = @sdt,
                            diachi = @diachi,
                            tendichvu = @tendichvu,
                            phidichvu = @phidichvu,
                            malinhkien = @malinhkien,
                            tenlinhkien = @tenlinhkien,
                            soluonglinhkien = @soluonglinhkien,
                            maloi = @maloi,
                            tenloi = @tenloi,
                            soluongloi = @soluongloi,
                            manv = @manv,
                            tennv = @tennv,
                            ghichu = @ghichu,
                            maduan = @maduan
               
                        WHERE macvduan = @macvduan";
                    using (var command = new NpgsqlCommand(updateCongViecQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ngaythuchien", newTask.NgayThucHien);
                        command.Parameters.AddWithValue("@hotenkh", newTask.HoTenKH ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@sdt", newTask.SDT ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@diachi", newTask.DiaChi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tendichvu", newTask.TenDichVu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@phidichvu", newTask.PhiDichVu);
                        command.Parameters.AddWithValue("@malinhkien", newTask.MaLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenlinhkien", newTask.TenLK ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluonglinhkien", newTask.SoLuongLK);
                        command.Parameters.AddWithValue("@maloi", newTask.MaLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tenloi", newTask.TenLoi ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@soluongloi", newTask.SoLuongLoi);
                        command.Parameters.AddWithValue("@manv", newTask.MaNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@tennv", newTask.TenNV ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@ghichu", newTask.GhiChu ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@maduan", AppData.ProjectID/*newTask.MaDuAn ?? (object)DBNull.Value*/);

                        command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    // Kiểm tra bảng `congviectamthoi`
                    string checkCongViecTamThoiQuery = "SELECT COUNT(*) FROM congviectamthoi WHERE macvduan = @macvduan";
                    int countInCongViecTamThoi = 0;
                    using (var command = new NpgsqlCommand(checkCongViecTamThoiQuery, connection))
                    {
                        command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                        countInCongViecTamThoi = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }

                    if (countInCongViecTamThoi > 0)
                    {
                        // Cập nhật bảng `congviectamthoi`
                        string updateCongViecTamThoiQuery = @"
                            UPDATE congviectamthoi
                            SET 
                                ngaythuchien = @ngaythuchien,
                                hotenkh = @hotenkh,
                                sdt = @sdt,
                                diachi = @diachi,
                                tendichvu = @tendichvu,
                                phidichvu = @phidichvu,
                                malinhkien = @malinhkien,
                                tenlinhkien = @tenlinhkien,
                                soluonglinhkien = @soluonglinhkien,
                                maloi = @maloi,
                                tenloi = @tenloi,
                                soluongloi = @soluongloi,
                                manv = @manv,
                                tennv = @tennv,
                                ghichu = @ghichu,
                                maduan = @maduan
                  
                            WHERE macvduan = @macvduan";
                        using (var command = new NpgsqlCommand(updateCongViecTamThoiQuery, connection))
                        {
                            command.Parameters.AddWithValue("@ngaythuchien", newTask.NgayThucHien);
                            command.Parameters.AddWithValue("@hotenkh", newTask.HoTenKH ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@sdt", newTask.SDT ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@diachi", newTask.DiaChi ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@tendichvu", newTask.TenDichVu ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@phidichvu", newTask.PhiDichVu);
                            command.Parameters.AddWithValue("@malinhkien", newTask.MaLK ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@tenlinhkien", newTask.TenLK ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@soluonglinhkien", newTask.SoLuongLK);
                            command.Parameters.AddWithValue("@maloi", newTask.MaLoi ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@tenloi", newTask.TenLoi ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@soluongloi", newTask.SoLuongLoi);
                            command.Parameters.AddWithValue("@manv", newTask.MaNV ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@tennv", newTask.TenNV ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@ghichu", newTask.GhiChu ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@maduan", AppData.ProjectID/*newTask.MaDuAn ?? (object)DBNull.Value*/);

                            command.Parameters.AddWithValue("@macvduan", selectedTask.MaCVDuAn);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        // Task không tồn tại trong cả hai bảng
                        ContentDialog notFoundDialog = new ContentDialog
                        {
                            Title = "Không tìm thấy",
                            Content = "Task không tồn tại trong cơ sở dữ liệu.",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };

                        await notFoundDialog.ShowAsync();
                    }
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

        public ObservableCollection<Linhkien> GetAllLinhKienTam()
        {
            string query = @"
                SELECT mahieu, tenlinhkien, giaban
                FROM linhkienduantam
                WHERE maduan = @maduan";

            ObservableCollection<Linhkien> linhkiens = new ObservableCollection<Linhkien>();

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

        public ObservableCollection<Linhkien> GetAllLinhKienDuAn()
        {
            string query = @"
                SELECT mahieu, tenlinhkien, giaban
                FROM linhkien_duan
                WHERE maduan = @maduan";

            ObservableCollection<Linhkien> linhkiens = new ObservableCollection<Linhkien>();

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
        public async void SaveToLinhKienTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        // Xóa dữ liệu cũ trong bảng linhkienduantam (nếu có)
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkienduantam
                                                   WHERE maduan = @maduan";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                            await command.ExecuteNonQueryAsync();
                        }
                        string insertQuery = @" INSERT INTO linhkienduantam (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                SELECT CONCAT(mahieu,'_', @maDuAn), mahieu, tenlinhkien, giaban, @maDuAn
                                                FROM linhkien";
                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    //XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }
        public async void SaveToLinhKienDuAnToTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        // Xóa dữ liệu cũ trong bảng linhkienduantam (nếu có)
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkienduantam
                                                   WHERE maduan = @maDuAn";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                            await command.ExecuteNonQueryAsync();
                        }
                        string insertQuery = @" INSERT INTO linhkienduantam (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                SELECT mahieuduan, mahieu, tenlinhkien, giaban, maduan
                                                FROM linhkien_duan
                                                 WHERE maduan = @maDuAn";
                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }
        public async void DeleteAllLinhKienTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkienduantam
                                                   WHERE maduan = @maduan";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        public async Task<bool> CheckLinhKienTonTai(string maSanPham)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem dự án đã tồn tại hay chưa
                    string checkDuAnQuery = "SELECT COUNT(1) " +
                                            "FROM linhkienduantam " +
                                            "WHERE maduan = @maDuAn and mahieu = @maHieu";

                    long duAnExists;

                    using (var command = new NpgsqlCommand(checkDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        command.Parameters.AddWithValue("@maHieu", maSanPham);
                        duAnExists = (long)await command.ExecuteScalarAsync();
                    }

                    return duAnExists > 0;

                }
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return false;
            }

        }
        public int CheckLinhKienDuAnTamTonTai(string maDuAn)
        {
            string query = "SELECT COUNT(*) FROM linhkienduantam WHERE maduan = @MaDuAn";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection
                    command.Parameters.AddWithValue("@MaDuAn", maDuAn);

                    // Thực thi truy vấn và trả về kết quả
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
        }
        public int CheckLinhKienDuAnTonTai(string maDuAn)
        {
            string query = "SELECT COUNT(*) FROM linhkien_duan WHERE maduan = @MaDuAn";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection
                    command.Parameters.AddWithValue("@MaDuAn", maDuAn);

                    // Thực thi truy vấn và trả về kết quả
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
        }

        public async void DeleteLinhKienTam(string maLinhKien)
        {
            string maLinhKienDuAn = maLinhKien + "_" + AppData.ProjectID;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkienduantam
                                                   WHERE mahieuduan = @maLinhKienDuAn";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maLinhKienDuAn", maLinhKienDuAn);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        public async void InsertLinhKienToDaTaBaseTemp(Linhkien newLinhKien, string mahieuduan)
        {
            string insertQuery = @" INSERT INTO LinhKienDuAnTam (MaHieuDuAn, MaHieu, TenLinhKien, GiaBan, MaDuAn) 
                                    VALUES (@MaHieuDuAn, @MaHieu, @TenLinhKien, @GiaBan, @MaDuAn)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@MaHieuDuAn", mahieuduan);
                    command.Parameters.AddWithValue("@MaHieu", newLinhKien.MaSanPham);
                    command.Parameters.AddWithValue("@TenLinhKien", newLinhKien.TenSanPham);
                    command.Parameters.AddWithValue("@GiaBan", newLinhKien.GiaBan);
                    command.Parameters.AddWithValue("@MaDuAn", AppData.ProjectID);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async void UpdateLinhkien(Linhkien selectedLinhkien, Linhkien newLinhkien)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                // Cập nhật bảng `linhkienduantamthoi`
                string updateLinhkienTamThoiQuery = @" UPDATE linhkienduantam
                                                        SET TenLinhKien = @TenLinhKien,
                                                             GiaBan = @GiaBan
                                                        WHERE maduan = @maDuAn and mahieu = @maHieu";
                using (var command = new NpgsqlCommand(updateLinhkienTamThoiQuery, connection))
                {
                    command.Parameters.AddWithValue("@TenLinhKien", newLinhkien.TenSanPham);
                    command.Parameters.AddWithValue("@GiaBan", newLinhkien.GiaBan);
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    command.Parameters.AddWithValue("@maHieu", selectedLinhkien.MaSanPham);
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

        public ObservableCollection<Loisp> GetAllLoiTam()
        {
            string query = @"SELECT mahieu, tenloi, giaban
                            FROM loiduantam
                            WHERE maduan = @maDuAn";
        

            ObservableCollection<Loisp> lois = new ObservableCollection<Loisp>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);

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

        public ObservableCollection<Loisp> GetAllLoiDuAn()
        {
            string query = @"SELECT mahieu, tenloi, giaban
                             FROM loi_duan
                            WHERE maduan = @maDuAn";

            ObservableCollection<Loisp> lois = new ObservableCollection<Loisp>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);

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

        public async void SaveToLoiTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        // Xóa dữ liệu cũ trong bảng loiduantam (nếu có)
                        //string deleteLoiDuAnQuery = @" DELETE 
                        //                           FROM loiduantam
                        //                           WHERE maduan = @maDuAn";
                        //using (var command = new NpgsqlCommand(deleteLoiDuAnQuery, connection))
                        //{
                        //    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        //    await command.ExecuteNonQueryAsync();
                        //}
                        // Thêm dữ liệu từ loi vào loiduantam
                        string insertQuery = @" INSERT INTO loiduantam (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                SELECT CONCAT(mahieu,'_', @maDuAn), mahieu, tenloi, giaban, @maDuAn
                                                FROM loi";
                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }
        public async void SaveToLoiDuAnToTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        // Xóa dữ liệu cũ trong bảng loiduantam (nếu có)
                        string deleteLoiDuAnQuery = @" DELETE 
                                                   FROM loiduantam
                                                   WHERE maduan = @maDuAn";
                        using (var command = new NpgsqlCommand(deleteLoiDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                            await command.ExecuteNonQueryAsync();
                        }
                        // Thêm dữ liệu từ loi vào loiduantam
                        string insertQuery = @" INSERT INTO loiduantam (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                SELECT mahieuduan, mahieu, tenloi, giaban, maduan
                                                FROM loi_duan
                                                WHERE maduan = @maDuAn";
                        using (var command = new NpgsqlCommand(insertQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }

        }

        public async void DeleteAllLoiTam()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM loiduantam
                                                   WHERE maduan = @maduan";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        public async void DeleteLoiTam(string maLoi)
        {
            string maLinhKienDuAn = maLoi + "_" + AppData.ProjectID;
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    {
                        string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM loiduantam
                                                   WHERE mahieuduan = @maLoiDuAn";
                        using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                        {
                            command.Parameters.AddWithValue("@maLoiDuAn", maLinhKienDuAn);
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
                    Content = $"Có lỗi xảy ra khi tạo dự án: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
        public async void InsertLoiToDaTaBaseTemp(Loisp newLoi, string mahieuduan)
        {
            string insertQuery = @" INSERT INTO LoiDuAnTam (MaHieuDuAn, MaHieu, TenLoi, GiaBan, MaDuAn) 
                                    VALUES (@MaHieuDuAn, @MaHieu, @TenLoi, @GiaBan, @MaDuAn)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@MaHieuDuAn", mahieuduan);
                    command.Parameters.AddWithValue("@MaHieu", newLoi.MaSanPham);
                    command.Parameters.AddWithValue("@TenLoi", newLoi.TenSanPham);
                    command.Parameters.AddWithValue("@GiaBan", newLoi.GiaBan);
                    command.Parameters.AddWithValue("@MaDuAn", AppData.ProjectID);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> CheckLoiTonTai(string maSanPham)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem dự án đã tồn tại hay chưa
                    string checkDuAnQuery = "SELECT COUNT(1) " +
                                            "FROM loiduantam " +
                                            "WHERE maduan = @maDuAn and mahieu = @maHieu";

                    long duAnExists;

                    using (var command = new NpgsqlCommand(checkDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                        command.Parameters.AddWithValue("@maHieu", maSanPham);
                        duAnExists = (long)await command.ExecuteScalarAsync();
                    }

                    return duAnExists > 0;

                }
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
                return false;
            }

        }
        public int CheckLoiDuAnTamTonTai(string maDuAn)
        {
            string query = "SELECT COUNT(*) FROM loiduantam WHERE maduan = @MaDuAn";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection
                    command.Parameters.AddWithValue("@MaDuAn", maDuAn);

                    // Thực thi truy vấn và trả về kết quả
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
        }
        public int CheckLoiDuAnTonTai(string maDuAn)
        {
            string query = "SELECT COUNT(*) FROM loi_duan WHERE maduan = @MaDuAn";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection
                    command.Parameters.AddWithValue("@MaDuAn", maDuAn);

                    // Thực thi truy vấn và trả về kết quả
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
        }

        public async void UpdateLoisp(Loisp selectedLoi, Loisp newLoi)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                // Cập nhật bảng `linhkienduantamthoi`
                string updateLoiTamThoiQuery = @" UPDATE loiduantam
                                                        SET TenLoi = @TenLoi,
                                                             GiaBan = @GiaBan
                                                        WHERE maduan = @maDuAn and mahieu = @maHieu";
                using (var command = new NpgsqlCommand(updateLoiTamThoiQuery, connection))
                {
                    command.Parameters.AddWithValue("@TenLoi", newLoi.TenSanPham);
                    command.Parameters.AddWithValue("@GiaBan", newLoi.GiaBan);
                    command.Parameters.AddWithValue("@maDuAn", AppData.ProjectID);
                    command.Parameters.AddWithValue("@maHieu", selectedLoi.MaSanPham);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async void SaveProjectWithDifferentName(Project projectInsert, string maDuAnCu)
        {
            string maDuAnMoi = projectInsert.ID;
            string tenDuAnMoi = projectInsert.Name;
            DateTime ngayBatDauDuAnMoi = projectInsert.TimeCreate;

            List<String> Queries = new List<string>
            {
                @"
                INSERT INTO duan (maduan, tenduan, ngaythuchien)
                SELECT @maDuAnMoi, @tenDuAnMoi, @ngayThucHienDuAnMoi
                FROM duan
                WHERE maduan = @maDuAnCu;
                "
            };

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < Queries.Count; i++)
                        {
                            using (var command = new NpgsqlCommand(Queries[i], connection, transaction))
                            {
                                command.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                                command.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                                command.Parameters.AddWithValue("@tenDuAnMoi", tenDuAnMoi);
                                command.Parameters.AddWithValue("@ngayThucHienDuAnMoi", ngayBatDauDuAnMoi);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Them du lieu tu linhkienduantam vào linhkien_duan
                // Xóa dữ liệu cũ trong bảng linhkien_duan
                string deleteLinhKienDuAnQuery = @" DELETE 
                                                   FROM linhkien_duan
                                                   WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(deleteLinhKienDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", maDuAnMoi);
                    await command.ExecuteNonQueryAsync();
                }
                if(CheckLinhKienDuAnTamTonTai(maDuAnCu) > 0)
                {
                    // Thêm dữ liệu từ linhkienduantam vào linhkien_duan
                    string insertLinhKienDuAnQuery = @" INSERT INTO LinhKien_DuAn (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                   SELECT  CONCAT(mahieu,'_', @maDuAnMoi), mahieu, tenlinhkien, giaban, @maDuAnMoi
                                                   FROM linhkienduantam
                                                    WHERE maduan = @maDuAnCu";
                    using (var command = new NpgsqlCommand(insertLinhKienDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                        command.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                else if(CheckLinhKienDuAnTonTai(maDuAnCu) > 0)
                {
                    // Thêm dữ liệu từ linhkien_duan vào linhkien_duan
                    string insertLinhKienDuAnQuery = @" INSERT INTO LinhKien_DuAn (mahieuduan, mahieu, tenlinhkien, giaban, maduan)
                                                   SELECT  CONCAT(mahieu,'_', @maDuAnMoi), mahieu, tenlinhkien, giaban, @maDuAnMoi
                                                   FROM linhkien_duan
                                                    WHERE maduan = @maDuAnCu";
                    using (var command = new NpgsqlCommand(insertLinhKienDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                        command.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                // Them du lieu tu loiduantam vào loisp_duan
                // Xóa dữ liệu cũ trong bảng loi_duan
                string deleteLoiDuAnQuery = @" DELETE 
                                                FROM loi_duan
                                                WHERE maduan = @maDuAn";
                using (var command = new NpgsqlCommand(deleteLoiDuAnQuery, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", maDuAnMoi);
                    await command.ExecuteNonQueryAsync();
                }
                if (CheckLoiDuAnTamTonTai(maDuAnCu) > 0) {
                    // Thêm dữ liệu từ loiduantam vào loi_duan
                    string insertLoiDuAnQuery = @" INSERT INTO Loi_DuAn (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                   SELECT  CONCAT(mahieu,'_', @maDuAnMoi), mahieu, tenloi, giaban, @maDuAnMoi
                                                   FROM loiduantam
                                                    WHERE maduan = @maDuAnCu";
                    using (var command = new NpgsqlCommand(insertLoiDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                        command.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                else if (CheckLoiDuAnTonTai(maDuAnCu) > 0) {
                    // Thêm dữ liệu từ loiduantam vào loi_duan
                    string insertLoiDuAnQuery = @" INSERT INTO Loi_DuAn (mahieuduan, mahieu, tenloi, giaban, maduan)
                                                   SELECT  CONCAT(mahieu,'_', @maDuAnMoi), mahieu, tenloi, giaban, @maDuAnMoi
                                                   FROM loi_duan
                                                    WHERE maduan = @maDuAnCu";
                    using (var command = new NpgsqlCommand(insertLoiDuAnQuery, connection))
                    {
                        command.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                        command.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                        await command.ExecuteNonQueryAsync();
                    }
                }


                string copyNhanVien = @"
                    INSERT INTO nhanvien (
                        MaNVDuAn, MaNV, HoTen, DanToc, GioiTinh, NgaySinh, DiaChi, SDT, Email, 
                        PhongBan, CCCD, AnhDaiDien, TrangThai, NgayKyHopDong, MaDuAn, DaLuu)
                    SELECT 
                        CONCAT(MaNV, @maDuAnMoi) AS MaNVDuAn,
                        MaNV,
                        HoTen,
                        DanToc,
                        GioiTinh,
                        NgaySinh,
                        DiaChi,
                        SDT,
                        Email,
                        PhongBan,
                        CCCD,
                        AnhDaiDien,
                        TrangThai,
                        NgayKyHopDong,
                        @maDuAnMoi AS MaDuAn,
                        B'1' AS DaLuu 
                    FROM nhanvien
                    WHERE MaDuAn = @maDuAnCu;
                ";




                // Cập nhật maduan thành maDuAnMoi
                using (var updateMaDuAnCommand = new NpgsqlCommand(copyNhanVien, connection))
                {
                    updateMaDuAnCommand.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);   // Mã dự án cũ
                    updateMaDuAnCommand.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi); // Mã dự án mới
                    await updateMaDuAnCommand.ExecuteNonQueryAsync();
                }
            }

            // Thêm dữ liệu từ congviectamthoi vào congviec
            // Câu lệnh SQL để cập nhật maduan trong bảng congviectamthoi
            string copyFromCongViec = @"
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
                        CONCAT(@maDuAnMoi, '_', TO_CHAR(NOW(), 'YYYY_MM_DD_HH24_MI_SS'), '_', stt) AS macvduan, -- Tạo khóa chính mới
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
                        @maDuAnMoi AS maduan
                    FROM congviec
                    WHERE maduan = @maDuAnCu;
                ";

            string copyFromCongViecTamThoi = @"
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
                            CONCAT(@maDuAnMoi, '_', TO_CHAR(NOW(), 'YYYY_MM_DD_HH24_MI_SS'), '_', stt) AS macvduan, -- Tạo khóa chính mới
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
                            @maDuAnMoi AS maduan
                        FROM congviectamthoi
                        WHERE maduan = @maDuAnCu;
                    ";


          
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Copy từ bảng congviec
                using (var copyCommand = new NpgsqlCommand(copyFromCongViec, connection))
                {
                    copyCommand.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                    copyCommand.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                    await copyCommand.ExecuteNonQueryAsync();
                }

                // Copy từ bảng congviectamthoi
                using (var copyCommand = new NpgsqlCommand(copyFromCongViecTamThoi, connection))
                {
                    copyCommand.Parameters.AddWithValue("@maDuAnCu", maDuAnCu);
                    copyCommand.Parameters.AddWithValue("@maDuAnMoi", maDuAnMoi);
                    await copyCommand.ExecuteNonQueryAsync();
                }
               
                // Xóa tất cả các dòng trong nhanvientamthoi và congviectamthoi, linhkientam, loitam
                string deleteTempTables = @"
                        DELETE FROM duan_tam;
                        
                       
                        DELETE FROM linhkienduantam WHERE maduan = @maDuAn;
                        DELETE FROM loiduantam WHERE maduan = @maDuAn;
                    ";

                using (var command = new NpgsqlCommand(deleteTempTables, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", maDuAnCu);
                    await command.ExecuteNonQueryAsync();
                }

                string updateBitNhanVien = @"
                    UPDATE nhanvien
                    SET daluu = B'1'
                    WHERE maduan = @maDuAn;
                ";

                using (var command = new NpgsqlCommand(updateBitNhanVien, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", maDuAnMoi);
                    await command.ExecuteNonQueryAsync();
                }

                // Xóa tất cả các dòng trong nhanvientamthoi và congviectamthoi, linhkientam, loitam
                string deleteTempTask = @"
                      
                        
                        DELETE FROM congviectamthoi WHERE maduan = @maDuAn;
                      
                    ";

                using (var command = new NpgsqlCommand(deleteTempTask, connection))
                {
                    command.Parameters.AddWithValue("@maDuAn", maDuAnMoi);
                    await command.ExecuteNonQueryAsync();
                }

            }


        }


        public ObservableCollection<MonthlyProductSummary> GetMonthlyProductSummaries()
        {
            string query = @"
            SELECT 
                Thang, MaSanPham, TenSanPham, SoLuong, GiaBan, TongTien
            FROM TongHopSanPhamTheoThang";

            ObservableCollection<MonthlyProductSummary> monthlyProductSummaries = new ObservableCollection<MonthlyProductSummary>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monthlyProductSummaries.Add(new MonthlyProductSummary
                            {
                                Month = reader.IsDBNull(0) ? 0 : reader.GetInt32(0), // Thang
                                ProductCode = reader.IsDBNull(1) ? null : reader.GetString(1), // MaSanPham
                                ProductName = reader.IsDBNull(2) ? null : reader.GetString(2), // TenSanPham
                                Quantity = reader.IsDBNull(3) ? 0 : reader.GetInt32(3), // SoLuong
                                Price = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4), // GiaBan
                                TotalPrice = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5) // TongTien
                            });
                        }
                    }
                }
            }

            return monthlyProductSummaries;
        }
        public ObservableCollection<MonthlyServiceSummary> GetMonthlyServiceSummaries()
        {
            string query = @"
                SELECT 
                    MaTongHop, Thang, MaNhanVien, TenNhanVien, MaCongViec, PhiDichVu, TongPhiDichVu, TongPhiDichVuThang
                FROM TongHopDichVuTheoThang";

            ObservableCollection<MonthlyServiceSummary> monthlyServiceSummaries = new ObservableCollection<MonthlyServiceSummary>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            monthlyServiceSummaries.Add(new MonthlyServiceSummary
                            {
                                Code = reader.IsDBNull(0) ? null : reader.GetString(0), // MaTongHop
                                Month = reader.IsDBNull(1) ? 0 : reader.GetInt32(1), // Thang
                                EmployeeCode = reader.IsDBNull(2) ? null : reader.GetString(2), // MaNhanVien
                                EmployeeName = reader.IsDBNull(3) ? null : reader.GetString(3), // TenNhanVien
                                TaskCode = reader.IsDBNull(4) ? null : reader.GetString(4), // MaCongViec
                                ServiceFee = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5), // PhiDichVu
                                TotalServiceFee = reader.IsDBNull(6) ? 0.0 : reader.GetDouble(6), // TongPhiDichVu
                                MonthlyTotalFee = reader.IsDBNull(7) ? 0.0 : reader.GetDouble(7) // TongPhiDichVuThang
                            });
                        }
                    }
                }
            }

            return monthlyServiceSummaries;
        }

        public void SummaryProduct(ObservableCollection<MonthlyProductSummary> monthlyProductSummaries)
        {
            string insertQuery = @"
                    INSERT INTO TongHopSanPhamTheoThang 
                    (MaTongHop, Thang, MaSanPham, TenSanPham, SoLuong, GiaBan, TongTien)
                    VALUES (@MaTongHop, @Thang, @MaSanPham, @TenSanPham, @SoLuong, @GiaBan, @TongTien)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var summary in monthlyProductSummaries)
                        {
                            using (var command = new NpgsqlCommand(insertQuery, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@MaTongHop", summary.Code);
                                command.Parameters.AddWithValue("@Thang", summary.Month);
                                command.Parameters.AddWithValue("@MaSanPham", summary.ProductCode);
                                command.Parameters.AddWithValue("@TenSanPham", summary.ProductName);
                                command.Parameters.AddWithValue("@SoLuong", summary.Quantity);
                                command.Parameters.AddWithValue("@GiaBan", summary.Price);
                                command.Parameters.AddWithValue("@TongTien", summary.TotalPrice);

                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void SummaryService(ObservableCollection<MonthlyServiceSummary> monthlyServiceSummaries)
        {
            string query = @"
                INSERT INTO TongHopDichVuTheoThang 
                (MaTongHop, Thang, MaNhanVien, TenNhanVien, MaCongViec, PhiDichVu, TongPhiDichVu, TongPhiDichVuThang) 
                VALUES (@Code, @Month, @EmployeeCode, @EmployeeName, @TaskCode, @ServiceFee, @TotalServiceFee, @MonthlyTotalFee)";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                foreach (var summary in monthlyServiceSummaries)
                {
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Code", summary.Code);
                        command.Parameters.AddWithValue("@Month", summary.Month);
                        command.Parameters.AddWithValue("@EmployeeCode", summary.EmployeeCode);
                        command.Parameters.AddWithValue("@EmployeeName", summary.EmployeeName);
                        command.Parameters.AddWithValue("@TaskCode", summary.TaskCode);
                        command.Parameters.AddWithValue("@ServiceFee", summary.ServiceFee);
                        command.Parameters.AddWithValue("@TotalServiceFee", summary.TotalServiceFee);
                        command.Parameters.AddWithValue("@MonthlyTotalFee", summary.MonthlyTotalFee);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ClearSummary()
        {
            string deleteProductSummaryQuery = "DELETE FROM TongHopSanPhamTheoThang";
            string deleteServiceSummaryQuery = "DELETE FROM TongHopDichVuTheoThang";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Xóa dữ liệu trong bảng TongHopSanPhamTheoThang
                        using (var deleteProductCommand = new NpgsqlCommand(deleteProductSummaryQuery, connection))
                        {
                            deleteProductCommand.Transaction = transaction;
                            deleteProductCommand.ExecuteNonQuery();
                        }

                        // Xóa dữ liệu trong bảng TongHopDichVuTheoThang
                        using (var deleteServiceCommand = new NpgsqlCommand(deleteServiceSummaryQuery, connection))
                        {
                            deleteServiceCommand.Transaction = transaction;
                            deleteServiceCommand.ExecuteNonQuery();
                        }

                        // Xác nhận thay đổi
                        transaction.Commit();
                    }
                    catch
                    {
                        // Hủy bỏ thay đổi nếu có lỗi
                        transaction.Rollback();
                        throw; // Ném lại lỗi để xử lý ở cấp cao hơn
                    }
                }
            }
        }
        public void DeleteProject(string maDuAn)
        {
            List<String> deleteQueries = new List<string> {"DELETE FROM duan WHERE maduan = @maDuAn",
                    "DELETE FROM nhanvien WHERE maduan = @maDuAn",
                    "DELETE FROM linhkien_duan WHERE maduan = @maDuAn",
                    "DELETE FROM linhkienduantam WHERE maduan = @maDuAn",
                    "DELETE FROM loiduantam WHERE maduan = @maDuAn",
                    "DELETE FROM congviectamthoi WHERE maduan = @maDuAn",
                    "DELETE FROM congviec WHERE maduan = @maDuAn",
                    "DELETE FROM loiduantam WHERE maduan = @maDuAn",
                    "DELETE FROM loi_duan WHERE maduan = @maDuAn",
                    "DELETE FROM duan_tam WHERE maduan = @maDuAn",
            };


            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var query in deleteQueries)
                        {
                            using (var command = new NpgsqlCommand(query, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@maDuAn", maDuAn);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public int CountMaNhanVien(string MaNhanVien)
        {
            string query = "SELECT COUNT(*) FROM NhanVien WHERE manv = @MaNhanVien and maduan = @MaDuAn";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    // Thêm tham số để tránh SQL Injection
                    command.Parameters.AddWithValue("@MaNhanVien", MaNhanVien);
                    command.Parameters.AddWithValue("@MaDuAn", AppData.ProjectID);

                    // Thực thi truy vấn và trả về kết quả
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count;
                }
            }
        }

        public void ClearAllTempData()
        {
            string deleteTempTables = @"
                        DELETE FROM duan_tam;
                        DELETE FROM linhkienduantam;
                        DELETE FROM loiduantam;
                        DELETE FROM congviectamthoi;
                    ";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(deleteTempTables, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
