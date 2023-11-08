using CT.MOD;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
    public class PhieuNhapDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        SqlConnection SQLCon = null;

        public BaseResultMOD getDSPhieuNhap(int page)
        {
            var result = new BaseResultMOD();
            const int Producperpage = 20;
            int startpage = Producperpage * (page - 1);
            try
            {

                List<PhieuNhapMOD> dspn = new List<PhieuNhapMOD>();
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT *
										FROM PhieuNhap
										ORDER BY ID_PhieuNhap
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Producperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        PhieuNhapMOD item = new PhieuNhapMOD();
                        item.ID_PhieuNhap = reader.GetInt32(0);
                        item.NgayNhap = reader.GetDateTime(1);
                        item.NguoiNhapHang = reader.GetString(2);
                        item.TongGiaTri = reader.GetDecimal(3);
                        item.ID_DonVi = reader.GetInt32(4);
                        dspn.Add(item);


                    }reader.Close();
                    result.Status = 1;
                    result.Data=dspn;

                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
          
        }

        public BaseResultMOD ThemPhieuNhap(ThemMoiPhieuNhapMOD item)
        {
            var result = new BaseResultMOD();
            
            try
            {

                    using (SqlConnection SQLCon = new SqlConnection(strcon)){
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "INSERT INTO PhieuNhap (NgayNhap,NguoiNhapHang,TongGiaTri,ID_DonVi) VALUES(@NgayNhap,@NguoiNhapHang,@TongGiaTri,@ID_DonVi)";
                    cmd.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NguoiNhapHang", item.NguoiNhapHang);
                    cmd.Parameters.AddWithValue("@TongGiaTri", item.TongGiaTri);
                    cmd.Parameters.AddWithValue("@ID_DonVi", item.ID_DonVi);
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Thêm phiếu nhập thành công";
                    result.Data = 1;

                }
            }
            catch(Exception ex)
            {
                result.Status = 1;
                result.Message = ULT.Constant.API_Error_System;

            }
            return result;
            

        }
        public idPhieuNhapMOD ThongTinPhieuNhap(int id)
        {
            idPhieuNhapMOD item = null;

            try
            {
                if (SQLCon == null)
                {
                    SQLCon = new SqlConnection(strcon);

                }
                if (SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT ID_PhieuNhap from PhieuNhap where ID_PhieuNhap ='" + id + "'";
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new idPhieuNhapMOD();
                    item.ID_PhieuNhap = reader.GetInt32(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return item;
        }

        public BaseResultMOD SuaPhieuNhap(PhieuNhapMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "UPDATE [PhieuNhap] SET NgayNhap=@NgayNhap ,NguoiNhapHang=@NguoiNhapHang ,TongGiaTri=@TongGiaTri ,ID_DonVi=@ID_DonVi WHERE ID_PhieuNhap=@ID_PhieuNhap";
                    cmd.Parameters.AddWithValue("@NgayNhap", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NguoiNhapHang", item.NguoiNhapHang);
                    cmd.Parameters.AddWithValue("@TongGiaTri", item.TongGiaTri);
                    cmd.Parameters.AddWithValue("@ID_DonVi", item.ID_DonVi);
                    cmd.Parameters.AddWithValue("@ID_PhieuNhap", item.ID_PhieuNhap);
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Chỉnh sửa thông tin thành công";
                    result.Data = 1;

                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
       public BaseResultMOD XoaPhieuNhap(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "DELETE from PhieuNhap Where ID_PhieuNhap=@ID_PhieuNhap";
                    cmd.Parameters.AddWithValue("@ID_PhieuNhap", id);
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Xóa thành công";
                    
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
    }
}
