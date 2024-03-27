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

namespace CT.DAL
{
	public class LichSuDonHangUserDAL
	{
		//private string SQLHelper.appConnectionStrings = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getLS_MuaHang_User(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<LichSuDonHangUserMOD> lsdhuser = new List<LichSuDonHangUserMOD>();
				using (SqlConnection SQLCon = new SqlConnection(SQLHelper.appConnectionStrings))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"
                                        select dh.ID_DonHang, dh.OrderID,ctdh.MSanPham,sp.TenSanPham,sp.Picture,ctdh.SoLuong,ctdh.DonGia,ctdh.ThanhTien,dh.PhuongThucThanhToan,dh.NgayMua,dh.Status

                                        from DonHang dh

                                        inner join ChiTietDonHang ctdh on dh.OrderID = ctdh.OrderID
                                        left join SanPham sp on ctdh.MSanPham = sp.MSanPham

										ORDER BY ID_DonHang
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
                        LichSuDonHangUserMOD item = new LichSuDonHangUserMOD();
						item.ID_DonHang = read.GetInt32(0);
                        item.OrderID = read.GetString(1);
                        item.MsanPham=read.GetString(2);
                        item.TenSanPham=read.GetString(3);


                        string picture = read.GetString(4);
                        if (picture.EndsWith(".jpg") || picture.EndsWith(".png") || picture.EndsWith(".gif"))
                        {
                            item.Picture = "https://localhost:7177/" + read.GetString(4);
                        }
                        else
                        {
                            // nếu ko phải là kiểu ảnh thì là base64
                            item.Picture = read.GetString(4);
                        }
                        item.SoLuong = read.GetInt32(5);
                        item.DonGia = read.GetInt32(6);
                        item.ThanhTien = read.GetInt32(7);
                        item.PhuongThucThanhToan = read.GetString(8);
                        item.NgayMua = read.GetDateTime(9);
                        item.Status = read.GetString(10);

                        lsdhuser.Add(item);
					}
					read.Close();
					result.Status = 1;
					result.Data = lsdhuser;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}



        public List<LichSuDonHangUserMOD> CT_LSMuahang(int page, string iduser)
        {
            const int productperpage = 20;
            int startpage = productperpage * (page - 1);
            var result = new BaseResultMOD();
            try
            {
                List<LichSuDonHangUserMOD> lsdhuser = new List<LichSuDonHangUserMOD>();
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
                cmd.CommandText = @"SELECT dh.ID_DonHang, dh.OrderID, ctdh.MSanPham, sp.TenSanPham, sp.Picture, ctdh.SoLuong, ctdh.DonGia, ctdh.ThanhTien, dh.PhuongThucThanhToan, dh.NgayMua, dh.Status
                            FROM DonHang dh
                            INNER JOIN ChiTietDonHang ctdh ON dh.OrderID = ctdh.OrderID
                            LEFT JOIN SanPham sp ON ctdh.MSanPham = sp.MSanPham 
                            WHERE dh.iduser = @iduser
                            ORDER BY ID_DonHang
                            OFFSET @StartPage ROWS
                            FETCH NEXT @ProductPerPage ROWS ONLY;";
                cmd.Parameters.AddWithValue("@StartPage", startpage);
                cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
                cmd.Parameters.AddWithValue("@iduser", iduser);
                cmd.Connection = SQLCon;
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    LichSuDonHangUserMOD item = new LichSuDonHangUserMOD();

                    if (!read.IsDBNull(0))
                    {
                        item.ID_DonHang = read.GetInt32(0);
                    }

                    item.OrderID = read.GetString(1);
                    item.MsanPham = read.GetString(2);
                    item.TenSanPham = read.GetString(3);

                    string picture = read.GetString(4);
                    if (picture.EndsWith(".jpg") || picture.EndsWith(".png") || picture.EndsWith(".gif"))
                    {
                        item.Picture = "https://localhost:7177/" + read.GetString(4);
                    }
                    else
                    {
                        item.Picture = read.GetString(4);
                    }
                    item.SoLuong = read.GetInt32(5);
                    item.DonGia = read.GetInt32(6);
                    item.ThanhTien = read.GetInt32(7);
                    item.PhuongThucThanhToan = read.GetString(8);
                    item.NgayMua = read.GetDateTime(9);
                    item.Status = read.GetString(10);
                    lsdhuser.Add(item);
                }
                read.Close();

                return lsdhuser;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
