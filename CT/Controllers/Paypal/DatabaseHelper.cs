using CT.MOD;
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

                string query = "UPDATE Products SET Quantity = @NewQuantity WHERE ProductId = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@NewQuantity", newQuantity);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}