using CT.MOD;
using CT.ULT;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
    public class ChiTietUserDAL
    {


        public BaseResultMOD getDSChiTietUser(int page)
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachChiTietUserMOD> dsctuser = new List<DanhSachChiTietUserMOD>();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = SQLCon;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT * from ChiTietUsers ORDER BY id
                                OFFSET @StartPage ROWS
                                FETCH NEXT @ProductPerPage ROWS ONLY;";
                    cmd.Parameters.AddWithValue("@StartPage", startpage);
                    cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachChiTietUserMOD item = new DanhSachChiTietUserMOD();
                        item.id = reader.GetInt32(0);
                        item.idUser = reader.GetInt32(1);
                        //item.AvatarUser = reader.GetString(2);

                        // Kiểm tra xem cột có tồn tại và không phải là null
                        if (!reader.IsDBNull(2))
                        {
                            string AvatarUser = reader.GetString(2);

                            // Kiểm tra xem AvatarUser có giá trị null không
                            if (AvatarUser != null)
                            {
                                if (AvatarUser.EndsWith(".jpg") || AvatarUser.EndsWith(".png") || AvatarUser.EndsWith(".gif"))
                                {
                                    item.AvatarUser = "https://localhost:7177/" + AvatarUser;
                                }
                                else
                                {
                                    item.AvatarUser = AvatarUser;
                                }
                            }

                        }
                        else
                        {
                            // Xử lý trường hợp Avatar là null
                            item.AvatarUser = "Người Dùng Chưa Cập Nhật"; // hoặc bất kỳ giá trị mặc định nào khác bạn muốn
                        }

                        // Kiểm tra xem cột có tồn tại và không phải là null
                        if (!reader.IsDBNull(3))
                        {
                            item.FullName = reader.GetString(3);
                        }
                        else
                        {
                            item.FullName = "Người Dùng Chưa Cập Nhật";
                        }

                        if (!reader.IsDBNull(4))
                        {
                            item.NgaySinh = reader.GetDateTime(4);
                        }
                      
                        if (!reader.IsDBNull(5))
                        {
                            item.DiaChi = reader.GetString(5);
                        }
                        else
                        {
                            item.DiaChi = "Người Dùng Chưa Cập Nhật";
                        }

                        if (!reader.IsDBNull(6))
                        {
                            item.GioiTinh = reader.GetInt32(6);
                        }
                        else
                        {
                            item.GioiTinh = 3;
                        }
                       

                        dsctuser.Add(item);
                    }
                    reader.Close();
                    result.Status = 1;
                    result.Data = dsctuser;
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
            }
            return result;
        }

        public BaseResultMOD ThemIDuser(InsertUserMOD item)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {

                SqlCommand sqlcmd = new SqlCommand();
                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {

                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "INSERT INTO ChiTietUsers (idUser) VALUES (@idUser)";
                    sqlcmd.Connection = SQLCon;
                    sqlcmd.Parameters.AddWithValue("@idUser", item.idUser);



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
        public BaseResultMOD CapNhatChiTietUser(CapNhatChiTietUserMOD item, IFormFile file)
        {
            string Picture;
            var Result = new BaseResultMOD();
            try
            {
                SqlCommand sqlcmd = new SqlCommand();
                if (file.Length > 0)
                {
                    string fileName = $"{item.idUser}_{file.FileName}";
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

                // Xử lý chuyển đổi định dạng ngày tháng
                DateTime? ngaySinh = null;
                if (DateTime.TryParseExact(item.NgaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    ngaySinh = parsedDate;
                }
               /* else
                {
                    // Xử lý trường hợp ngày tháng không hợp lệ
                    // Ví dụ: Ghi log hoặc trả về thông báo lỗi cho người dùng
                }*/

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = "UPDATE ChiTietUsers SET AvatarUser=@AvatarUser,FullName=@FullName,NgaySinh=@NgaySinh,DiaChi=@DiaChi,GioiTinh=@GioiTinh where idUser=@idUser";
                    sqlcmd.Connection = SQLCon;
                    sqlcmd.Parameters.AddWithValue("@idUser", item.idUser);
                    sqlcmd.Parameters.AddWithValue("@AvatarUser", Picture);
                    sqlcmd.Parameters.AddWithValue("@FullName", item.FullName);
                    sqlcmd.Parameters.AddWithValue("@NgaySinh", SqlDbType.Date).Value = ngaySinh ?? (object)DBNull.Value; // Sử dụng SqlDbType.Date và DBNull.Value

                    sqlcmd.Parameters.AddWithValue("@DiaChi", item.DiaChi);
                    sqlcmd.Parameters.AddWithValue("@GioiTinh", item.GioiTinh);

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


    }
}