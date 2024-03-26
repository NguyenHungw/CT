using CT.MOD;
using CT.ULT;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
    public class GioHangDAL
    {
       // private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        SqlConnection SQLCon = null;
     
        public BaseResultMOD GetDanhSachSP_Gio(int page)
        {
            var result = new BaseResultMOD();
            List<DanhSachGioHang> productCart = new List<DanhSachGioHang>();

            try
            {

                const int ProductPerPage = 20;
                int startPage = ProductPerPage * (page - 1);

                using (SqlConnection sqlCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    sqlCon.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType= CommandType.Text;
                        //cmd.CommandText = "v1_SanPham_DanhSach";
                        /* cmd.CommandText = @"SELECT sp.MSanPham, sp.Picture, sp.TenSanPham, lsp.TenLoaiSP
                                             FROM SanPham sp
                                             INNER JOIN LoaiSanPham lsp ON sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                             ORDER BY sp.id
                                             OFFSET @StartPage ROWS
                                             FETCH NEXT @ProductPerPage ROWS ONLY;";*/
                        cmd.CommandText = "Select * from GioHang";
                        cmd.Parameters.AddWithValue("@StartPage", startPage);
                        cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                        cmd.Connection = sqlCon;
                     

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DanhSachGioHang item = new DanhSachGioHang();
                                item.ID_GioHang = reader.GetInt32(0);
                                item.idUser = reader.GetInt32(1);
                                item.MSanPham = reader.GetString(2);
                                item.GioSoLuong = reader.GetInt32(3);

                              

                                productCart.Add(item);
                            }
                        }
                    }
                }

                result.Status = 1;
                result.Data = productCart;
            }
            catch (Exception ex)
            {
                result.Status = 0;
                result.Message = ex.Message;
                throw;

            }

            return result;
        }



        public BaseResultMOD ThemSP_Gio(ThemSP_Gio item)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
            
                SqlCommand sqlcmd = new SqlCommand();
            
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings)) {
                //sqlcmd.CommandType = CommandType.StoredProcedure;
                    //sqlcmd.CommandText = "v2_SanPham_ThemMoi";
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = " INSERT INTO GioHang (idUser,MSanPham,GioSoLuong) VALUES (@idUser,@MSanPham,@GioSoLuong)";
                    //sqlcmd.CommandText = " INSERT INTO SanPham (MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia) VALUES (@MSanPham, @Picture, @TenSanPham, @LoaiSanPham, @SoLuong, @DonGia)";
                sqlcmd.Connection = SQLCon;
                sqlcmd.Parameters.AddWithValue("@idUser", item.idUser);
                sqlcmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                    sqlcmd.Parameters.AddWithValue("@GioSoLuong", item.GioSoLuong);
             
  
                SQLCon.Open();
                sqlcmd.ExecuteNonQuery();
                SQLCon.Close();
                Result.Status = 1;
                Result.Message = "Thêm thành công";
                Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ULT.Constant.API_Error_System;
            }
            return Result;
        }
      

        public BaseResultMOD XoaSp_Gio(string msanpham,string idUser)
        {
            var Result = new BaseResultMOD();
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
                cmd.CommandText = "DELETE FROM GioHang where MSanPham= +'" + msanpham + "' and idUser= +'" + idUser + "' ";
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Xóa sản phẩm thành công";
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Không tìm thấy sản phẩm";

                }

            }
            catch (Exception)
            {
                throw;
            }
            return Result;

        }

        public BaseResultMOD XoaAllSP(string idUser)
        {
            var Result = new BaseResultMOD();
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
                cmd.CommandText = "DELETE FROM GioHang where idUser=@idUser";
                cmd.Parameters.AddWithValue("@idUser", idUser);
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Xóa sản phẩm thành công";
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Không có sản phẩm để xóa";

                }

            }
            catch (Exception)
            {
                throw;
            }
            return Result;

        }


        public SanPhamMOD ThongTinSp(string msp)
        {
            SanPhamMOD item = null;
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
                cmd.CommandText = "SELECT tensanpham from SanPham where MSanPham ='" + msp + "'";
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new SanPhamMOD();
                    item.TenSP = reader.GetString(0);
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return item;

        }

    }

}
