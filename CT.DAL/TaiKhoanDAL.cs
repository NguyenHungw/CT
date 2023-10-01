using CT.MOD;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace CT.DAL
{
    public class TaiKhoanDAL
    {
        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
        SqlConnection SQLCon = null;



        public TaiKhoanModel LoginDAL(TaiKhoanMOD item)
        {

            
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "v1_Users_Login";
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    //cmd.Parameters.AddWithValue("Email",item.Email);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string hashedPasswordFromDB = reader.GetString(3);
                        if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPasswordFromDB))
                        {
                            var Result = new TaiKhoanModel();
                            // Result.Username = reader.GetString(0);
                            Result.PhoneNumber = reader.GetString(2);
                            Result.Password = hashedPasswordFromDB;
                            //Result.Email = item.Email;
                           // Result.RoleId = reader.GetInt32(5);
                            Result.isActive = reader.GetInt32(5);
                            return Result;
                        }
                    }

                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                //xu ly cac ngoai le o day
                Console.WriteLine("Caught exception: " + ex.Message);
                throw;
            }
            return null;
        }


        public BaseResultMOD RegisterDAL(DangKyTK item)
        {
            var Result = new BaseResultMOD();
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hash = BCrypt.Net.BCrypt.HashPassword(item.Password, salt);
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "v1_Users_Register";
                    cmd.Parameters.AddWithValue("@Username",item.Name );
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Password", hash);
                    cmd.Parameters.AddWithValue("@Email",item.Email);

                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();


                }

                Result.Status = 1;
                Result.Message = "Đăng ký thành công ";


            }
            catch (Exception ex)
            {
                throw;

            }
            return Result;

        }
        public DangKyTK inforTK(String Phonenumber)
        {
            DangKyTK item = null;
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [User] WHERE phonenumber = @PhoneNumber";
                    cmd.Parameters.AddWithValue("@PhoneNumber", Phonenumber);

                    cmd.Connection = SQLCon;
                    //lay dl va tra kq
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        item = new DangKyTK();
                        item.PhoneNumber = reader.GetString(1);
                    }
                    
                    reader.Close();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
            return item;

        }
        public BaseResultMOD CheckRoles(string phone)
        {
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachNhomND> dsnhomnd = new List<DanhSachNhomND>();
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "v1_User_Checkroles";


                    cmd.Parameters.AddWithValue("@PhoneNumber", phone);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    int count = 0;
                    while (reader.Read())
                    {
                        DanhSachNhomND item = new DanhSachNhomND();
                        item.idUser = reader.GetInt32(0);
                        item.Username = reader.GetString(1);
                        item.TenNND = reader.GetString(2); 
                        item.ChucNang = reader.GetString(3);
                        item.Xem = reader.GetSqlBoolean(4).Value;
                        item.Them = reader.GetSqlBoolean(5).Value;
                        item.Sua = reader.GetSqlBoolean(6).Value;
                        item.Xoa = reader.GetSqlBoolean(7).Value;

                        /*string xemValue = reader.GetString(3);
                        if (xemValue == "true" || xemValue == "false")
                        {
                            item.Xem = Boolean.Parse(xemValue); // Chuyển đổi thành kiểu bool
                        }*/


                        dsnhomnd.Add(item);
                        count++;
                    }

                    reader.Close();
                    if(count == 0)
                    {
                        result.Status = 0;
                        result.Message = "Chưa có role";
                    }
                    else
                    {
                        result.Status = 1;
                        result.Data = dsnhomnd;
                    }

                }
                
               
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }


        public BaseResultMOD DanhSachTK(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                List<TaiKhoanModel> ListAccounts = new List<TaiKhoanModel>();
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    const int ProductPerPage = 20; 
                    int startPage = ProductPerPage * (page - 1);
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    var item = new TaiKhoanModel();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "v1_Users_DanhSach_Active";
                    cmd.Parameters.AddWithValue("@startPage", startPage);
                    cmd.Parameters.AddWithValue("@productPerPage", ProductPerPage);
                    
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TaiKhoanModel model = new TaiKhoanModel();
                        model.idUser = reader.GetInt32(0);
                        model.Username = reader.GetString(1);
                        model.PhoneNumber = reader.GetString(2);
                        model.Password = reader.GetString(3);
                        model.Email = reader.GetString(4);
                        model.isActive = reader.GetInt32(5);
                        ListAccounts.Add(model);
                    }

                    reader.Close();


                }
                result.Status = 1;
                result.Data = ListAccounts;

            }
            catch (Exception ex)
            {
                throw;
            }
            return result;

        }
      

        public BaseResultMOD DoiMatKhau(DoiMK item)
        {
            var Result = new BaseResultMOD();
            try
            {
               
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hash = BCrypt.Net.BCrypt.HashPassword(item.Password, salt);
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = " UPDATE [User] SET password = @password WHERE phonenumber = @phonenumber";
                    cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@password", hash);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                }
                Result.Status = 1;
                Result.Message = "Đổi mật khẩu thành công";

            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "Đổi mật khẩu thất bại";
                throw;
            }
            return Result;
            
        }
        public BaseResultMOD DoiTen(Rename item)
        {
            var Result = new BaseResultMOD();
            try
            {
                using(SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText= "UPDATE [User] SET username = @username WHERE phonenumber = @phonenumber";
                    cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@username", item.Username);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                   
                    
                }
                Result.Status = 1;
                Result.Message = "Doi ten thanh cong";
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "Doi ten that bai";
                throw;
            }
            return Result;
        }

        public BaseResultMOD XoaTK(string sdt)
        {
            var Result = new BaseResultMOD();
            try
            {
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "v1_Users_Delete_ByPhonenum";
                    cmd.Parameters.AddWithValue("@phonenumber", sdt);
                    cmd.Connection = SQLCon;

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Result.Status = 1;
                        Result.Message = "Xóa tài khoản thành công";
                    }
                    else
                    {
                        Result.Status = -1;
                        Result.Message = "Không tìm thấy tài khoản để xóa";
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý các ngoại lệ ở đây và ghi log nếu cần
                Result.Status = -1;
                Result.Message = "Lỗi xóa tài khoản: " + ex.Message;
            }
            return Result;
        }


    }

}


