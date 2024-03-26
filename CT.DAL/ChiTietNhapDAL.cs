using CT.MOD;
using CT.ULT;
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
        //private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;";
        //SqlConnection SQLCon = null;
        public BaseResultMOD getDSChiTietNhap(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<ChiTietNhapMOD> dsctnhap = new List<ChiTietNhapMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT * from ChiTietNhap ORDER BY ID_ChiTietNhap
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ChiTietNhapMOD item = new ChiTietNhapMOD();
                        item.ID_ChiTietNhap = reader.GetInt32(0);
                        item.ID_PhieuNhap = reader.GetInt32(1);
                        item.MSanPham = reader.GetString(2);
                        item.SoLuong = reader.GetInt32(3);
                        item.TongSoLuong = reader.GetInt32(4);
                        item.DonGia = reader.GetDecimal(5);
                        item.ThanhTien = reader.GetDecimal(6);
                        dsctnhap.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctnhap;

                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD ThemChiTietNhap(ThemChiTietNhap item)
        {
            var result = new BaseResultMOD();
            try
            {
                bool checkMSanPham = KiemTraTrungMSanPham(item);
                bool checkIDPhieu = KiemTraTrungIDPhieu(item);
                if (!checkMSanPham)
                {
                    result.Status = -1;
                    result.Message = "Mã sản phẩm không tồn tại";
                }
                else if (!checkIDPhieu)
                {
                    result.Status = -1;
                    result.Message = "ID Phiếu nhập không tồn tại";
                }
                else
                {
                    // Thêm chức năng vào cơ sở dữ liệu
                    using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        SQLCon.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"Insert into ChiTietNhap (ID_PhieuNhap,MSanPham,SoLuong,TongSoLuong,DonGia,ThanhTien) VALUES(@ID_PhieuNhap,@MSanPham,@SoLuong,@SoLuong,@DonGia,@ThanhTien)";
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
              

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Update [ChiTietNhap] set ID_PhieuNhap =@ID_PhieuNhap,MSanPham=@MSanPham,SoLuong=@SoLuong,TongSoLuong=@SoLuong,DonGia=@DonGia,ThanhTien=@ThanhTien where ID_ChiTietNhap =@ID_ChiTietNhap";
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
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
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

        public BaseResultMOD GetDSKho(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<QuanLyKho> dsctnhap = new List<QuanLyKho>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT ct.MSanPham, sp.TenSanPham, gbsp.GiaBan, ct.SoLuong,ct.TongSoLuong , lsp.TenLoaiSP
                                        FROM ChiTietNhap ct
                                        LEFT JOIN SanPham sp ON ct.MSanPham = sp.MSanPham
                                        LEFT JOIN GiaBanSanPham gbsp ON sp.MSanPham = gbsp.MSanPham
                                        INNER JOIN LoaiSanPham lsp ON sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                        Order by ct.MSanPham
                                        OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QuanLyKho item = new QuanLyKho();
                        item.MSanPham = reader.IsDBNull(0) ? null : reader.GetString(0);
                        item.TenSanPham = reader.IsDBNull(1) ? null : reader.GetString(1);
                        item.GiaBan = reader.IsDBNull(2) ? (decimal?)null : reader.GetDecimal(2);
                        item.SoLuong = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                        item.TongSoLuong = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                        item.LoaiSanPham = reader.IsDBNull(5) ? null : reader.GetString(5);
                        dsctnhap.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctnhap;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        private bool KiemTraTrungMSanPham(ThemChiTietNhap item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM SanPham WHERE MSanPham = @MSanPham";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);

                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }
        private bool KiemTraTrungIDPhieu(ThemChiTietNhap item)
        {
            using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM PhieuNhap WHERE ID_PhieuNhap = @ID_PhieuNhap";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@ID_PhieuNhap", item.ID_PhieuNhap);

                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }
        public BaseResultMOD GetDSKhoSapHetHang(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<QuanLyKho> dsctnhap = new List<QuanLyKho>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT ct.MSanPham, sp.TenSanPham, gbsp.GiaBan, ct.SoLuong,ct.TongSoLuong, lsp.TenLoaiSP
                                        FROM ChiTietNhap ct
                                        INNER JOIN SanPham sp ON ct.MSanPham = sp.MSanPham
                                        INNER JOIN LoaiSanPham lsp ON sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                        join GiaBanSanPham gbsp on sp.MSanPham = gbsp.MSanPham
                                        WHERE ct.SoLuong > 0 AND ct.SoLuong < 10  
                                        ORDER BY ct.MSanPham
                                        OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;
                                        ";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QuanLyKho item = new QuanLyKho();
                        item.MSanPham = reader.GetString(0);
                        item.TenSanPham = reader.GetString(1);
                        item.GiaBan = reader.GetDecimal(2);
                        item.SoLuong = reader.GetInt32(3);
                        item.TongSoLuong = reader.GetInt32(4);
                        item.LoaiSanPham = reader.GetString(5);
                        dsctnhap.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctnhap;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD GetDSKhoDaHetHang(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {

                List<QuanLyKho> dsctnhap = new List<QuanLyKho>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT ct.MSanPham, sp.TenSanPham, gbsp.GiaBan, ct.SoLuong,ct.TongSoLuong, lsp.TenLoaiSP
                                        FROM ChiTietNhap ct
                                        INNER JOIN SanPham sp ON ct.MSanPham = sp.MSanPham
                                        INNER JOIN LoaiSanPham lsp ON sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
										join GiaBanSanPham gbsp on sp.MSanPham = gbsp.MSanPham
                                        WHERE ct.SoLuong <= 0 
                                        ORDER BY ct.MSanPham
                                        OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;
                                        ";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        QuanLyKho item = new QuanLyKho();
                        item.MSanPham = reader.GetString(0);
                        item.TenSanPham = reader.GetString(1);
                        item.GiaBan = reader.GetDecimal(2);
                        item.SoLuong = reader.GetInt32(3);
                        item.TongSoLuong = reader.GetInt32(4);
                        item.LoaiSanPham = reader.GetString(5);
                        dsctnhap.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctnhap;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }
        public BaseResultMOD GetDSPhieuNhapKho(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {

                List<DanhSachPhieuNhapKho> dsctnhap = new List<DanhSachPhieuNhapKho>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select pn.ID_PhieuNhap,sp.MSanPham,sp.TenSanPham,lsp.TenLoaiSP,pn.NgayNhap,ct.SoLuong,ct.SoLuong,nc.TenNhaCungCap,ct.DonGia,ct.ThanhTien
                                        from ChiTietNhap ct
                                        join SanPham sp on ct.MSanPham =sp.MSanPham
                                        join LoaiSanPham lsp on sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                        join PhieuNhap pn on ct.ID_PhieuNhap = pn.ID_PhieuNhap
                                        join NhaCungCap nc on pn.id_NhaCungCap = nc.id_NhaCungCap
                                        ORDER BY ct.MSanPham
                                        OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;
                                        ";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachPhieuNhapKho item = new DanhSachPhieuNhapKho();
                        item.ID_PhieuNhap = reader.GetInt32(0);
                        item.MSanPham = reader.GetString(1);
                        item.TenSanPham = reader.GetString(2);
                        item.TenLoaiSP = reader.GetString(3);
                        item.NgayNhap = reader.GetDateTime(4);
                        item.SoLuong = reader.GetInt32(5);
                        item.TongSoLuong = reader.GetInt32(6);
                        item.TenNhaCungCap = reader.GetString(7);
                        item.DonGia = reader.GetDecimal(8);
                        item.ThanhTien = reader.GetDecimal(9);
                        dsctnhap.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctnhap;
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

