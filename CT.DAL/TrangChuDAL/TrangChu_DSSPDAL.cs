using CT.MOD;
using CT.MOD.TrangChuMOD;
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
	public class TrangChu_DSSPDAL
	{
		private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getdssp(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<TrangChu_DSSPMOD> dssp = new List<TrangChu_DSSPMOD>();
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"select sp.id,sp.MSanPham,sp.Picture , sp.TenSanPham ,dgsp.DiemDanhGia, gbsp.GiaBan
										from SanPham sp
										left join GiaBanSanPham gbsp on sp.MSanPham = gbsp.MSanPham
										left join DanhGiaSanPham dgsp on sp.MSanPham = dgsp.MSanPham
										ORDER BY id
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
                        TrangChu_DSSPMOD item = new TrangChu_DSSPMOD();
						item.id = read.GetInt32(0);
						item.MaSanPham = read.GetString(1);
                        string picture = read.GetString(2);
                        if (picture.EndsWith(".jpg") || picture.EndsWith(".png") || picture.EndsWith(".gif"))
                        {
                            item.Picture = "https://localhost:7177/" + read.GetString(2);
                        }
                        else
                        {
                            // nếu ko phải là kiểu ảnh thì là base64
                            item.Picture = read.GetString(2);
                        }
                        item.TenSanPham = read.GetString(3);
                        if (!read.IsDBNull(4))
                        {
                            item.DiemDanhGia = Convert.ToInt32(read.GetValue(4));
                        }
                        else
                        {
                            item.DiemDanhGia = null;
                        }

                        if (!read.IsDBNull(5))
                        {
                            item.Giaban = Convert.ToDecimal(read.GetValue(5));
                        }
                        else
                        {
                            item.Giaban = null;
                        }

                        dssp.Add(item);
					}
					read.Close();
					result.Status = 1;
					result.Data = dssp;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}
	

		public DanhGiaSanPhamMOD ThongTinDanhGia(int id)
		{
            DanhGiaSanPhamMOD item = null;

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
				cmd.CommandText = "SELECT id from DanhGiaSanPham where id ='" + id + "'";
				cmd.Connection = SQLCon;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					item = new DanhGiaSanPhamMOD();
					item.id = reader.GetInt32(0);
				}
				reader.Close();
			}
			catch (Exception)
			{
				throw;
			}
			return item;
		}

        public TrangChu_CTSPMOD CTSP(string msp)
        {
            TrangChu_CTSPMOD item = null;
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
                cmd.CommandText = @"select sp.id,sp.MSanPham as MaSanPham ,sp.Picture , sp.TenSanPham,dgsp.idUser,u.Username,dgsp.DiemDanhGia,dgsp.NhanXet,dgsp.NgayDanhGia, gbsp.GiaBan
									from SanPham sp
									left join GiaBanSanPham gbsp on sp.MSanPham = gbsp.MSanPham
									left join DanhGiaSanPham dgsp on sp.MSanPham = dgsp.MSanPham
									left join [User] u on dgsp.idUser = u.idUser
                                    where sp.MSanPham = @MSanPham;";
                cmd.Parameters.AddWithValue("@MSanPham", msp);
                cmd.Connection = SQLCon;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    item = new TrangChu_CTSPMOD();
					item.id = reader.GetInt32(0);
                    item.MaSanPham = msp;
                    string picture = reader.GetString(2);
                    if (picture.EndsWith(".jpg") || picture.EndsWith(".png") || picture.EndsWith(".gif"))
                    {
                        item.Picture = "https://localhost:7177/" + reader.GetString(2);
                    }
                    else
                    {
                        // nếu ko phải là kiểu ảnh thì là base64
                        item.Picture = reader.GetString(1);
                    }
                    item.TenSanPham = reader.GetString(3);

                    if (!reader.IsDBNull(4))
                    {
                        item.idUser =Convert.ToInt32(reader.GetValue(4));   
                    }
                    else
                    {
                        item.idUser = null;
                    }
                    if (!reader.IsDBNull(5))
                    {
                        item.Username = Convert.ToString(reader.GetValue(5));
                    }
                    else
                    {
                        item.Username = null;
                    }

                    if (!reader.IsDBNull(6))
                    {
                        item.DiemDanhGia = Convert.ToInt32(reader.GetValue(6));
                    }
                    else
                    {
                        item.DiemDanhGia = null;
                    }

                    if (!reader.IsDBNull(7))
                    {
                        item.NhanXet = Convert.ToString(reader.GetValue(7));
                    }
                    else
                    {
                        item.NhanXet = null;
                    }

                    if (!reader.IsDBNull(8))
                    {
                        item.NgayDanhGia = Convert.ToDateTime(reader.GetValue(8));
                    }
                    else
                    {
                        item.NgayDanhGia = null;
                    }

                    if (!reader.IsDBNull(9))
                    {
                        item.GiaBan = Convert.ToDecimal(reader.GetValue(9));
                    }
                    else
                    {
                        item.GiaBan = null;
                    }

                  

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
