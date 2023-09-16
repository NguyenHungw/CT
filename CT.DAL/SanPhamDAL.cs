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
                    cmd.CommandText = "SELECT MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia FROM SanPham ORDER BY id OFFSET @StartPage ROWS FETCH NEXT @ProductPerPage ROWS ONLY";
                    cmd.Parameters.AddWithValue("@StartPage", startPage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", ProductPerPage);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachModel Result = new DanhSachModel();


                        Result.MSanPham = reader.GetString(0);
                        Result.Picture = "https://localhost:7177/" + reader.GetString(1);
                        Result.TenSP = reader.GetString(2);
                        Result.LoaiSanPham = reader.GetString(3);
                        Result.SoLuong = reader.GetInt32(4);
                        Result.DonGia = (float)reader.GetDecimal(5);

                        dssp.Add(Result);


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
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia FROM SanPham ORDER BY id OFFSET @StartPage ROWS FETCH NEXT @ProductPerPage ROWS ONLY";
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
                                item.LoaiSanPham = reader.GetString(3);
                                item.SoLuong = reader.GetInt32(4);
                                item.DonGia = (float)reader.GetDecimal(5);
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
                result.Messeage = ex.Message;
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
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", file.FileName);
                    using (Stream stream = System.IO.File.Create(path))
                    {
                        file.CopyTo(stream);

                    }
                     Picture = "/upload/" + file.FileName;
                    //Picture = file.FileName;

                }
                else
                {
                    Picture = "";
                }
                using (SqlConnection SQLCon = new SqlConnection(strcon)) { 
                    sqlcmd.CommandText = "insert into SanPham (MSanPham, Picture, TenSanPham, LoaiSanPham, SoLuong, DonGia)values(@MSanPham, @Picture, @TenSanPham, @LoaiSanPham, @SoLuong ,@DonGia)";
                sqlcmd.Connection = SQLCon;
                sqlcmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
                sqlcmd.Parameters.AddWithValue("@Picture", Picture);
                sqlcmd.Parameters.AddWithValue("@TenSanPham", item.TenSP);
                sqlcmd.Parameters.AddWithValue("@LoaiSanPham", item.LoaiSanPham);
                sqlcmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                sqlcmd.Parameters.AddWithValue("@DonGia", item.DonGia);
                SQLCon.Open();
                sqlcmd.ExecuteNonQuery();
                SQLCon.Close();
                Result.Status = 1;
                Result.Messeage = "Them sp thanh cong";
                Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Messeage = "Them sp that bai";

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
                    sqlcmd.Parameters.AddWithValue("@SoLuong", item.SoLuong);
                    sqlcmd.Parameters.AddWithValue("@DonGia", item.DonGia);

                    SQLCon.Open();
                    sqlcmd.ExecuteNonQuery();
                    SQLCon.Close();

                    Result.Status = 1;
                    Result.Messeage = "Thêm sản phẩm thành công";
                    Result.Data = 1;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Messeage = "Thêm sản phẩm thất bại";
                Result.Messeage = "Caught exception: " + ex.Message;
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
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", file.FileName);
                        using (Stream stream = System.IO.File.Create(path))
                        {
                            file.CopyTo(stream);

                        }
                        Picture = "/upload/" + file.FileName;

                    }
                    else
                    {
                        Picture = "";

                    }
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "UPDATE [SanPham] SET Picture=@Picture ,TenSanPham=@TenSanPham, LoaiSanPham=@LoaiSanPham, SoLuong=@Soluong, DonGia=@Dongia WHERE MSanPham=@MSanPham";
                    sqlcmd.Connection = SQLCon;

                    sqlcmd.Parameters.AddWithValue("@Picture", Picture);
                    sqlcmd.Parameters.AddWithValue("@TenSanPham", editsp.TenSP);
                    sqlcmd.Parameters.AddWithValue("@LoaiSanPham", editsp.LoaiSanPham);
                    sqlcmd.Parameters.AddWithValue("@SoLuong", editsp.SoLuong);
                    sqlcmd.Parameters.AddWithValue("@DonGia", editsp.DonGia);
                    sqlcmd.Parameters.AddWithValue("@MSanPham", editsp.MSanPham);

                    sqlcmd.ExecuteNonQuery();
                 
                        Result.Status = 1;
                        Result.Messeage = "Chinh sua thong tin thanh cong";
                        Result.Data = 1;
                }

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Messeage = "San pham ko ton tai";
                
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
                    Result.Messeage = "xoa sp thanh cong";
                }
                else
                {
                    Result.Status = -1;
                    Result.Messeage = "vui long kiem tra lai ma sp";

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
                    Result.Messeage = "xoa sp thanh cong";
                }
                else
                {
                    Result.Status = -1;
                    Result.Messeage = "Ko co sp de xoa";

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
                    item.SoLuong = reader.GetInt32(5);
                    item.DonGia = (float)reader.GetDecimal(6);
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
                        item.LoaiSanPham = reader.GetString(3);
                        item.SoLuong = reader.GetInt32(4);
                        item.DonGia = (float)reader.GetDecimal(5);
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
