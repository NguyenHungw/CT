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
	public class NhaCungCapDAL
	{
		private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";
		SqlConnection SQLCon = null;
		public BaseResultMOD getdsNCC(int page)
		{
			const int productperpage = 20;
			int startpage = productperpage * (page - 1);
			var result = new BaseResultMOD();
			try
			{
				List<DanhSachNhaCungCapMOD> dsdv = new List<DanhSachNhaCungCapMOD>();
				using (SqlConnection SQLCon = new SqlConnection(strcon))
				{
					SQLCon.Open();
					SqlCommand cmd = new SqlCommand();
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = @"SELECT *
										FROM NhaCungCap
										ORDER BY id_NhaCungCap
										OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;";

					cmd.Parameters.AddWithValue("@StartPage", startpage);
					cmd.Parameters.AddWithValue("@ProductPerPage", productperpage);
					cmd.Connection = SQLCon;
					cmd.ExecuteNonQuery();
					SqlDataReader read = cmd.ExecuteReader();
					while (read.Read())
					{
						DanhSachNhaCungCapMOD item = new DanhSachNhaCungCapMOD();
						item.id_NhaCungCap = read.GetInt32(0);
						item.TenNhaCungCap = read.GetString(1);
						item.NgayHopTac = read.GetDateTime(2);

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
		public BaseResultMOD SuaDonVi(DanhSachNhaCungCapMOD item)
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
					cmd.CommandText = "UPDATE [NhaCungCap] SET TenNhaCungCap=@TenNhaCungCap,NgayHopTac=@NgayHopTac where id_NhaCungCap=@id_NhaCungCap";
					cmd.Parameters.AddWithValue("@id_NhaCungCap", item.id_NhaCungCap);
					cmd.Parameters.AddWithValue("@TenNhaCungCap", item.TenNhaCungCap);
					cmd.Parameters.AddWithValue("@NgayHopTac", item.NgayHopTac);
					

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

		public DanhSachNhaCungCapMOD ThongTinDonVi(int id)
		{
            DanhSachNhaCungCapMOD item = null;

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
				cmd.CommandText = "SELECT id_NhaCungCap from NhaCungCap where id_NhaCungCap ='" + id + "'";
				cmd.Connection = SQLCon;
				SqlDataReader reader = cmd.ExecuteReader();
				while (reader.Read())
				{
					item = new DanhSachNhaCungCapMOD();
					item.id_NhaCungCap = reader.GetInt32(0);
				}
				reader.Close();
			}
			catch (Exception)
			{
				throw;
			}
			return item;
		}

		public BaseResultMOD ThemDonVi(ThemMoiNhaCC item)
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
					cmd.CommandText = "INSERT INTO NhaCungCap (TenNhaCungCap,NgayHopTac) VALUES (@TenNhaCungCap,@NgayHopTac)";
					cmd.Parameters.AddWithValue("@TenNhaCungCap", item.TenNhaCungCap);
					cmd.Parameters.AddWithValue("@NgayHopTac", DateTime.UtcNow);
					cmd.ExecuteNonQuery();

					result.Status = 1;
					result.Message = "Thêm mới đơn vị thành công";
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
		public BaseResultMOD XoaDonVi(int id)
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
					cmd.CommandText = "DELETE from DonViTinh where id_NhaCungCap=@id_NhaCungCap";
					cmd.Parameters.AddWithValue("@id_NhaCungCap", id);
					cmd.ExecuteReader();

					result.Status = 1;
					result.Message = "Xóa đơn vị thành công";

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
