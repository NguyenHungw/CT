using CT.MOD;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
    public class ChiTietNhapDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;";
        SqlConnection SQLCon = null;
        public BaseResultMOD getDSChiTietNhap(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChiTietNhapMOD> dsctnhap = new List<ChiTietNhapMOD>();
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT * from ChiTietNhap ORDER BY ID_ChiTietNhap
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";
                    cmd.Parameters.AddWithValue("@StartPage",startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage",Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietNhapMOD item = new ChiTietNhapMOD();
                        item.ID_ChiTietNhap = reader.GetInt32(0);
                        item.ID_PhieuNhap = reader.GetInt32(1);
                        item.MSanPham = reader.GetString(2);
                        item.SoLuong = reader.GetInt32(3);
                        item.DonGia = reader.GetDecimal(4);
                        item.ThanhTien = reader.GetDecimal(5);
                        dsctnhap.Add(item);
                    }reader.Close();
                    result.Status = 1;
                    result.Data=dsctnhap;

                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD ThemChiTietNhap(ThemChiTietNhap2 item)
        {
            var result = new BaseResultMOD();
            try
            {
                // Thêm chức năng vào cơ sở dữ liệu
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Insert into ChiTietNhap (ID_PhieuNhap,MSanPham,SoLuong,DonGia,ThanhTien) VALUES(@ID_PhieuNhap,@MSanPham,@SoLuong,@DonGia,@ThanhTien)";
                    cmd.Parameters.AddWithValue("@ID_PhieuNhap", item.ID_PhieuNhap);
                    cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", item.DonGia);
                    cmd.Parameters.AddWithValue("@ThanhTien", item.SoLuong * item.DonGia);

                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    result.Status = 1;
                    result.Message = "Thêm thành công";
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

        public BaseResultMOD SuaChiTietPhieuNhap(SuaChiTietNhapMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [ChiTietNhap] set ID_PhieuNhap =@ID_PhieuNhap,MSanPham=@MSanPham,SoLuong=@SoLuong,DonGia=@DonGia,ThanhTien=@ThanhTien where ID_ChiTietNhap =@ID_ChiTietNhap";
                    cmd.Parameters.AddWithValue("@ID_PhieuNhap", item.ID_PhieuNhap);
                    cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                    cmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", item.DonGia);
                    cmd.Parameters.AddWithValue("@ThanhTien", item.SoLuong * item.DonGia);
                    cmd.Parameters.AddWithValue("@ID_ChiTietNhap", item.ID_ChiTietNhap);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();

                    result.Status = 1;
                    result.Message = "Sửa thành công";
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
        public BaseResultMOD XoaChiTietNhap(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Delete from ChiTietNhap where ID_ChiTietNhap = @ID_ChiTietNhap";
                    cmd.Parameters.AddWithValue("@ID_ChiTietNhap", id);
                    cmd.Connection = SQLCon;
                    int rowaffected = cmd.ExecuteNonQuery();
                    if (rowaffected > 0)
                    {
                        result.Status = 1;
                        result.Message = "Xóa thành công";
                    }
                    else
                    {
                        result.Status = 0;
                        result.Message = "{id} không hợp lệ";

                    }
                }

            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
    }
   

}

