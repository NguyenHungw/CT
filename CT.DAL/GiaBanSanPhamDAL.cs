using CT.MOD;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.DAL
{
	public class GiaBanSanPhamDAL
	{
		private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getdsgiaBan(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<GiaBanSanPhamMOD> dsdv = new List<GiaBanSanPhamMOD>();
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"SELECT *
										FROM GiaBanSanPham
										ORDER BY ID_GiaBan
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
                        GiaBanSanPhamMOD item = new GiaBanSanPhamMOD();
						item.ID_GiaBan = read.GetInt32(0);
						item.MSanPham = read.GetString(1);
						item.NgayBatDau = read.GetDateTime(2);
						item.GiaBan = read.GetDecimal(3);

						dsdv.Add(item);
					}
					read.Close();
					result.Status = 1;
					result.Data = dsdv;


				}
			}
			catch (Exception ex)
			{
				result.Status = -1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}
		public BaseResultMOD SuaGiaBan(ThemGiaBanSanPham item)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();

					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.Connection = SQLCon;
					cmd.CommandText = "UPDATE [GiaBanSanPham] SET NgayBatDau=@NgayBatDau,GiaBan=@GiaBan where MSanPham=@MSanPham";
					cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
					cmd.Parameters.AddWithValue("@NgayBatDau", item.NgayBatDau);
					cmd.Parameters.AddWithValue("@GiaBan", item.GiaBan);
					

					cmd.ExecuteNonQuery();
					result.Status = 1;
					result.Message = "Chỉnh sửa thông tin thành công";
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

		public GiaBanSanPhamMOD ThongTinGiaBanSP(int id)
		{
            GiaBanSanPhamMOD item = null;

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
				cmd.CommandText = "SELECT MSanPham from GiaBanSanPham where MSanPham ='" + id + "'";
				cmd.Connection = SQLCon;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					item = new GiaBanSanPhamMOD();
					item.MSanPham = reader.GetString(0);
				}
				reader.Close();
			}
			catch (Exception)
			{
				throw;
			}
			return item;
		}

		public BaseResultMOD ThemGiaBanSP(ThemGiaBanSanPham item)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "INSERT INTO GiaBanSanPham (MSanPham,NgayBatDau,GiaBan) VALUES (@MSanPham,@NgayBatDau,@GiaBan)";
					cmd.Parameters.AddWithValue("@MSanPham", item.MSanPham);
					cmd.Parameters.AddWithValue("@NgayBatDau", item.NgayBatDau);
                    cmd.Parameters.AddWithValue("@GiaBan", item.GiaBan);

                    cmd.ExecuteNonQuery();

					result.Status = 1;
					result.Message = "Thêm mới giá bán thành công";
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
		public BaseResultMOD XoaGiaBan(int id)
		{
			var result = new BaseResultMOD();
			try
			{
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.Connection = SQLCon;
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "DELETE from GiaBanSanPham where MSanPham=@MSanPham";
					cmd.Parameters.AddWithValue("@MSanPham", id);
					cmd.ExecuteReader();

					result.Status = 1;
					result.Message = "Xóa giá bán thành công";

				}

			}
			catch (Exception ex)
			{
				result.Status = 1;
				result.Message = ULT.Constant.API_Error_System;
			}
			return result;
		}
	}
}
