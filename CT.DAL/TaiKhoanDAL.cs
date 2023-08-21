﻿using CT.MOD;
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
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT username, phonenumber, password,role FROM TaiKhoan WHERE phonenumber = @PhoneNumber";
                    cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                    cmd.Connection = SQLCon;

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string hashedPasswordFromDB = reader.GetString(2);
                        if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPasswordFromDB))
                        {
                            var Result = new TaiKhoanModel();
                           // Result.Username = reader.GetString(0);
                            Result.PhoneNumber = reader.GetString(1);
                           // Result.role = reader.GetInt32(3);
                            return Result;
                        }
                    }

                    reader.Close();
                }

            }

            catch (Exception ex)
            {

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
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO TaiKhoan (UserName, PhoneNumber, Email, Password, role) VALUES ('" + item.Name + "', '" + item.PhoneNumber + "', '" + item.Email + "', '" + hash + "', 1)";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();


                }

                Result.Status = 1;
                Result.Messeage = "Dang ki thanh cong ";


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
                    cmd.CommandText = "SELECT * FROM TaiKhoan WHERE phonenumber = @PhoneNumber";
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
        public BaseResultMOD function(int quyen)
        {
            var result = new BaseResultMOD();
            try
            {
                List<DanhSachChucNang> dschucnang = new List<DanhSachChucNang>();
                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select Mchucnang,TenChucNang,quyen from phanquyen where quyen = @quyen ";
                    cmd.Parameters.AddWithValue("@quyen", quyen);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        DanhSachChucNang item = new DanhSachChucNang();
                        item.TenChucNang = reader.GetString(1);
                        dschucnang.Add(item);

                    }
              
                    reader.Close();
                }
                result.Status = 1;
                result.Data = dschucnang;

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
                    cmd.CommandText = "select username, phonenumber, email , role from TaiKhoan order by id offset " + startPage + " rows fetch next " + ProductPerPage + " rows only ";
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TaiKhoanModel model = new TaiKhoanModel();
                        model.Email = reader.GetString(0);
                        model.Username = reader.GetString(1);
                        model.PhoneNumber = reader.GetString(2);
                       
                        model.role = reader.GetInt32(3);
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
                    cmd.CommandText = " UPDATE TaiKhoan SET password = @password WHERE phonenumber = @phonenumber";
                    cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@password", hash);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                }
                Result.Status = 1;
                Result.Messeage = "Doi mk thanh cong";

            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Messeage = "doi mk that bai";
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
                    cmd.CommandText="UPDATE TaiKhoan SET username = @username WHERE phonenumber = @phonenumber";
                    cmd.Parameters.AddWithValue("@phonenumber", item.PhoneNumber);
                    cmd.Parameters.AddWithValue("@username", item.Username);
                    cmd.Connection = SQLCon;
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();
                   
                    
                }
                Result.Status = 1;
                Result.Messeage = "Doi ten thanh cong";
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Messeage = "Doi ten that bai";
                throw;
            }
            return Result;
        }

        public BaseResultMOD XoaTK (string sdt)
        {
            var Result = new BaseResultMOD();
            try
            {
                if(SQLCon == null)
                {
                    SQLCon = new SqlConnection(strcon);
                }if(SQLCon.State == ConnectionState.Closed)
                {
                    SQLCon.Open();
                }
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM TaiKhoan WHERE phonenumber = @phonenumber";
                cmd.Parameters.AddWithValue("@phonenumber",sdt);
                cmd.Connection = SQLCon;
                cmd.ExecuteNonQuery ();
                if(SQLCon != null)
                {
                    Result.Status = 1;
                    Result.Messeage = "xoa tk thanh cong";

                }
                else
                {
                    Result.Status = -1;
                    Result.Messeage = "vui long kiem tra lai sdt";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
           
        }

    }

}


