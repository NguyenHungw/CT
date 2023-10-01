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
    public class NguoiDungTrongNhomDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";

        public BaseResultMOD getdsndtn(int page)
        {
            const int ProductPerPage = 10;
            int startpage = ProductPerPage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<NguoiDungTrongNhomMOD> dsndtn = new List<NguoiDungTrongNhomMOD>();
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType=CommandType.Text;
                    cmd.CommandText = "Select * from NguoiDungTrongNhom";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        NguoiDungTrongNhomMOD item = new NguoiDungTrongNhomMOD();
                        item.NNDID = reader.GetInt32(0);
                        item.idUser = reader.GetInt32(1);
                        dsndtn.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsndtn;
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                throw ex;
            }
            return result;
        }
        public BaseResultMOD ThemNDvaoNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO NguoiDungTrongNhom (NNDID, idUser) VALUES (@NNDID, @idUser)";
                    cmd.Connection = SQLCon;
                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@idUser", item.idUser);
                    SQLCon.Open();
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Them thanh cong";
                    result.Data = 1;
                    
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = "Lỗi hệ thống"+ex;

            }
            return result;
        }
        public BaseResultMOD SuaNDtrongNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update set NNDID =@NNDID where idUser = @idUser";
                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@idUser", item.idUser);
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa thành công";
                    result.Data = 1;
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = "Lỗi hệ thống";
            }
            return result;
        }
        public BaseResultMOD XoaNDKhoiNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from NguoiDungTrongNhom where NNDID=@NNDID and idUser=@idUser ";
                    cmd.Parameters.AddWithValue("@NNDID", item.NNDID);
                    cmd.Parameters.AddWithValue("@idUser", item.idUser);
                    cmd.Connection = SQLCon;
                    int rowsaffected = cmd.ExecuteNonQuery();
                    if (rowsaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa thành công";

                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "ID không hợp lệ";

                    }

                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.ERR_DELETE;


            }
            return result;

        }

    }
}
