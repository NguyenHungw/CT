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
    public class NhomNgoiDungDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        //SqlConnection SQLCon = null;

        public BaseResultMOD getDanhSachNhomND(int page)

        {
            const int ProductPerPage = 10;
            int startPage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachNhomNDMOD> dsnnd = new List<DanhSachNhomNDMOD>();
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {

                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM NhomNguoiDung";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachNhomNDMOD item = new DanhSachNhomNDMOD();
                        item.NNDID = reader.GetInt32(0);
                        item.TenNND = reader.GetString(1);
                        item.GhiChu = reader.GetString(2);
                        dsnnd.Add(item);

                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsnnd;
                }
            }

            catch (Exception ex)
            {
                throw;

            }
            return result;
        }

        public BaseResultMOD ThemNND(ThemMoiNND item)
        {
            var result = new BaseResultMOD();
            try
            {
                bool isDuplicate = KiemTraTrungNND(item);
                if (isDuplicate)
                {
                    result.Status = -1;
                    result.Message = "Tên nhóm người dùng đã tồn tại.";
                }
                else
                {
                    using (SqlConnection SQLCon = new SqlConnection(strcon))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "insert into NhomNguoiDung (TenNND,GhiChu)VALUES(@TenNND,@GhiChu)";
                        cmd.Connection = SQLCon;
                        cmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                        cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
                        SQLCon.Open();

                        cmd.ExecuteNonQuery();
                        result.Status = 1;
                        result.Message = "Thêm thành công";
                        result.Data = 1;

                    }
                }
                

            }catch(Exception ex){
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                throw;
            }
            return result;
        }

        private bool KiemTraTrungNND(ThemMoiNND item)
        {
            using (SqlConnection SQLCon = new SqlConnection(strcon))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM NhomNguoiDung WHERE TenNND = @TenNND";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }
        public BaseResultMOD SuaNhomND(DanhSachNhomNDMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update NhomNguoiDung set TenNND = @TenNND , GhiChu = @GhiChu where NNDID=@NNDID";
                    cmd.Connection = SQLCon;
                    

                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@TenNND", item.TenNND);
                    cmd.Parameters.AddWithValue("@GhiChu", item.GhiChu);
                    SQLCon.Open();
                    cmd.ExecuteNonQuery();
                    Result.Status = 1;
                    Result.Message = "Sua thanh cong";
                    Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ULT.Constant.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultMOD XoaNND(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from NhomNguoiDung where NNDID = @NNDID";
                    cmd.Parameters.AddWithValue("@NNDID", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if(rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa nhóm người dùng thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "Không tìm thấy nhóm người dùng để xóa";
                    }
                }
            }catch(Exception ex)
            {
                result.Status= -1;
                result.Message = ULT.Constant.API_Error_System + ex;
                throw;

            }
            return result;
        }
      
    }
    

}
