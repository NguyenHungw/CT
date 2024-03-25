using CT.MOD;
using CT.ULT;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CT.Controllers.Paypal
{
    public class DatabaseHelper1
    {
        private readonly string connectionString;

        public DatabaseHelper1(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int GetProductQuantity(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select SoLuong from dbo.ChiTietNhap where MSanPham = @MSanPham;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MSanPham", productId);

                    // Sử dụng ExecuteScalar để lấy giá trị số lượng
                    object result = command.ExecuteScalar();

                    // Kiểm tra giá trị null và chuyển đổi sang kiểu số nguyên
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }


        public List<CartUser> GetCartItems(int idUser)
        {
            List<CartUser> productCart = new List<CartUser>();

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    //int idUser = 1;

                    //check gio hang xem co sp hay ko
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"select u.Username,sp.TenSanPham,gh.MSanPham, gh.GioSoLuong, gb.GiaBan
                                        from GioHang gh
                                        inner join SanPham sp on gh.MSanPham = sp.MSanPham
                                        inner join GiaBanSanPham gb on sp.MSanPham = gb.MSanPham
                                        inner join [User] u on gh.idUser = u.idUser
                                        where gh.idUser = @idUser;";
                    cmd.Parameters.AddWithValue("@idUser", idUser);
                    cmd.Connection = sqlCon;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartUser item = new CartUser();
                            item.Username = reader.GetString(0);
                            item.TenSanPham = reader.GetString(1);
                            item.MSanPham = reader.GetString(2);
                            item.GioSoLuong = reader.GetInt32(3);
                            item.GiaBan = reader.GetDecimal(4);
                            productCart.Add(item);
                        }
                    }

                }

                return productCart;
            }
        }


        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE ChiTietNhap SET SoLuong = @NewSoLuong WHERE ProductId = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@NewSoLuong", newQuantity);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void RemoveProductFromCart(int idUser)
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();

                string query = "delete from GioHang where idUser=@idUser;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUser", idUser);

                    command.ExecuteNonQuery();
                }
            }
        }


        /// order
        /// 

        public void SaveOrder(string orderId, int iduser, string PhuongThucThanhToan, DateTime NgayMua, string Status)
        {
            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO DonHang (OrderId, iduser, PhuongThucThanhToan, NgayMua,Status) VALUES (@OrderId, @iduser, @PhuongThucThanhToan, @NgayMua,@Status)", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.Parameters.AddWithValue("@iduser", iduser);
                    command.Parameters.AddWithValue("@PhuongThucThanhToan", PhuongThucThanhToan);
                    command.Parameters.AddWithValue("@NgayMua", NgayMua);
                    command.Parameters.AddWithValue("@Status", Status);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdatePaymentStatus(string orderId, string Status)
        {
            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE DonHang SET Status = @Status WHERE OrderId = @OrderId", connection))
                {
                    command.Parameters.AddWithValue("@Status", Status);
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Các thuộc tính và các logic khác

        public void SaveOrderDetail(string OrderId, string MSanPham, int SoLuong, decimal DonGia, decimal TrietKhau, decimal ThanhTien)
        {
            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();

                // Tạo một transaction để đảm bảo tính nhất quán khi lưu dữ liệu
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Tiến hành lưu chi tiết đơn hàng vào cơ sở dữ liệu

                        // Xây dựng câu lệnh SQL để chèn dữ liệu vào bảng chitietdonhang
                        string sql = @"INSERT INTO ChiTietDonHang (OrderId, MSanPham, SoLuong, DonGia, TrietKhau, ThanhTien)
                        VALUES (@OrderId, @MSanPham, @SoLuong, @DonGia, @TrietKhau, @ThanhTien)";

                        // Tạo đối tượng Command để thực thi câu lệnh SQL
                        SqlCommand command = new SqlCommand(sql, connection, transaction);

                        // Truyền các tham số vào câu lệnh SQL
                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@MSanPham", MSanPham);
                        command.Parameters.AddWithValue("@SoLuong", SoLuong);
                        command.Parameters.AddWithValue("@DonGia", DonGia);
                        command.Parameters.AddWithValue("@TrietKhau", TrietKhau);
                        command.Parameters.AddWithValue("@ThanhTien", ThanhTien);

                        // Thực thi câu lệnh SQL
                        command.ExecuteNonQuery();

                        // Commit transaction để lưu các thay đổi vào cơ sở dữ liệu
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Nếu có lỗi xảy ra, rollback transaction để hủy bỏ các thay đổi
                        transaction.Rollback();
                        // Xử lý hoặc ghi log lỗi
                        Console.WriteLine("Error occurred: " + ex.Message);
                        throw; // Ném ngoại lệ để thông báo lỗi cho lớp gọi
                    }
                }
            }
        }
        public void TruSoLuongSanPham(string msp, int soLuongCanTru)
        {
            using (SqlConnection connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int soLuongDaTru = 0;
                    int currentID = 0;

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                        SELECT TOP 1 ID_ChiTietNhap
                        FROM ChiTietNhap
                        WHERE MSanPham = @msp
                        ORDER BY ID_ChiTietNhap";

                        command.Parameters.AddWithValue("@msp", msp);

                        currentID = (int)command.ExecuteScalar();
                    }

                    while (soLuongDaTru < soLuongCanTru && currentID != 0)
                    {
                        int soLuong = 0;

                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = @"
                            SELECT SoLuong
                            FROM ChiTietNhap
                            WHERE ID_ChiTietNhap = @currentID";

                            command.Parameters.AddWithValue("@currentID", currentID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    soLuong = reader.GetInt32(0);
                                }
                            }
                        }

                        if (soLuong >= soLuongCanTru - soLuongDaTru)
                        {
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"
                                UPDATE ChiTietNhap
                                SET SoLuong = SoLuong - (@soLuongCanTru - @soLuongDaTru)
                                WHERE ID_ChiTietNhap = @currentID";

                                command.Parameters.AddWithValue("@soLuongCanTru", soLuongCanTru);
                                command.Parameters.AddWithValue("@soLuongDaTru", soLuongDaTru);
                                command.Parameters.AddWithValue("@currentID", currentID);

                                command.ExecuteNonQuery();
                            }

                            soLuongDaTru = soLuongCanTru;
                        }
                        else
                        {
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"
                                UPDATE ChiTietNhap
                                SET SoLuong = 0
                                WHERE ID_ChiTietNhap = @currentID";

                                command.Parameters.AddWithValue("@currentID", currentID);

                                command.ExecuteNonQuery();
                            }

                            soLuongDaTru += soLuong;

                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandText = @"
                                SELECT TOP 1 ID_ChiTietNhap
                                FROM ChiTietNhap
                                WHERE MSanPham = @msp
                                AND ID_ChiTietNhap > @currentID
                                ORDER BY ID_ChiTietNhap";

                                command.Parameters.AddWithValue("@msp", msp);
                                command.Parameters.AddWithValue("@currentID", currentID);

                                currentID = (int)command.ExecuteScalar();
                            }
                        }

                        if (currentID == 0)
                        {
                            break;
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }


}



