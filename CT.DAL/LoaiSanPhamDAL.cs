using CT.MOD;
using CT.ULT;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
    public class LoaiSanPhamDAL
    {
        //private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        SqlConnection SQLCon = null;
        public BaseResultMOD getDanhSachLoaiSP(int page)
        {
            const int productperpage = 20;
            int startpage = productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<LoaiSanPhamMOD> dslsp = new List<LoaiSanPhamMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM LoaiSanPham";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        LoaiSanPhamMOD item = new LoaiSanPhamMOD();
                        item.ID_LoaiSanPham = read.GetInt32(0);
                        item.TenLoaiSP = read.GetString(1);
                        item.MoTaLoaiSP = read.GetString(2);
                        item.TrangThai = read.GetBoolean(3);

                        dslsp.Add(item);
                    }
                    read.Close();
                    result.Status = 1;
                    result.Data = dslsp;


                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD SuaLoaiSP(LoaiSanPhamMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = SQLCon;
                    cmd.CommandText = "UPDATE [LoaiSanPham] SET TenLoaiSP=@TenLoaiSP, MoTaLoaiSP =@MoTaLoaiSP, TrangThai=@TrangThai where ID_LoaiSanPham=@ID_LoaiSanPham";
                    cmd.Parameters.AddWithValue("@TenLoaiSP", item.TenLoaiSP);
                    cmd.Parameters.AddWithValue("@MoTaLoaiSP", item.MoTaLoaiSP);
                    cmd.Parameters.AddWithValue("@TrangThai", item.TrangThai);
                    cmd.Parameters.AddWithValue("@ID_LoaiSanPham", item.ID_LoaiSanPham);
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Chỉnh sửa thông tin thành công";
                    result.Data = 1;


                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }

        public LoaiSanPhamMOD ThongTinLSp(int id)
        {
            LoaiSanPhamMOD item = null;

            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(SQLHelper.appConnectionStrings);

                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT ID_LoaiSanPham from LoaiSanPham where ID_LoaiSanPham ='" + id + "'";
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new LoaiSanPhamMOD();
                    item.ID_LoaiSanPham = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return item;
        }

        public BaseResultMOD ThemLoaiSP(ThemMoiLoaiSP item)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO LoaiSanPham (TenLoaiSP,MoTaLoaiSP,TrangThai) VALUES (@TenLoaiSP,@MoTaLoaiSP,@TrangThai)";
                    cmd.Parameters.AddWithValue("@TenLoaiSP", item.TenLoaiSP);
                    cmd.Parameters.AddWithValue("@MoTaLoaiSP", item.MoTaLoaiSP);
                    cmd.Parameters.AddWithValue("@TrangThai", item.TrangThai);
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Thêm mới loại sán phẩm thành công";
                    result.Data = 1;


                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD XoaLoaiSP(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE from LoaiSanPham where ID_LoaiSanPham=@ID_LoaiSanPham";
                    cmd.Parameters.AddWithValue("@ID_LoaiSanPham", id);
                    cmd.ExecuteReader();

                    result.Status = 1;
                    result.Message = "Xóa loại sản phẩm thành công";
                    
                }

            }catch(Exception ex)
            {
                result.Status = 1;
                result.Message= ULT.Constant.API_Error_System;
            }
            return result;
        }
    }
}
