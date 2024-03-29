using CT.MOD;
using CT.MOD.ThongKeVaLichSuMOD;
using CT.MOD.TrangChuMOD;
using CT.ULT;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CT.DAL
{
	public class ThongKeDAL
	{
		//private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
        public BaseResultMOD TK_ChiTieu(TK_Date item)
        {
            var result = new BaseResultMOD();
            try
            {
                string query = "SELECT SUM(CTN.ThanhTien) AS TongDoanhThu " +
                               "FROM PhieuNhap PH " +
                               "INNER JOIN ChiTietNhap CTN ON PH.ID_PhieuNhap = CTN.ID_PhieuNhap " +
                               "WHERE YEAR(PH.NgayNhap) = @year ";

                if (item.Month.HasValue)
                {
                    query += "AND MONTH(PH.NgayNhap) = @month ";
                }
                /*if (item.Week.HasValue)
                {
                    query += "AND DATEPART(WEEK, PH.NgayNhap) = @Week ";
                }*/

                if (item.Day.HasValue)
                {
                    query += "AND DAY(PH.NgayNhap) = @day ";
                }

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    using (SqlCommand command = new SqlCommand(query, SQLCon))
                    {
                        command.Parameters.AddWithValue("@year", item.Year);

                        if (item.Month.HasValue)
                        {
                            command.Parameters.AddWithValue("@month", item.Month);
                        }
                       /* if (item.Week.HasValue)
                        {
                            command.Parameters.AddWithValue("@Week", item.Week);
                        }*/

                        if (item.Day.HasValue)
                        {
                            command.Parameters.AddWithValue("@day", item.Day);
                        }

                        // Thực hiện truy vấn và lấy kết quả
                        object queryResult = command.ExecuteScalar();

                        // Xử lý kết quả truy vấn
                        if (queryResult != null && queryResult != DBNull.Value)
                        {

                            var res = new TK_ChiTieu();
                            res.TongSoTien = Convert.ToInt32((decimal)queryResult);
                            // Giả sử TongDoanhThu là thuộc tính của đối tượng TK_ChiTieu

                            // Gán kết quả vào result.Data
                            result.Status = 1;
                            result.Data = res;
                        }
                        else
                        {
                            result.Status = 0;
                            result.Data = null; // Không cần gán giá trị null nếu kết quả là null
                        }
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

        public BaseResultMOD TK_SoLuongNhap(TK_Date item)
        {
            var result = new BaseResultMOD();
            try
            {
                string query = "SELECT SUM(CTN.TongSoLuong) AS SoLuongDaNhap " +
                               "FROM PhieuNhap PH " +
                               "INNER JOIN ChiTietNhap CTN ON PH.ID_PhieuNhap = CTN.ID_PhieuNhap " +
                               "WHERE YEAR(PH.NgayNhap) = @year ";

                if (item.Month.HasValue)
                {
                    query += "AND MONTH(PH.NgayNhap) = @month ";
                }
                /*if (item.Week.HasValue)
                {
                    query += "AND DATEPART(WEEK, PH.NgayNhap) = @Week ";
                }*/

                if (item.Day.HasValue)
                {
                    query += "AND DAY(PH.NgayNhap) = @day ";
                }

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    using (SqlCommand command = new SqlCommand(query, SQLCon))
                    {
                        command.Parameters.AddWithValue("@year", item.Year);

                        if (item.Month.HasValue)
                        {
                            command.Parameters.AddWithValue("@month", item.Month);
                        }
                        /* if (item.Week.HasValue)
                         {
                             command.Parameters.AddWithValue("@Week", item.Week);
                         }*/

                        if (item.Day.HasValue)
                        {
                            command.Parameters.AddWithValue("@day", item.Day);
                        }

                        // Thực hiện truy vấn và lấy kết quả
                        object queryResult = command.ExecuteScalar();

                        // Xử lý kết quả truy vấn
                        if (queryResult != null && queryResult != DBNull.Value)
                        {

                            var res = new TK_SoLuong();
                            res.SoLuong = Convert.ToInt32(queryResult);
                            // Giả sử TongDoanhThu là thuộc tính của đối tượng TK_ChiTieu

                            // Gán kết quả vào result.Data
                            result.Status = 1;
                            result.Data = res;
                        }
                        else
                        {
                            result.Status = 0;
                            result.Data = null; // Không cần gán giá trị null nếu kết quả là null
                        }
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


        public BaseResultMOD TK_SoLuongDaBan(TK_Date item)
        {
            var result = new BaseResultMOD();
            try
            {
                string query = "SELECT SUM(CTDH.SoLuong) as SoLuongDaBan " +
                               "FROM ChiTietDonHang CTDH " +
                               "inner join DonHang DH on CTDH.OrderID = DH.OrderID " +
                               "where Year(DH.NgayMua) = @year ";

                if (item.Month.HasValue)
                {
                    query += "AND MONTH(PH.NgayNhap) = @month ";
                }
                /*if (item.Week.HasValue)
                {
                    query += "AND DATEPART(WEEK, PH.NgayNhap) = @Week ";
                }*/

                if (item.Day.HasValue)
                {
                    query += "AND DAY(PH.NgayNhap) = @day ";
                }

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    using (SqlCommand command = new SqlCommand(query, SQLCon))
                    {
                        command.Parameters.AddWithValue("@year", item.Year);

                        if (item.Month.HasValue)
                        {
                            command.Parameters.AddWithValue("@month", item.Month);
                        }
                        /* if (item.Week.HasValue)
                         {
                             command.Parameters.AddWithValue("@Week", item.Week);
                         }*/

                        if (item.Day.HasValue)
                        {
                            command.Parameters.AddWithValue("@day", item.Day);
                        }

                        // Thực hiện truy vấn và lấy kết quả
                        object queryResult = command.ExecuteScalar();

                        // Xử lý kết quả truy vấn
                        if (queryResult != null && queryResult != DBNull.Value)
                        {

                            var res = new TK_SoLuong();
                            res.SoLuong = Convert.ToInt32(queryResult);
                            // Giả sử TongDoanhThu là thuộc tính của đối tượng TK_ChiTieu

                            // Gán kết quả vào result.Data
                            result.Status = 1;
                            result.Data = res;
                        }
                        else
                        {
                            result.Status = 0;
                            result.Data = null; // Không cần gán giá trị null nếu kết quả là null
                        }
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

        public BaseResultMOD TK_DoanhThu(TK_Date item)
        {
            var result = new BaseResultMOD();
            try
            {
                string query = "SELECT SUM(CTDH.ThanhTien) as DoanhThu " +
                               "FROM ChiTietDonHang CTDH " +
                               "inner join DonHang DH on CTDH.OrderID = DH.OrderID " +
                               "where Year(DH.NgayMua) = @year ";

                if (item.Month.HasValue)
                {
                    query += "AND MONTH(PH.NgayNhap) = @month ";
                }
                /*if (item.Week.HasValue)
                {
                    query += "AND DATEPART(WEEK, PH.NgayNhap) = @Week ";
                }*/

                if (item.Day.HasValue)
                {
                    query += "AND DAY(PH.NgayNhap) = @day ";
                }

                using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    SQLCon.Open();
                    using (SqlCommand command = new SqlCommand(query, SQLCon))
                    {
                        command.Parameters.AddWithValue("@year", item.Year);

                        if (item.Month.HasValue)
                        {
                            command.Parameters.AddWithValue("@month", item.Month);
                        }
                        /* if (item.Week.HasValue)
                         {
                             command.Parameters.AddWithValue("@Week", item.Week);
                         }*/

                        if (item.Day.HasValue)
                        {
                            command.Parameters.AddWithValue("@day", item.Day);
                        }

                        // Thực hiện truy vấn và lấy kết quả
                        object queryResult = command.ExecuteScalar();

                        // Xử lý kết quả truy vấn
                        if (queryResult != null && queryResult != DBNull.Value)
                        {

                            var res = new TK_DoanhThuMOD();
                            res.DoanhThu = Convert.ToInt32(queryResult);
                            // Giả sử TongDoanhThu là thuộc tính của đối tượng TK_ChiTieu

                            // Gán kết quả vào result.Data
                            result.Status = 1;
                            result.Data = res;
                        }
                        else
                        {
                            result.Status = 0;
                            result.Data = null; // Không cần gán giá trị null nếu kết quả là null
                        }
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
