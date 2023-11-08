using CT.MOD;
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
    public class SanPhamDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        SqlConnection SQLCon = null;
        public List<DanhSachModel> DSDAL(int page)
        {

            List<DanhSachModel> dssp = new List<DanhSachModel>();

            try
            {
                const int ProductPerPage = 20;
                int startPage = ProductPerPage * (page - 1);

                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    // cmd.CommandText = "SELECT * FROM SanPham";
                    cmd.CommandText = "SELECT MSanPham, Picture, TenSanPham, ID_LoaiSanPham FROM SanPham ORDER BY id OFFSET @StartPage ROWS FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachModel item = new DanhSachModel();


                        item.MSanPham = reader.GetString(0);
                        item.Picture = "https://localhost:7177/" + reader.GetString(1);
                        item.TenSP = reader.GetString(2);
                        item.LoaiSanPham = reader.GetInt32(3);
                      
                        dssp.Add(item);
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return dssp;
        }
        public BaseResultMOD GetDanhSachSP(int page)
        {
            var result = new BaseResultMOD();
            List<DanhSachSP> productList = new List<DanhSachSP>();

            try
            {

                const int ProductPerPage = 20;
                int startPage = ProductPerPage * (page - 1);

                using (SqlConnection sqlCon = new SqlConnection(strcon))
                {
                    sqlCon.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType= CommandType.Text;
                        //cmd.CommandText = "v1_SanPham_DanhSach";
                        cmd.CommandText = @"SELECT sp.MSanPham, sp.Picture, sp.TenSanPham, lsp.TenLoaiSP
                                            FROM SanPham sp
                                            INNER JOIN LoaiSanPham lsp ON sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                            ORDER BY sp.id
                                            OFFSET @StartPage ROWS
                                            FETCH NEXT @ProductPerPage ROWS ONLY;";
                        cmd.Parameters.AddWithValue("@StartPage", startPage);
                        cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                        cmd.Connection = sqlCon;
                     

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DanhSachSP item = new DanhSachSP();
                                item.MSanPham = reader.GetString(0);
                                string picture = reader.GetString(1);
                                if (picture.EndsWith(".jpg") || picture.EndsWith(".png") || picture.EndsWith(".gif"))
                                {
                                    item.Picture = "https://localhost:7177/" + reader.GetString(1);
                                }
                                else
                                {
                                    // nếu ko phải là kiểu ảnh thì là base64
                                  
                                    item.Picture = reader.GetString(1);
                                }
                                //item.Picture =  reader.GetString(1);
                                item.TenSP = reader.GetString(2);
                                item.TenLoaiSP = reader.GetString(3);
                              
                                productList.Add(item);
                            }
                        }
                    }
                }

                result.Status = 1;
                result.Data = productList;
            }
            catch (Exception ex)
            {
                result.Status = 0;
                result.Message = ex.Message;
                throw;

            }

            return result;
        }



        public BaseResultMOD ThemSP(SanPhamMOD item, IFormFile file)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
            
                SqlCommand sqlcmd = new SqlCommand();
                if (file.Length > 0)
                {
                    string fileName = $"{item.MSanPham}_{file.FileName}";
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (Stream stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);

                    }
                     Picture = "/upload/" + fileName;
                    //Picture = file.FileName;

                }
                else
                {
                    Picture = "";
                }
                using (SqlConnection SQLCon = new SqlConnection(strcon)) {
                //sqlcmd.CommandType = CommandType.StoredProcedure;
                    //sqlcmd.CommandText = "v2_SanPham_ThemMoi";
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = " INSERT INTO SanPham (MSanPham, Picture, TenSanPham, ID_LoaiSanPham) VALUES (@MSanPham, @Picture, @TenSanPham, @ID_LoaiSanPham)";
                    //sqlcmd.CommandText = " INSERT INTO SanPham (MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia) VALUES (@MSanPham, @Picture, @TenSanPham, @LoaiSanPham, @SoLuong, @DonGia)";
                    sqlcmd.Connection = SQLCon;
                sqlcmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                sqlcmd.Parameters.AddWithValue("@Picture", Picture);
                sqlcmd.Parameters.AddWithValue("@TenSanPham", item.TenSP);
                sqlcmd.Parameters.AddWithValue("@ID_LoaiSanPham", item.LoaiSanPham);
  
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
        public BaseResultMOD ThemSPBase64(SanPhamMOD item, IFormFile file)
        {
            var Result = new BaseResultMOD();
            try
            {
                string Picture = "";

                // Kiểm tra xem 'file' và 'item' có tồn tại
                if (file != null && file.Length > 0 && item != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        // Chuyển chuỗi base64 thành mảng byte
                        byte[] imageBytes = memoryStream.ToArray();
                        Picture = Convert.ToBase64String(imageBytes);

                        // Lưu ảnh vào thư mục tạm thời
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", "temp", file.FileName);
                        System.IO.File.WriteAllBytes(imagePath, imageBytes);
                    }
                }

                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandText = "INSERT INTO SanPham (MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia) VALUES (@MSanPham, @Picture, @TenSanPham, @LoaiSanPham, @SoLuong, @DonGia)";
                    sqlcmd.Connection = SQLCon;
                    sqlcmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                    sqlcmd.Parameters.AddWithValue("@Picture", Picture);
                    sqlcmd.Parameters.AddWithValue("@TenSanPham", item.TenSP);
                    sqlcmd.Parameters.AddWithValue("@LoaiSanPham", item.LoaiSanPham);
              

                    SQLCon.Open();
                    sqlcmd.ExecuteNonQuery();
                    SQLCon.Close();

                    Result.Status = 1;
                    Result.Message = "Thêm sản phẩm thành công";
                    Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "Thêm sản phẩm thất bại";
                Result.Message = "Caught exception: " + ex.Message;
            }

            return Result;
        }



        public BaseResultMOD SuaSP(SanPhamMOD editsp, IFormFile file)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    if (file.Length > 0)
                    {
                        string fileName = $"{editsp.MSanPham}_{file.FileName}";
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
                        string filePath = Path.Combine(uploadsFolder, fileName);
                        using (Stream stream = System.IO.File.Create(filePath))
                        {
                            file.CopyTo(stream);

                        }
                        Picture = "/upload/" + fileName;

                    }
                    else
                    {
                        Picture = "";

                    }
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "UPDATE [SanPham] SET Picture=@Picture ,TenSanPham=@TenSanPham,ID_LoaiSanPham=@ID_LoaiSanPham,WHERE MSanPham=@MSanPham";
                    sqlcmd.Connection = SQLCon;

                    sqlcmd.Parameters.AddWithValue("@Picture", Picture);
                    sqlcmd.Parameters.AddWithValue("@TenSanPham", editsp.TenSP);
                    sqlcmd.Parameters.AddWithValue("@ID_LoaiSanPham", editsp.LoaiSanPham);
                
                    sqlcmd.Parameters.AddWithValue("@MSanPham", editsp.MSanPham);
                    sqlcmd.ExecuteNonQuery();
                 
                        Result.Status = 1;
                        Result.Message = "Chỉnh sửa thông tin thành công";
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
        public BaseResultMOD XoaSp(string msanpham)
        {
            var Result = new BaseResultMOD();
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
                cmd.CommandText = "DELETE FROM SanPham where MSanPham= +'" + msanpham + "'";
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "xoa sp thanh cong";
                }
                else
                {
                    Result.Status = -1;
                    Result.Message = "vui long kiem tra lai ma sp";

                }

            }
            catch (Exception)
            {
                throw;
            }
            return Result;

        }

        public BaseResultMOD XoaAllSP()
        {
            var Result = new BaseResultMOD();
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
                cmd.CommandText = "DELETE FROM SanPham";
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery();
                if (SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Message = "Xóa sản phẩm thành công";
                }
                else
                {
                    Result.Status = -1;
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
                    SQLCon = new SqlConnection(strcon);

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
        public ChiTietSP CTSP(string msp)
        {
            ChiTietSP item = null;
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
                cmd.CommandText = " SELECT * FROM SanPham WHERE MSanPham=@MSanPham  ";
                cmd.Parameters.AddWithValue("@MSanPham", msp);
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new ChiTietSP();
                    item.id = reader.GetInt32(0);
                    item.MSanPham = msp;

                    item.Picture = reader.GetString(2);
                    item.TenSP = reader.GetString(3);
                    item.LoaiSanPham = reader.GetString(4);
              
                }
                reader.Close();


            }
            catch (Exception)
            {
                throw;
            }
            return item;
        }

        public TimSp TBNM(string name)
        {
            var Result = new TimSp();
            try
            {

                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT MSanPham , Picture , LoaiSanPham , SoLuong  , DonGia  FROM SanPham WHERE TenSanPham = @TenSanPham";

                    cmd.Parameters.AddWithValue("@TenSanPham", name);

                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {

                        Result.MSanPham = reader.GetString(0);
                        Result.Picture = "localhost:7177/" + reader.GetString(1);
                        Result.TenSP = reader.GetString(2);
                        //    Result.LoaiSanPham = reader.GetString(4);
                        /* Result.SoLuong = reader.GetInt32(5);
                         Result.DonGia = (float)reader.GetDecimal(6);*/


                    }
                    reader.Close();


                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return Result;
        }

        public TimSp SearchByName(string name)
        {
            var Result = new TimSp();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT MSanPham, Picture, LoaiSanPham, SoLuong, DonGia FROM SanPham WHERE TenSanPham = @TenSanPham";
                    cmd.Parameters.AddWithValue("@TenSanPham", name);
                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Result.MSanPham = reader.GetString(0);
                        Result.Picture = reader.GetString(1);
                        Result.TenSP = name;
                        Result.LoaiSanPham = reader.GetString(2);
                        Result.SoLuong = reader.GetInt32(3);
                        Result.DonGia = (float)reader.GetDecimal(4);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Result;
        }


        public BaseResultMOD DanhSachSPbyTypeSP(string loaisp, int page)
        {
            var Result = new BaseResultMOD();
            const int productperpage = 20;
            int startpage = productperpage * (page - 1);
            List<DanhSachSP> Danhsachloaisp = new List<DanhSachSP>();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select msanpham,picture,tensanpham,loaisanpham,soluong,dongia from SanPham where loaisanpham = '" 
                        + loaisp + "'order by id offset " + startpage + " rows fetch next " + productperpage + " rows only";

                    cmd.Connection = SQLCon;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachSP item = new DanhSachSP();
                        item.MSanPham = reader.GetString(0);
                        item.Picture = reader.GetString(1);
                        item.TenSP = reader.GetString(2);
                        item.TenLoaiSP = reader.GetString(3);
                  
                        Danhsachloaisp.Add(item);

                    }
                    reader.Close();
                }
                Result.Status = -1;
                Result.Data = Danhsachloaisp;
            }
            catch (Exception)
            {
                throw;

            }
            return Result;
        }

       
    }

}
